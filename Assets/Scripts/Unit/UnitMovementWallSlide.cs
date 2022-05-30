using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

[RequireComponent(typeof(UnitMain))]

class UnitMovementWallSlide : MonoBehaviour
{
	[SerializeField] UnitMain uMain;
	[SerializeField] private float wallSlideSpeed = 1f;
	[SerializeField] private float wallSlideSpeedFast = 3f;
	public float WallSlideMultiplier
	{
		get => wallSlideSpeed;
	}

	[SerializeField] private SLIDECONTROL slideControlType;
	[SerializeField] private GameObject m_WallSlideDust;

	[Header("SFX")]
	[SerializeField] private string slideOnSFX = "";
	[SerializeField] private string slideOffSFX = "";

	private EventInstance slideSFX;
	private List<UNITSTATE> StatesReadyToWallSlide = new List<UNITSTATE> 
	{
		UNITSTATE.FALL,
	};

	void Update()
    {
        if (uMain.uCollisions.WallInFrontX() && uMain.uCollisions.WallForSlideInFront() && StatesReadyToWallSlide.Contains(uMain.uState.CurrentState))
		{
			switch (slideControlType)
			{
				case SLIDECONTROL.HOLD: 
					if (uMain.uMovement.InputDirection.x * (int)uMain.uMovement.CurrentDirection > 0.1f)
                    {
						GrabWall();
						return;
                    }
					break;
				case SLIDECONTROL.PRESS:
					if (uMain.uMovement.InputDirection.x * (int)uMain.uMovement.CurrentDirection > 0.1f && uMain.rb.velocity.y < 1f)
					{
						GrabWall();
						return;
					}
					break;
				case SLIDECONTROL.AUTO:
					if (uMain.rb.velocity.y < 0.1f)
					{
						GrabWall();
						return;
					}
					break;
			}
		}


		if (uMain.uState.CurrentState == UNITSTATE.WALLSLIDE)
		{
			if (uMain.uMovement.InputDirection.y < -0.1f && slideControlType != SLIDECONTROL.HOLD)
			{
				uMain.rb.velocity = Vector3.down * wallSlideSpeedFast;
            }
            else
            {
				uMain.rb.velocity = Vector3.down * wallSlideSpeed;
            }


			switch (slideControlType)
			{
				case SLIDECONTROL.HOLD:
					if (!(uMain.uMovement.InputDirection.x * (int)uMain.uMovement.CurrentDirection > 0.1f))
					{
						StopGrabWall();
						return;
					}
					break;
				case SLIDECONTROL.PRESS:
					if (uMain.uMovement.InputDirection.x * (int)uMain.uMovement.CurrentDirection < -0.1f )
					{
						StopGrabWall();
						return;
					}
					break;
				case SLIDECONTROL.AUTO:
					if (uMain.uMovement.InputDirection.x * (int)uMain.uMovement.CurrentDirection < -0.1f )
					{
						StopGrabWall();
						return;
					}
					break;
			}
		}
		
		if (uMain.uState.CurrentState == UNITSTATE.WALLSLIDE && !uMain.uCollisions.WallForSlideInFront())
		{
			if (slideSFX.isValid())
			{
				uMain.uSFX.StopSFX(slideSFX);
			}
			uMain.uSFX.PlayOneShotSFX(slideOffSFX);
			uMain.uMovement.ResetState();
			return;
		}
		if (uMain.uState.CurrentState != UNITSTATE.WALLSLIDE && slideSFX.isValid())
		{
				uMain.uSFX.StopSFX(slideSFX);
		}
	}

	void AE_WallSlide()
	{
		float dustXOffset = uMain.bc.bounds.size.x / 2;
		float dustYOffset = uMain.bc.bounds.size.y / 2 - uMain.bc.offset.y;
		uMain.SpawnVFX(m_WallSlideDust, 0, false, dustXOffset, dustYOffset);
	}

	void GrabWall()
    {
		uMain.uState.CurrentState = UNITSTATE.WALLSLIDE;
		uMain.uAnimator.SetAnimatorTrigger("WallSlide");

		if (!slideSFX.isValid() && slideOnSFX != null)
		{
			slideSFX = uMain.uSFX.CreateSFX(slideOnSFX);
			uMain.uSFX.PlaySFX(slideSFX);
		}
	}	
	
	void StopGrabWall()
    {
		if (slideSFX.isValid())
		{
			uMain.uSFX.StopSFX(slideSFX);
		}
		uMain.uSFX.PlayOneShotSFX(slideOffSFX);

		uMain.uMovement.ResetState();
	}

	public enum SLIDECONTROL
    {
		HOLD,
		PRESS,
		AUTO
    }
}