using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UnitMain))]

public class UnitMovementDefend : SpecialMove
{
	
	[SerializeField] UnitMain uMain;
	[SerializeField] private float defendMinTime = 1; //the chance that an incoming attack is defended %

	private float inputDefend = 0f;
	private bool specialMove = false;
	


	public void OnDefend(InputValue input)
	{
		inputDefend = Mathf.RoundToInt(input.Get<float>());
		if (inputDefend > 0.1f)
		{
			if (uMain.uState.CurrentGroundedState && uMain.uState.CurrentState == UNITSTATE.ATTACK && !uMain.uMovementAttack.BttnMesh)
            {
				StartCoroutine(TryToDefend());
				return;
            }
			if (!uMain.uState.StatesReadyToDefend.Contains(uMain.uState.CurrentState)) return;
			StartDefend();
		}
		else
		{
			if (uMain.uState.CurrentState == UNITSTATE.DEFEND )
			{
				uMain.uState.CurrentState = UNITSTATE.IDLE;
				uMain.uAnimator.SetAnimatorTrigger("Idle");
			}
		}
	}

    private void StartDefend()
    {
		uMain.uState.CurrentRunState = false;
		uMain.rb.velocity = new Vector2(uMain.rb.velocity.x * 0.5f, uMain.rb.velocity.y);
		uMain.uState.CurrentState = UNITSTATE.PARRY;
		uMain.uAnimator.SetAnimatorTrigger("Parry");
	}
    private void StopDefend()
    {
		if (uMain.uState.CurrentState == UNITSTATE.DEFEND)
		{
			specialMove = false;
			uMain.uState.CurrentState = UNITSTATE.IDLE;
			uMain.uAnimator.SetAnimatorTrigger("Idle");
			return;
		}
    }


	IEnumerator TryToDefend()
	{
		//check for hit
		while (uMain.uState.CurrentState == UNITSTATE.ATTACK)
		{
			yield return null;
		}
		if (!uMain.uMovementAttack.BttnMesh)
        {
			StartDefend();
        }
	}

	public override void OnSpecialMove()
	{
		if (uMain.uState.CurrentState != UNITSTATE.DEFEND)
		{
			if (!uMain.uState.StatesReadyToDefend.Contains(uMain.uState.CurrentState)) return;
			CancelInvoke(nameof(StopDefend));
			uMain.uState.CurrentRunState = false;
			specialMove = true;
			uMain.uMovement.SetVelocity(Vector3.zero);
			uMain.uState.CurrentState = UNITSTATE.PARRY;
			uMain.uAnimator.SetAnimatorTrigger("Parry");

			Invoke(nameof(StopDefend), defendMinTime);
			return;
		}
	}

    public override bool IsSpecialMoveFaced()
    {
		return true;
    }

    public void AE_Defend()
    {
		if (inputDefend > 0.1f || specialMove)
		{
			uMain.uMovement.SetVelocity(Vector3.zero);
			uMain.uState.CurrentState = UNITSTATE.DEFEND;
			uMain.uAnimator.SetAnimatorTrigger("Defend");
		}
		else
		{
			uMain.uState.CurrentState = UNITSTATE.IDLE;
			uMain.uAnimator.SetAnimatorTrigger("Idle");
		}
	}
}
