using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UnitMain))]

public class UnitMovementDash : SpecialMove
{
	[SerializeField] UnitMain uMain;

	[SerializeField] private LayerMask invictibleLayer;
	[SerializeField] private bool allowAirDash = false;
	[SerializeField] private float dashSpeedX = 6f;
	[SerializeField] private float dashCooldown = 0.1f;
	[SerializeField] private float staminaCost;

	[SerializeField] private string dashSFX;
	[SerializeField] private GameObject m_DashDust;
	[SerializeField] private GameObject m_DashStopDust;
	private float dashOffTime = 0;
	
	private bool airDashDone = false;
	public bool AirDashDone
	{
		get => airDashDone;
		set => airDashDone = value;
	}

	public InputAction dashAction;

	public override bool IsSpecialMoveFaced()
	{
		return false;
	}

	public override void OnSpecialMove()
	{
		if (!uMain.uState.StatesReadyToDash.Contains(uMain.uState.CurrentState) || Time.time - dashOffTime < dashCooldown || uMain.uStamina.CurrentStamina < staminaCost) return;
		uMain.uMovement.UpdateDirection();
		uMain.uState.CurrentState = UNITSTATE.DASH;
		uMain.uAnimator.SetAnimatorTrigger("Dash");
		uMain.uStamina.StaminaDecrease(staminaCost);
	}

	public void OnDash()
	{
		if ((uMain.uState.CurrentGroundedState || (allowAirDash && !airDashDone)) && uMain.uState.CurrentState == UNITSTATE.ATTACK && !uMain.uMovementAttack.BttnMesh)
		{
			StartCoroutine(TryToDash());
			return;
		}
		if (Time.time - dashOffTime > dashCooldown)
        {
			if ((uMain.uState.CurrentGroundedState && uMain.uState.StatesReadyToDash.Contains(uMain.uState.CurrentState)) ||
				(allowAirDash && !airDashDone && !uMain.uState.CurrentGroundedState && uMain.uState.StatesReadyToAirDash.Contains(uMain.uState.CurrentState)))
            {
				StartDash();
			}
        }
	}

	private void StartDash()
	{
		if (!uMain.uState.CurrentGroundedState)
        {
			airDashDone = true;
		}
		if(uMain.uStamina.CurrentStamina < staminaCost * 0.1f)
        {
			uMain.uStamina.LowStamina = true;
			return;
		}
		if(uMain.uStamina.CurrentStamina < staminaCost)
        {
			uMain.uStamina.LowStamina = true;
		}

		uMain.uMovement.UpdateDirection();
		uMain.uState.CurrentState = UNITSTATE.DASH;
		uMain.uAnimator.SetAnimatorTrigger("Dash");
		uMain.uStamina.StaminaDecrease(staminaCost);
	}

	IEnumerator TryToDash()
	{
		//check for hit
		while (uMain.uState.CurrentState == UNITSTATE.ATTACK)
		{
			yield return null;
		}
		if (!uMain.uMovementAttack.BttnMesh)
		{
			StartDash();
		}
	}

	void AE_Dash()
	{
		int dir = (int)uMain.uMovement.CurrentDirection;
		uMain.rb.velocity = new Vector2(dir * (dashSpeedX /*+ Mathf.Abs(uMain.rb.velocity.x)*/), uMain.rb.velocity.y);

		uMain.uSFX.PlayOneShotSFX(dashSFX);
		gameObject.layer = (int)Mathf.Log(invictibleLayer.value, 2);

		float dustYOffset = -(uMain.bc.bounds.size.y / 2 + uMain.bc.offset.y / 2);
		uMain.SpawnVFX(m_DashDust, 0, false, 0.0f, dustYOffset);
	}

	void AE_ResetDash()
	{
		dashOffTime = Time.time;
		gameObject.layer = uMain.uState.DefaultLayer;

		float dustXOffset = -uMain.bc.bounds.size.x / 2;
		float dustYOffset = -(uMain.bc.bounds.size.y / 2 + uMain.bc.offset.y / 2);
		uMain.SpawnVFX(m_DashStopDust, 0, false, dustXOffset, dustYOffset);
	}
}
