using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(UnitMain))]

public class BossArcherHideSequence : SpecialAttack
{
	[SerializeField] UnitMain uMain;
	[SerializeField] EnemyActions eActions;

	[SerializeField] private LayerMask invictibleLayer;
	
	[SerializeField] private float hideTime = 2f;

	[SerializeField] private Vector2 JumpSpeed;
	[SerializeField] private float staminaCost;
	[SerializeField] private int comboNum;

	[SerializeField] private Transform[] spawnPoints;
	[SerializeField] private string jumpSFX;
	[SerializeField] private string hideSFX;
	[SerializeField] private string unhideSFX;

	[SerializeField] private GameObject m_JumpDust;


	private List<UNITSTATE> StatesReadyToHide = new List<UNITSTATE> 
	{
		UNITSTATE.IDLE,
		UNITSTATE.WALK,
		UNITSTATE.SLIDE,
		UNITSTATE.DEFEND,
	};

	public override void OnSpecialAttack()
	{
		OnHide();
	}

	public void OnHide()
    {
        //default jump
        if (StatesReadyToHide.Contains(uMain.uState.CurrentState))
        {
            uMain.uState.CurrentState = UNITSTATE.JUMP;
            uMain.uAnimator.SetAnimatorTrigger("Jump");
            return;
        }
    }

    public void AE_AddJumpForce()
    {
        uMain.rb.velocity = new Vector2((int)uMain.uMovement.CurrentDirection * JumpSpeed.x * 0.5f, JumpSpeed.y);

        float dustYOffset = -(uMain.bc.bounds.size.y / 2 + uMain.bc.offset.y / 2);
        uMain.SpawnVFX(m_JumpDust, 0, false, 0.0f, dustYOffset);

        if (staminaCost > 0.1f)
        {
            uMain.uStamina.StaminaDecrease(staminaCost);
        }

        uMain.uSFX.PlayOneShotSFX(jumpSFX);
    }

    public void AE_StartHideAnimation()
    {
		uMain.uState.CurrentState = UNITSTATE.HIDE;
		uMain.uAnimator.SetAnimatorTrigger("Hide");
		gameObject.layer = (int)Mathf.Log(invictibleLayer.value, 2);


		uMain.uSFX.PlayOneShotSFX(hideSFX);
		Invoke(nameof(UnhideSequence), hideTime);
	}
	
	public void UnhideSequence()
    {		
		transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
		uMain.rb.velocity = Vector2.zero;

		eActions.LookAtTarget(eActions.targetAttack.transform);

		uMain.uAnimator.SetAnimatorTrigger("Unhide");
		gameObject.layer = uMain.uState.DefaultLayer;
	}

	void AE_ResetUnhide()
	{
		ShootObject to = (ShootObject)uMain.uMovementAttack.ComboList[comboNum].ComboSequence[0];
		eActions.LookAtTarget(eActions.targetAttack.transform);
		to.ShootDirection = (int)uMain.uMovement.CurrentDirection;
		Vector2 dir = eActions.targetAttack.transform.position - transform.position;
		to.AngleType = ANGLETYPE.LOW;
		to.ShootAngle = (dir - Vector2.right * to.ColOffsetLow.x * to.ShootDirection - Vector2.up * to.ColOffsetLow.y).normalized;
		uMain.uMovementAttack.tryComboAttack(uMain.uMovementAttack.ComboList[comboNum].ComboSequence[0].ActionName, comboNum);
	}
}
