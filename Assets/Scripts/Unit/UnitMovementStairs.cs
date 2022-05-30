using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UnitMain))]

class UnitMovementStairs : MonoBehaviour
{
	[SerializeField] UnitMain uMain;

	[SerializeField] private GameObject m_WallSlideDust;
	
	[SerializeField] private float ladderMoveSpeed = 3f;

	[SerializeField] private float grabXOffset = 0.1f;
	[SerializeField] private float grabYOffset = 0.2f;
	[SerializeField] private float climbXOffset = 0.7f;
	[SerializeField] private float climbYOffset = 1.12f;

	private bool readyToGrab = true;
	private float inputY = 0;
	
	private List<UNITSTATE> StatesReadyGrabLedge = new List<UNITSTATE> 
	{
		UNITSTATE.FALL,
		UNITSTATE.JUMP,
		UNITSTATE.WALLJUMP,
		UNITSTATE.WALLSLIDE,
	};

	private List<UNITSTATE> StatesReadyGrabStairs = new List<UNITSTATE>
	{
		UNITSTATE.IDLE,
		UNITSTATE.WALK,
		UNITSTATE.SLIDE,
		UNITSTATE.FALL,
		UNITSTATE.JUMP,
		UNITSTATE.WALLJUMP,
		UNITSTATE.WALLSLIDE,
	};

	void FixedUpdate()
    {
		RaycastHit2D ledge = uMain.uCollisions.LedgeInFrontX();
		if (readyToGrab && ledge && StatesReadyGrabLedge.Contains(uMain.uState.CurrentState) && !uMain.uCollisions.PlatformInBackX())
		{
			uMain.rb.velocity = Vector2.zero;
			uMain.uState.CurrentState = UNITSTATE.GRABLEDGE;
			uMain.uAnimator.SetAnimatorTrigger("GrabLedge");
			transform.position = ledge.transform.position + Vector3.right * grabXOffset * (int)uMain.uMovement.CurrentDirection + Vector3.up * grabYOffset;

			uMain.uSFX.PlayOneShotSFX("event:/move/ledge_grab");
		}

		if (readyToGrab && ledge && uMain.uState.CurrentState == UNITSTATE.GRABSTAIRS)
		{
			readyToGrab = false;
			transform.position = ledge.transform.position + Vector3.right * grabXOffset * (int)uMain.uMovement.CurrentDirection + Vector3.up * grabYOffset;
			uMain.uState.CurrentState = UNITSTATE.CLIMBUP;
			uMain.rb.velocity = Vector2.zero;
			uMain.uAnimator.SetAnimatorTrigger("ClimbUp");

			uMain.uSFX.PlayOneShotSFX("event:/move/ledge_climb");
			StartCoroutine(SkipLedge());
		}

		if ( (uMain.uMovement.InputDirection.y > .1f && uMain.uCollisions.StairOnTop() && StatesReadyGrabStairs.Contains(uMain.uState.CurrentState))  
			|| (uMain.uMovement.InputDirection.y < -.1f && uMain.uState.CurrentState == UNITSTATE.GRABLEDGE) )
		{
			readyToGrab = false;
			StartCoroutine(SkipLedge());
			Transform t = uMain.uCollisions.StairOnTop().transform;

			uMain.uMovement.SetDirection((DIRECTION)Mathf.RoundToInt(t.localScale.x));

			uMain.uState.CurrentState = UNITSTATE.GRABSTAIRS;
			uMain.uAnimator.SetAnimatorTrigger("GrabStairs");
			uMain.uMovement.SetVelocity(Vector2.zero);
			transform.position = new Vector3(t.position.x - Mathf.RoundToInt(t.localScale.x) * uMain.bc.bounds.size.x, transform.position.y + uMain.bc.bounds.size.y / 2, transform.position.z);
		}

	}

	void Update()
	{
		if (uMain.uState.CurrentState == UNITSTATE.GRABSTAIRS)
		{
			if (Mathf.Abs(inputY) > .1f)
			{
				uMain.rb.velocity = new Vector2(0, ladderMoveSpeed * inputY);
				uMain.uAnimator.SetAnimatorTrigger("ClimbStairs");
			}
			else
			{
				uMain.uAnimator.SetAnimatorTrigger("GrabStairs");
				uMain.uMovement.SetVelocity(Vector2.zero);
			}
		}
	}



	public void OnMoveVertical(InputValue input)
	{
		inputY = input.Get<float>();
		
		if (Mathf.RoundToInt(input.Get<float>()) < -.1f && uMain.uState.CurrentState == UNITSTATE.GRABLEDGE && !uMain.uCollisions.StairOnTop())
        {
			uMain.uMovement.ResetState();
			readyToGrab = false;
			StartCoroutine(SkipLedge());
        }
		
		if (Mathf.RoundToInt(input.Get<float>()) > .1f && uMain.uState.CurrentState == UNITSTATE.GRABLEDGE)
        {
			uMain.rb.velocity = Vector2.zero;
			uMain.uState.CurrentState = UNITSTATE.CLIMBUP;
            uMain.uAnimator.SetAnimatorTrigger("ClimbUp");
			uMain.uSFX.PlayOneShotSFX("event:/move/ledge_climb");

			StartCoroutine(SkipLedge());
        }
	}

	public void AE_LedgeGrab()
    {
		float dustXOffset = uMain.bc.bounds.size.x / 2;
		float dustYOffset = uMain.bc.bounds.size.y / 4 + uMain.bc.offset.y;
		uMain.SpawnVFX(m_WallSlideDust, 0, false, dustXOffset, dustYOffset);
	}
	IEnumerator SkipLedge()
	{
		while (uMain.uCollisions.LedgeInFrontX())
		{
			yield return new WaitForFixedUpdate();
		}
		readyToGrab = true;
	}

	void AE_SetClimbPosition()
	{
		Vector3 m_climbPosition = transform.position + Vector3.up * climbYOffset + Vector3.right * climbXOffset * (int)uMain.uMovement.CurrentDirection;
		transform.position = m_climbPosition;
	}
	void AE_LedgeClimb()
	{
	}
}