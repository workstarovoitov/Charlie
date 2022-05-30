using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitMovementHit : MonoBehaviour, IDamagable<HitObject>
{
	[SerializeField] UnitMain uMain;

	public bool blockAttacksFromBehind = false;         //block enemy attacks coming from behind

	//public float hitRecoveryTime = .4f;					//the time it takes to recover from a hit
	[Header("Blink settings")]
	[SerializeField] private float hurtTime = .25f;                   //the time before we can get hit again
	[SerializeField] private float iTime = .1f;                   //the time before we can get hit again
	[SerializeField] private float offTime;
	[SerializeField] private float onTime;
	[SerializeField] private float offAlpha;
	[Header("SFX")]
	[SerializeField] private string hurtSFX = "";

	[Header("VFX")]
	[SerializeField] private GameObject[] m_HitVFX;
	[SerializeField] private GameObject m_ParryVFX;
	private float lastHitTime = 0;                      // the last time when we were hit 
	//we are hit
	public void Hit(HitObject d)
	{
		//skip hit if there are iframes
		if (Time.time < lastHitTime + iTime && lastHitTime > 0) return;
		
		//skip if we are not in hitable states
		if (!uMain.uState.StatesReadyToHit.Contains(uMain.uState.CurrentState))
		{
			return;
		}

		int incomeAttackDirection = d.inflictor.transform.position.x > transform.position.x ? -1 : 1;

		//parry incoming attack
		if (uMain.uState.CurrentState == UNITSTATE.PARRY && (d.ParryCooldown > .1f) && isFacingAttacker(d.inflictor))
		{
			uMain.uSFX.PlayOneShotSFX(d.HitBlockSFX);
			if (m_ParryVFX != null)
			{
				uMain.SpawnVFX(m_ParryVFX, incomeAttackDirection, true);
			}
			uMain.uMovement.SetVelocity(Vector2.right * d.KnockBackForce.x * incomeAttackDirection * 0.25f + Vector2.up * d.KnockBackForce.y * 0.25f);
			
			uMain.uState.CurrentState = UNITSTATE.DEFEND;
			uMain.uAnimator.SetAnimatorTrigger("Defend");
			
			// stun sequence
			UnitMain um = d.inflictor.GetComponent<UnitMain>();
			if (um != null)
            {
				um.rb.velocity = Vector2.zero;
				um.uMovementHit.SetStunned(d.ParryCooldown);
					
				EnemyActions ea = um.GetComponent<EnemyActions>();
				if (ea != null) ea.RunStunnedSequence();
			}
			return;
		}

		//defend incoming attack
		if ((uMain.uState.CurrentState == UNITSTATE.DEFEND || uMain.uState.CurrentState == UNITSTATE.PARRY) && !d.DefenceOverride && (isFacingAttacker(d.inflictor) || blockAttacksFromBehind))
		{
			uMain.uSFX.PlayOneShotSFX(d.HitBlockSFX);

			uMain.uMovement.SetVelocity(Vector2.right * d.KnockBackForce.x * incomeAttackDirection * 0.25f + Vector2.up * d.KnockBackForce.y * 0.25f);
			
			if (uMain.uStamina.CurrentStamina > d.Damage)
            {
				uMain.uHealth.HealthDecrease( d.Damage * 0.25f);
            }
            else
            {
				uMain.uHealth.HealthDecrease(d.Damage * 0.75f);
			}
			uMain.uStamina.StaminaDecrease( d.Damage * 2f);
			return;
		}

		//we are hit
		lastHitTime = Time.time;
		Debug.Log("Hit");
		//add a small force from the impact
		uMain.rb.velocity = new Vector2(uMain.rb.velocity.x - d.KnockBackForce.x * (int) uMain.uMovement.CurrentDirection,  d.KnockBackForce.y);

		//play sounds
		uMain.uSFX.PlayOneShotSFX(d.HitSFX);
		uMain.uSFX.PlayOneShotSFX(hurtSFX);
		
		CancelInvoke();
		StopAllCoroutines();

		//implement damadge
		uMain.uHealth.HealthDecrease(d.Damage);
		
		//addition damadge if stunned
		if (uMain.uState.CurrentState == UNITSTATE.STUNNED)
        {
			uMain.uHealth.HealthDecrease(d.Damage);
		}

		//play VFX
		if (d.HitWeight > 0 && m_HitVFX[d.HitWeight-1] != null)
        {
			uMain.SpawnVFX(m_HitVFX[d.HitWeight - 1], incomeAttackDirection, true);
        }
		
		if (uMain.uState.CurrentState == UNITSTATE.ATTACK /*&& uMain.uMovementAttack.AttackIsActive*/)
		{
			return;
		}

		//if (uMain.uState.CurrentState == UNITSTATE.ATTACK && !uMain.uMovementAttack.AttackIsActive && d.KnockDownTime > 0.1f)
		//{				
		//	uMain.uMovementAttack.ResetAttack();
		//}

		if (uMain.uHealth.CurrentHealth <= 0.01f)
		{
			CancelInvoke();
			StopAllCoroutines();

			return;
		}


		if (d.KnockDownTime > 0.1f)
		{
			SetStunned(d.KnockDownTime);
		}
        else
        {
			uMain.uState.CurrentState = UNITSTATE.HITTED;
			//uMain.uAnimator.SetAnimatorTrigger("Hitted");
			
			StartCoroutine(Blink());
		}

	}

	public void SetStunned(float knockdown)
    {
		uMain.uMovementAttack.ResetAttack();
		uMain.uState.CurrentState = UNITSTATE.STUNNED;
		uMain.uAnimator.SetAnimatorTrigger("Stunned");
		Invoke(nameof(ResetState), knockdown);
	}

	public bool isFacingAttacker(GameObject g)
	{
		return ((g.transform.position.x < transform.position.x && uMain.uMovement.CurrentDirection == DIRECTION.LEFT) || (g.transform.position.x > transform.position.x && uMain.uMovement.CurrentDirection == DIRECTION.RIGHT));
	}
	
	private void ResetState()
    {
		uMain.uMovement.ResetState();
	}
	
	IEnumerator ResetHitAnimationCoroutine()
	{
		while (uMain.uState.CurrentState == UNITSTATE.HITTED)
		{
			yield return new WaitForFixedUpdate();
		}
		TryToDefend();
	}

	private void TryToDefend()
    {
		EnemyActions ea = gameObject.GetComponent<EnemyActions>();
		if (ea != null) ea.RunHittedSequence();
	}

	public void AE_StopHitCoroutine()
	{
		uMain.uMovement.ResetState();
	}
	IEnumerator Blink()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		bool fadeOff = true;
		while (Time.time < lastHitTime + iTime)
		{
			if (Time.time > lastHitTime + hurtTime && uMain.uState.CurrentState == UNITSTATE.HITTED)
			{
				uMain.uMovement.ResetState();
			}
				if (fadeOff)
			{
				sr.color = new Color(1f, 1f, 1f, offAlpha);
				yield return new WaitForSeconds(offTime);
			}
			else
			{
				sr.color = new Color(1f, 1f, 1f, 1f);
				yield return new WaitForSeconds(onTime);
			}
			fadeOff = !fadeOff;
		}
		yield return new WaitForFixedUpdate();
		
		sr.color = new Color(1f, 1f, 1f, 1f);
	}

}
