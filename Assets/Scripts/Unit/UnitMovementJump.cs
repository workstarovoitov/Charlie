using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UnitMain))]

class UnitMovementJump : MonoBehaviour
{
    [SerializeField] UnitMain uMain;
    [SerializeField] private int jumpIterationsNum = 5;
    [SerializeField] private int jumpWallIterationsNum = 3;
    [SerializeField] private int jumpSecondIterationsNum = 3;
    [SerializeField] private float lowStaminaSpeeedMultiplier = 0.7f;

    [SerializeField] private float defaultJumpSpeed = 10f;
    [SerializeField] private float holdJumpSpeed = 10f;
    [SerializeField] private float SecondJumpSpeed = 10f;
    [SerializeField] private float WallJumpSpeedX = 10f;
    [SerializeField] private float WallJumpSpeedY = 10f;
    [SerializeField] private bool allowDoubleJump = true;
    [SerializeField] private float staminaCost = 10f;

    [SerializeField] private string jumpSFX;

    private bool secondJumpDone = false;
    public bool SecondJumpDone
    {
        get => secondJumpDone;
        set => secondJumpDone = value;
    }
    
    private bool jumpBttnHold = false;
    
    private bool jumpPerformed = false;
    private bool jumpWallPerformed = false;
    private bool jumpSecondPerformed = false;
    
    private int wallJumpDirection;
    private float jumpSpeedMultiplayer;
    
    private GameObject platform = null;
    
    [SerializeField] private GameObject m_JumpDust;
    [SerializeField] private GameObject m_WallJumpDust;

    private List<UNITSTATE> StatesReadyToJump = new List<UNITSTATE>
    {
        UNITSTATE.IDLE,
        UNITSTATE.WALK,
        UNITSTATE.SLIDE,
        UNITSTATE.DEFEND,
        UNITSTATE.LAND,
    };
    private List<UNITSTATE> StatesReadyToWallJump = new List<UNITSTATE>
    {
        UNITSTATE.WALLSLIDE,
        UNITSTATE.GRABSTAIRS,
    };
    private List<UNITSTATE> StatesReadyToSecondJump = new List<UNITSTATE>
    {
        UNITSTATE.FALL,
        UNITSTATE.JUMP,
        UNITSTATE.WALLJUMP,
    };
    private List<UNITSTATE> StatesJump = new List<UNITSTATE>
    {
        UNITSTATE.SECONDJUMP,
        UNITSTATE.JUMP,
        UNITSTATE.WALLJUMP,
    };

    public void LateUpdate()
    {
        if (uMain.uStamina.CurrentStamina > staminaCost * 0.1f)
        {
            jumpSpeedMultiplayer = 1f;
        }
        else
        {
            jumpSpeedMultiplayer = lowStaminaSpeeedMultiplier;
        }

        if (jumpPerformed)
        {
            uMain.rb.velocity = new Vector2(uMain.rb.velocity.x, defaultJumpSpeed * jumpSpeedMultiplayer);
            StartCoroutine(AddForceJumpCoroutine());
           
            jumpPerformed = false;
        }

        if (jumpWallPerformed)
        {
            uMain.rb.velocity = new Vector2(wallJumpDirection * WallJumpSpeedX * jumpSpeedMultiplayer, WallJumpSpeedY * jumpSpeedMultiplayer);
            StartCoroutine(AddForceJumpWallCoroutine());
            
            jumpWallPerformed = false;
        }   
        
        if (jumpSecondPerformed)
        {
            uMain.rb.velocity = new Vector2(uMain.rb.velocity.x, SecondJumpSpeed * jumpSpeedMultiplayer);
            StartCoroutine(AddForceJumpSecondCoroutine());
            
            jumpSecondPerformed = false;
        }
    }

    public void OnJump(InputValue input)
    {
        if (Mathf.RoundToInt(input.Get<float>()) > 0.1f)
        {
            jumpBttnHold = true;

            //jump down from platform
            if (uMain.uState.CurrentGroundedState && uMain.uState.OnPlatform && uMain.uMovement.InputDirection.y < 0f)
            {
                platform = uMain.uCollisions.Platform;
                Physics2D.IgnoreCollision(transform.gameObject.GetComponent<Collider2D>(), platform.GetComponent<Collider2D>());
                uMain.uState.CurrentState = UNITSTATE.JUMPDOWN;
                uMain.uAnimator.SetAnimatorTrigger("Fall");
                StartCoroutine(OffSkipPlatformCorutine());
                return;
            }

            //default jump
            if (StatesReadyToJump.Contains(uMain.uState.CurrentState))
            {
                uMain.uStamina.StaminaDecrease(staminaCost);

                uMain.uState.CurrentState = UNITSTATE.JUMP;
                uMain.uAnimator.SetAnimatorTrigger("Jump");
                return;
            }

            //second jump
            if (StatesReadyToSecondJump.Contains(uMain.uState.CurrentState) && !secondJumpDone && allowDoubleJump)
            {
                uMain.uStamina.StaminaDecrease(staminaCost);
                uMain.uState.CurrentState = UNITSTATE.SECONDJUMP;
                uMain.uAnimator.SetAnimatorTrigger("JumpSecond");
                jumpSecondPerformed = true;
                secondJumpDone = true;
                return;
            }

            //wall jump
            if (StatesReadyToWallJump.Contains(uMain.uState.CurrentState))
            {
                uMain.uStamina.StaminaDecrease(staminaCost);
                uMain.uMovement.ReverseDirection();
                uMain.uState.CurrentState = UNITSTATE.WALLJUMP;
                uMain.uAnimator.SetAnimatorTrigger("JumpWall");
                return;
            }
        }
        else
        {
            jumpBttnHold = false;
        }
    }

    public void AE_AddJumpForce()
    {
        jumpPerformed = true;
        
        float dustYOffset = -(uMain.bc.bounds.size.y / 2);
        uMain.SpawnVFX(m_JumpDust, 0, false, 0.0f, dustYOffset);
        uMain.uSFX.PlayOneShotSFX(jumpSFX);
    }

    public void AE_AddWallJumpForce()
    {
        wallJumpDirection = (int)Mathf.Sign((int)uMain.uMovement.CurrentDirection);
        jumpWallPerformed = true;

        uMain.SpawnVFX(m_WallJumpDust);
        uMain.uSFX.PlayOneShotSFX(jumpSFX);
    } 
    public void AE_OffJumpAmimation()
    {
        uMain.uMovement.ResetState();
    }

    IEnumerator AddForceJumpCoroutine()
    {
        int i = 0;
        yield return new WaitForFixedUpdate();
        while ( (jumpBttnHold && i < jumpIterationsNum && uMain.uStamina.CurrentStamina > 1) || (jumpBttnHold && i < jumpIterationsNum / 2 && uMain.uStamina.CurrentStamina < 1) )
        {
            if (StatesJump.Contains(uMain.uState.CurrentState))
            {
                uMain.rb.velocity = new Vector3(uMain.rb.velocity.x, holdJumpSpeed * jumpSpeedMultiplayer);
            }                
            i++;

            yield return new WaitForFixedUpdate();
        }
        uMain.uMovement.ResetState();
    }
       
    IEnumerator AddForceJumpWallCoroutine()
    {
        int i = 0;
        yield return new WaitForFixedUpdate();
        while (i < jumpWallIterationsNum)
        {
            if (StatesJump.Contains(uMain.uState.CurrentState))
            {
                uMain.rb.velocity = new Vector3(wallJumpDirection * WallJumpSpeedX * jumpSpeedMultiplayer, WallJumpSpeedY * jumpSpeedMultiplayer);
            }                
            i++;

            yield return new WaitForFixedUpdate();
        }
        uMain.uMovement.ResetState();
    }
    
    IEnumerator AddForceJumpSecondCoroutine()
    {
        int i = 0;
        yield return new WaitForFixedUpdate();
        while (i < jumpSecondIterationsNum)
        {
            if (StatesJump.Contains(uMain.uState.CurrentState))
            {
                uMain.rb.velocity = new Vector3(uMain.rb.velocity.x, SecondJumpSpeed * jumpSpeedMultiplayer);
            }                
            i++;

            yield return new WaitForFixedUpdate();
        }
        //uMain.uMovement.ResetState();
    }

    IEnumerator OffSkipPlatformCorutine()
    {
        while (uMain.uCollisions.IsTouchingPlatform(platform))
        {
            if (uMain.uCollisions.IsTouchingNewPlatform(platform) && uMain.uState.CurrentState == UNITSTATE.JUMPDOWN)
            {
                uMain.uState.CurrentState = UNITSTATE.IDLE;
            }
            yield return new WaitForFixedUpdate();
        }
        if (uMain.uState.CurrentState == UNITSTATE.JUMPDOWN)
        {
            uMain.uState.CurrentState = UNITSTATE.FALL;
        }
        Physics2D.IgnoreCollision(transform.gameObject.GetComponent<Collider2D>(), platform.GetComponent<Collider2D>(), false);
    }
}