using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] UnitMain uMain;
    [SerializeField] private bool enableMovement = true;
    public bool EnableMovement
    {
        get => enableMovement;
        set => enableMovement = value;
    }

    [Header("Input Timing Settings")]
    [SerializeField] private float doubleTapWindow = .3f;
    private Vector2 lastInputDirTime;

    [Header("Settings")]
    [SerializeField] private float gravityScale = 3.0f;
    public float GravityScale
    {
        get => gravityScale;
        set => gravityScale = value;
    }

    [SerializeField] private float AirAcceleration = 3f;
    [SerializeField] private bool runEnabled = true;

    [SerializeField] private float defaultWalkSpeed = 5f;
    [SerializeField] private float defaultRunSpeed = 8f;
    [SerializeField] private float staminaRunCost = 20f;

    [SerializeField] private float defaultWalkOnAcc = 3f;
    [SerializeField] private float defaultWalkOffAcc = 8f;
    [SerializeField] private float defaultRunOnAcc = 5f;
    [SerializeField] private float defaultRunOffAcc = 3f;
    [SerializeField] private float maxFallSpeed = -20f;
    private float fallSpeed = 0f;

    [Header("Effects")]
    [SerializeField] private GameObject m_RunStopDust;
    [SerializeField] private GameObject m_LandingDust;

    private DIRECTION currentDirection = DIRECTION.RIGHT;
    public DIRECTION CurrentDirection
    {
        get => currentDirection;
        set
        {
            currentDirection = value;
        }
    }

    private DIRECTION previousDirection = DIRECTION.RIGHT;
    public DIRECTION PreviousDirection
    {
        get => previousDirection;
        set
        {
            previousDirection = value;
        }
    }

    [Header("SFX")]
    [SerializeField] private string stepSFX = "";
    [SerializeField] private string landSFX = "";


    private float accelerationMultiplier = 0.05f;
    private Vector2 inputDirection;
    public Vector2 InputDirection
    {
        get => inputDirection;
    }
    private bool windEnabled;
    public bool WindEnabled
    {
        get => windEnabled;
        set => windEnabled = value;
    }

    private Vector3 windDirection;
    private float windForce;
    private Vector2 lastInputDir;
    private bool dirReleased;

    private bool moveX = false;
    private bool moveY = false;

    private Vector3 fixedVelocity;
    private float normalAngle;
    private bool updateVelocity;
    private float globalGravity = -9.81f;
    public float GlobalGravity
    {
        get => globalGravity;
    }

    private float walkSpeed;
    public float WalkSpeed
    {
        get => walkSpeed;
        set => walkSpeed = value;
    }
    private float runSpeed;
    public float RunSpeed
    {
        get => runSpeed;
        set => runSpeed = value;
    }

    private float walkOnAcc;
    private float walkOffAcc;
    private float runOnAcc;
    private float runOffAcc;

    private UnitMovementWallSlide ws;
    private UnitMovementJump uMJ;
    private UnitMovementDash uMD;

    private List<string> materials = new List<string>
    {
        "stone",
        "gravel",
        "sand",
        "dirt",
        "wood",
        "metal",
        "grass",
        "ice",
        "water",
    };

    void Start()
    {

        if (uMJ == null) uMJ = GetComponent<UnitMovementJump>();
        if (uMD == null) uMD = GetComponent<UnitMovementDash>();

        if (ws == null) ws = GetComponent<UnitMovementWallSlide>();
       
        SetSurfaceSpeedDefaults();
    }

    void Update()
    {
        if (!enableMovement) return;

        uMain.uState.CurrentGroundedState = uMain.uCollisions.IsGrounded();
        SetActualGroundState();

        Landing();  
        //air and ground Movement
        normalAngle = uMain.uCollisions.NormalAngle;

        if (uMain.uState.CurrentState == UNITSTATE.SLEEP) return;
        if (uMain.uState.CurrentState == UNITSTATE.DEATH) return;
        if (uMain.uState.CurrentState == UNITSTATE.CLIMBUP) return;
         if (uMain.uState.CurrentState == UNITSTATE.GRABSTAIRS) return;
       
        if (updateVelocity)
        {
            uMain.rb.velocity = fixedVelocity;
            updateVelocity = false;
        }
       

        if (Mathf.RoundToInt(inputDirection.x) != 0) SetDirection((DIRECTION)Mathf.RoundToInt(inputDirection.x));
        moveX = Mathf.Abs(inputDirection.x) > .25f;
        
        if (uMain.uState.CurrentGroundedState || uMain.uState.Flying)
        {
            RunAnimationGrounded();
        }
        else
        {
            RunAnimationAirborne();
        }
    }

    void FixedUpdate()
    {
        if (!enableMovement) return;
        if (uMain.uState.CurrentState == UNITSTATE.GRABSTAIRS) return;
        if (uMain.uState.CurrentState == UNITSTATE.CLIMBUP) return;

        SetActualGroundState();
        CalculateSpeed();
    }

    public void SetVelocity(Vector2 velocity)
    {
        fixedVelocity = velocity;
        updateVelocity = true;
    }
    //movement on the ground
    private void CalculateSpeed()
    {
        if (uMain.uState.StatesWithoutSpeedCalculation.Contains(uMain.uState.CurrentState))
        {
            return;
        }
        if (uMain.uState.CurrentState == UNITSTATE.ATTACK && uMain.uState.CurrentAttackType == ATTACKTYPE.ATTACKDASH)
        {
            return;
        }
        if (uMain.uState.StatesWithNoMove.Contains(uMain.uState.CurrentState))
        {
            moveX = false;
        } 
        
        //get peviuos velocity
        float movementSpeedX = uMain.rb.velocity.x;
        float movementSpeedY;
        float groundAngleX = normalAngle;
        if (Mathf.Abs(groundAngleX) < 0.01f) groundAngleX = 90;
        //set max speed and accleleration to run speed or walk speed depending on the current state
        float defSpeed = uMain.uState.CurrentRunState ? runSpeed : walkSpeed;
        float defOnAcc = uMain.uState.CurrentRunState ? runOnAcc : walkOnAcc;
        if (uMain.uState.CurrentState == UNITSTATE.ATTACK)
        {
            defSpeed = uMain.uMovementAttack.LastAttack.ForwardForce.x;
        }
        float velocity = uMain.rb.velocity.magnitude;
        float defOffAcc = velocity > walkSpeed + 0.1f ? runOffAcc : walkOffAcc;
        if (!uMain.uState.CurrentGroundedState && !uMain.uState.Flying)
        {
            defOnAcc = AirAcceleration;
            defOffAcc = AirAcceleration;
        }
        int wind = windEnabled ? 1 : 0;
        //starting move on X with acceleration
        if (moveX)
        {
            //calculating max movement speed
            float maxSpeed = defSpeed;
           
            //add wind modifier
            maxSpeed += wind * windForce * (windDirection.x * inputDirection.x);
            //add ground normal multiplier
            maxSpeed *= Mathf.Cos(Mathf.Deg2Rad * (groundAngleX - 90));
            //calculating movement speed
            movementSpeedX += defSpeed * inputDirection.x * defOnAcc * accelerationMultiplier;
            movementSpeedX = Mathf.Clamp(movementSpeedX, -maxSpeed, maxSpeed);
        }

        //ending move on X with deceleration
        if ((!moveX && !uMain.uMovementAttack.AttackIsActive) || uMain.uState.CurrentState == UNITSTATE.HITTED || uMain.uState.CurrentState == UNITSTATE.STUNNED)
        {
            //calculating speed if direction changed or offed
            movementSpeedX *= 1 - defOffAcc * accelerationMultiplier;
            if (Mathf.Sign(uMain.rb.velocity.x) > 0)
            {
                movementSpeedX = Mathf.Clamp(movementSpeedX, wind * windForce * windDirection.x, runSpeed + wind * windForce * windDirection.x);
            }
            else
            {
                movementSpeedX = Mathf.Clamp(movementSpeedX, -runSpeed - wind * windForce * windDirection.x, wind * windForce * windDirection.x);
            }
            if (Mathf.Abs(movementSpeedX) < 0.2f)
            {
                movementSpeedX = .0f;
            }
        }

        //calculating Y speed for slope movement
        movementSpeedY = Mathf.Abs(movementSpeedX) / Mathf.Tan(Mathf.Deg2Rad * groundAngleX * 1.0f);

        if (!uMain.uState.CurrentGroundedState && !uMain.uState.Flying)
        {
            movementSpeedY = uMain.rb.velocity.y + globalGravity * gravityScale * accelerationMultiplier;
            if (uMain.uState.CurrentState == UNITSTATE.WALLSLIDE && ws != null)
            {
                movementSpeedY *= ws.WallSlideMultiplier;
            }
            if (uMain.uState.CurrentState == UNITSTATE.ATTACK && uMain.uState.AirAttacks.Contains(uMain.uState.CurrentAttackType))
            {
                movementSpeedY *= 0.25f;
            } 

            if (uMain.uState.CurrentState == UNITSTATE.JUMP /*|| uMain.uState.CurrentState == UNITSTATE.SECONDJUMP*/)
            {
                movementSpeedY = uMain.rb.velocity.y;
            }
        } 
        
        if ((uMain.uState.CurrentState == UNITSTATE.HITTED || uMain.uState.CurrentState == UNITSTATE.STUNNED) && !uMain.uState.CurrentGroundedState && !uMain.uState.Flying)
        {
            movementSpeedY = uMain.rb.velocity.y + globalGravity * gravityScale * accelerationMultiplier;
        }
        
        if (uMain.uState.CurrentState == UNITSTATE.DASH)
        {
            movementSpeedX = uMain.rb.velocity.x;
            movementSpeedY = Mathf.Abs(movementSpeedX) / Mathf.Tan(Mathf.Deg2Rad * groundAngleX * 1.0f);
            if (!uMain.uState.CurrentGroundedState)
            {
                movementSpeedY *= 0.25f;
            }
        }

        if (movementSpeedY < maxFallSpeed)
        {
            movementSpeedY = maxFallSpeed;
        }
        
        
        if (fallSpeed > movementSpeedY)
        {
            fallSpeed = movementSpeedY;
        }

        if (uMain.uState.Flying)
        {
            movementSpeedY = uMain.rb.velocity.y + globalGravity * gravityScale * accelerationMultiplier;
            movementSpeedX += defSpeed * inputDirection.x * defOnAcc * accelerationMultiplier;
            movementSpeedX = Mathf.Clamp(movementSpeedX, -defSpeed, defSpeed);
        }

        uMain.rb.velocity = new Vector2(movementSpeedX, movementSpeedY);
    }
    private void RunAnimationGrounded()
    { 

        if (!uMain.uState.StatesReadyToSlide.Contains(uMain.uState.CurrentState))
        {
            return;
        }

        if (!moveX)
        {
            if (windEnabled)
            {
                uMain.uState.CurrentRunState = false;
                uMain.uState.CurrentState = UNITSTATE.IDLE;
                uMain.uAnimator.SetAnimatorTrigger("Idle");
            }
            else
            {
                if (uMain.rb.velocity.sqrMagnitude > 0.3f && !uMain.uState.Flying)
                {
                    uMain.uState.CurrentState = UNITSTATE.SLIDE;
                    uMain.uAnimator.SetAnimatorTrigger("Slide");
                }
                else
                {
                    uMain.uState.CurrentRunState = false;
                    uMain.uState.CurrentState = UNITSTATE.IDLE;
                    uMain.uAnimator.SetAnimatorTrigger("Idle");
                }
            }
        }
        else
        {
            if (uMain.uCollisions.WallInFrontX())
            {
                moveX = false;
            }
            uMain.uState.CurrentState = UNITSTATE.WALK;
            uMain.uAnimator.SetAnimatorTrigger("Move");
        }

        uMain.uAnimator.SetAnimatorFloat("isRunning", System.Convert.ToSingle(uMain.uState.CurrentRunState));
    }

    //movement in the air
    private void RunAnimationAirborne()
    {
        uMain.uAnimator.SetAnimatorFloat("isFalling", Mathf.Sign(uMain.rb.velocity.y));
        if (!uMain.uState.StatesReadyToFall.Contains(uMain.uState.CurrentState))
        {
            return;
        }
        uMain.uState.CurrentState = UNITSTATE.FALL;
        uMain.uAnimator.SetAnimatorTrigger("Fall");
    }

    public void SetWindForce(Wind g)
    {
        windDirection = g.Direction;
        windForce = g.CurrentSpeed;
    }

    public void SetSurfaceSpeedLimits(SurfaceFriction g)
    {
        walkSpeed = g.walkSpeed;
        runSpeed = g.runSpeed;

        walkOnAcc = g.walkOnAcc;
        walkOffAcc = g.walkOffAcc;
        runOnAcc = g.runOnAcc;
        runOffAcc = g.runOffAcc;
    }

    public void SetSurfaceSpeedDefaults()
    {
        walkSpeed = defaultWalkSpeed;
        runSpeed = defaultRunSpeed;

        walkOnAcc = defaultWalkOnAcc;
        walkOffAcc = defaultWalkOffAcc;
        runOnAcc = defaultRunOnAcc;
        runOffAcc = defaultRunOffAcc;
    }

    //player input - horizontal movement 
    public void OnMoveHorizontal(InputValue input)
    {
        inputDirection = new Vector2(Mathf.RoundToInt(input.Get<float>()), inputDirection.y);
        //check input
        moveX = Mathf.Abs(inputDirection.x) > .1f;
        if (DetectDoubleTapX())
        {
            StartRun();
        }
    }

    public void OnRun(InputValue input)
    {
        if (input.Get<float>() > 0.1f)
        {
            StartRun();
        }
    }

    public void StartRun()
    {
        if (!runEnabled) return;
        if (!uMain.uState.CurrentGroundedState) return;
        if (uMain.uState.CurrentRunState) return;
        if (!uMain.uState.StatesReadyToRun.Contains(uMain.uState.CurrentState)) return;
        if (uMain.uStamina.CurrentStamina < staminaRunCost * 0.1f)
        {
            uMain.uStamina.LowStamina = true;
            return;
        }
        if (uMain.uStamina.CurrentStamina < staminaRunCost)
        {
            uMain.uStamina.LowStamina = true;
        }
        
        uMain.uStamina.StaminaDecrease(staminaRunCost);
        uMain.uState.CurrentRunState = true;
    }
    
    public void OnMoveHorizontal(float input)
    {
        inputDirection = new Vector2(input, inputDirection.y);
        moveX = Mathf.Abs(inputDirection.x) > .1f;
    }
    //player input - vertical movement 
    public void OnMoveVertical(InputValue input)
    {
        inputDirection = new Vector2(inputDirection.x, Mathf.RoundToInt(input.Get<float>()));

        if (DetectDoubleTapY())
        {
        }
    }
    //npc input - horizontal movement 
    public void OnMoveVertical(float input)
    {
        inputDirection = new Vector2(inputDirection.x, input);
        moveY = Mathf.Abs(inputDirection.y) > .1f;
    }

    //checking double press input in horizontal direction
    private bool DetectDoubleTapX()
    {
        int dir = Mathf.RoundToInt(inputDirection.x);
        if (dir == 0)
        {
            if (!dirReleased)
            {
                lastInputDirTime.x = Time.time;
            }
            dirReleased = true;
            return false;
        }
        bool doubleTapDetected = (Time.time - lastInputDirTime.x < doubleTapWindow) && (lastInputDir.x == dir) && dirReleased;
        lastInputDir = new Vector2(dir, lastInputDir.y);
        dirReleased = false;
        return doubleTapDetected;
    }
    //checking double press input in vertical direction
    private bool DetectDoubleTapY()
    {
        int dir = Mathf.RoundToInt(inputDirection.y);
        if (dir == 0) return false;
        bool doubleTapDetected = ((Time.time - lastInputDirTime.y < doubleTapWindow) && (lastInputDir.y == dir));
        lastInputDir = new Vector2(lastInputDir.x, dir);
        lastInputDirTime.y = Time.time;
        return doubleTapDetected;
    }

    public void ResetState()
    {
        if (uMain.uState.CurrentState == UNITSTATE.DEATH)
        {
            return;
        }
           
        gameObject.layer = uMain.uState.DefaultLayer;

        if (uMain.uState.CurrentGroundedState || uMain.uState.Flying)
        {
            uMain.uState.CurrentState = UNITSTATE.IDLE;
            uMain.uAnimator.SetAnimatorTrigger("Idle");
        }
        else
        {
            uMain.uState.CurrentState = UNITSTATE.FALL;
            uMain.uAnimator.SetAnimatorTrigger("Fall");
        }
        uMain.uState.CurrentAttackType = ATTACKTYPE.DEFAULT;
        UpdateDirection();
    }

    public void UpdateDirection()
    {
        moveX = Mathf.Abs(inputDirection.x) > .1f;
        if (moveX) SetDirection((DIRECTION)Mathf.RoundToInt(inputDirection.x));
    }

    //set current direction and run revert animation if needed
    public void SetDirection(DIRECTION dir)
    {
        if (uMain.uState.StatesNoRotate.Contains(uMain.uState.CurrentState) || (uMain.uState.CurrentState == UNITSTATE.ATTACK && uMain.uState.CurrentAttackType != ATTACKTYPE.ATTACKCHARGING)) return;
        currentDirection = dir;
        if (previousDirection != currentDirection)
        {      
            if (uMain.uState.StatesReadyToRotate.Contains(uMain.uState.CurrentState))
            {
                RotateCharacter();
            }
        }
    }

    //flip character 180deg to currennt direction
    public void RotateCharacter()
    {
        //int flip = currentDirection == DIRECTION.RIGHT ? 1 : -1;
        //transform.localScale = new Vector3 (flip, 1, 1);
        
        float flip = currentDirection == DIRECTION.RIGHT ? 0f : 180f;
        transform.rotation = Quaternion.Euler(Vector3.up * flip);
        previousDirection = currentDirection;
    }

    public void ReverseDirection()
    {
        currentDirection = (DIRECTION)((int)currentDirection * (-1));
        RotateCharacter();
    }
    //checked when we need to run a land animation
    private void Landing()
    {
        if (uMain.uState.Flying) return;
        if (!uMain.uState.PreviousGroundedState && uMain.uState.CurrentGroundedState)
        {
            uMain.uMovementAttack.AirAttackDone = false;
            if (uMJ != null)
            {
                uMJ.SecondJumpDone = false;
            }
            if (uMD != null)
            {
                uMD.AirDashDone = false;
            }

            if (uMain.uState.StatesWithOwnLand.Contains(uMain.uState.CurrentState))
            {
                return;
            }

            if (uMain.uState.StatesReadyToLand.Contains(uMain.uState.CurrentState) && fallSpeed < maxFallSpeed * 0.25f)
            {           
                uMain.uState.CurrentState = UNITSTATE.LAND;
                uMain.uAnimator.SetAnimatorTrigger("Land");            
                uMain.rb.velocity = new Vector2(uMain.rb.velocity.x * 0.75f, 0);
            }
            else
            {
                AE_OffGroundAnimaion();
            }

            fallSpeed = 0f;
        }
    }
    private void SetActualGroundState()
    {
        if ((uMain.uState.CurrentState == UNITSTATE.HITTED || uMain.uState.CurrentState == UNITSTATE.STUNNED) && uMain.rb.velocity.y > 0.001f) uMain.uState.CurrentGroundedState = false;
        if (uMain.uState.CurrentState == UNITSTATE.ATTACK && uMain.rb.velocity.y > 0.01f) uMain.uState.CurrentGroundedState = false;
        if (uMain.uState.CurrentState == UNITSTATE.JUMP) uMain.uState.CurrentGroundedState = false;
        if (uMain.uState.CurrentState == UNITSTATE.JUMPDOWN) uMain.uState.CurrentGroundedState = false;
        if (uMain.rb.velocity.y > 0f && uMain.uState.CurrentState == UNITSTATE.FALL && uMain.uState.OnPlatform)
        {
            uMain.uState.OnPlatform = false;
            uMain.uState.CurrentGroundedState = false;
        }
    }


    public void AE_OffGroundAnimaion()
    {
        ResetState();
        RotateCharacter();
    }

    void AE_Footstep()
    {
        if (stepSFX == null)
        {
            return;
        }
        if (!uMain.uSFX.IsVisible())
        {
            return;
        }
        EventInstance sfx = uMain.uSFX.CreateSFX(stepSFX);
        uMain.uSFX.SetParameter(sfx, "terrainType", materials.FindIndex(x => x.StartsWith(uMain.uState.SurfaceMaterial)));
        uMain.uSFX.SetParameter(sfx, "isRunning", uMain.uState.CurrentRunState? 1f : 0f);
        uMain.uSFX.PlaySFX(sfx);
    }

    void AE_RunStop()
    {
        float dustXOffset = uMain.bc.bounds.size.x;
        float dustYOffset = -(uMain.bc.bounds.size.y/2 + uMain.bc.offset.y/2);
        uMain.SpawnVFX(m_RunStopDust, 0, false, dustXOffset, dustYOffset);
    }

    void AE_Landing()
    {
        float dustYOffset = -(uMain.bc.bounds.size.y / 2 - uMain.bc.offset.y / 2);
        uMain.SpawnVFX(m_LandingDust, 0, false, 0.0f, dustYOffset);


        if (landSFX == null)
        {
            return;
        }
        if (!uMain.uSFX.IsVisible())
        {
            return;
        }
        EventInstance sfx = uMain.uSFX.CreateSFX(landSFX);
        uMain.uSFX.SetParameter(sfx, "terrainType", materials.FindIndex(x => x.StartsWith(uMain.uState.SurfaceMaterial)));
        uMain.uSFX.PlaySFX(sfx);
    }
}

public enum DIRECTION
{
    RIGHT = 1,
    LEFT = -1
};
