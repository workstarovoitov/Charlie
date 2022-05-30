using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class UnitMovementAttack : MonoBehaviour
{
	[SerializeField] UnitMain uMain;
	
	[Header("Combo")]
	public AttackObjectsList[] ComboList = null;            //list of combos
	
	private AttackObject lastAttack;                        //data from the last attack
	public AttackObject LastAttack
	{
		get => lastAttack;
		set => lastAttack = value;
	}

	[Header("Settings")]
	[SerializeField] private bool comboContinueOnHit = true;                  //continue a combo when the previous attack was a hit
	public bool ComboContinueOnHit
    {
		get => comboContinueOnHit;
	}
	
	[SerializeField] private bool oneAttackPerJump = true;                    //only one attack per jump
	public bool OneAttackPerJump
	{
		get => oneAttackPerJump;
	}

	private float lastAttackTime = .0f;						//time of the last attack
	public float LastAttackTime
	{
		get => lastAttackTime;
		set => lastAttackTime = value;
	}

	private ATTACKTYPE lastAttackInput;                     //last attack input

	private bool targetWasHit = false;						//true if the last hit has hit a target
	public bool TargetWasHit
    {
		get => targetWasHit;
		set => targetWasHit = value;
	}

	private DIRECTION lastAttackDirection;

	private int attackNum = 0;                                  //the current attack combo number
	public int AttackNum
	{
		get => attackNum;
		set => attackNum = value;
	}
	
	private bool chargedAttackReady = false;
	public bool ChargedAttackReady 
	{
		get => chargedAttackReady;
	}

	private bool attackIsActive = false;
	public bool AttackIsActive
	{
		get => attackIsActive;
	}

	private bool attackIsPaused = false;
	public bool AttackIsPaused
	{
		get => attackIsPaused;
	}

	private bool airAttackDone = false;
	public bool AirAttackDone
	{
		get => airAttackDone;
		set => airAttackDone = value;
	}

	private bool bttnMesh = false;
	public bool BttnMesh
    {
		get => bttnMesh;
    }
	private static int maxComboLength = 20;
	private bool[] comboListCanContinue = new bool[maxComboLength];

    private void Start()
    {
		//refresh cooldown
		for (int i = 0; i < ComboList.Length; i++)
		{
			for (int j = 0; j < ComboList[i].ComboSequence.Length; j++)
			{
				ComboList[i].ComboSequence[j].LastCurrentAttackTime = 0;
			}
		}
	}
	public void OnPunchCharge(InputValue input)
	{
		if (input.Get<float>() > 0.1f)
		{
			if (!uMain.uState.CurrentGroundedState)
			{
				OnPunch();
				return;
			}

			uMain.uMovement.SetVelocity(Vector3.zero);
			tryAttack(ATTACKTYPE.ATTACKCHARGING);
		}
		else
		{
			if (uMain.uState.CurrentState == UNITSTATE.ATTACK && uMain.uState.CurrentAttackType == ATTACKTYPE.ATTACKCHARGING)
			{
				if (chargedAttackReady)
				{
					CancelInvoke(nameof(ResetCharge));
					tryAttack(ATTACKTYPE.ATTACKCHARGED);
					return;
				}
				ResetAttack();
				uMain.uMovement.ResetState();
			}
		}
	}

	public void ResetCharge()
    {
		if (chargedAttackReady && uMain.uState.CurrentState == UNITSTATE.ATTACK)
        {
			AE_OffAttackAnimation();
        }
    }

	//player input
	public void OnPunch()
	{
		tryAttack(ATTACKTYPE.DEFAULT);
	}

	public void OnThrow()
	{
		tryAttack(ATTACKTYPE.THROW);
	}

	public void OnShoot()
	{
		tryAttack(ATTACKTYPE.SHOOT);
	}

	private void tryAttack(ATTACKTYPE inputAction)
	{
		//air attack
		if (uMain.uState.StatesReadyToAirAttack.Contains(uMain.uState.CurrentState))
		{
			if (oneAttackPerJump && airAttackDone) return;
			ATTACKTYPE airAttack = ATTACKTYPE.ATTACKAIR;
			switch (inputAction)
            {
				case ATTACKTYPE.DEFAULT: 
					airAttack = ATTACKTYPE.ATTACKAIR;
					break;
				case ATTACKTYPE.SHOOT: 
					airAttack = ATTACKTYPE.SHOOTAIR;
					break;
				case ATTACKTYPE.THROW: 
					airAttack = ATTACKTYPE.THROWAIR;
					break;
            }
			if (comboListContainAttack(airAttack))
			{
				tryComboAttack(airAttack);
				return;
			}
		}
		//running attack
		if (uMain.uState.CurrentGroundedState == true && uMain.uState.CurrentRunState == true && inputAction == ATTACKTYPE.DEFAULT)
		{
			if (comboListContainAttack(ATTACKTYPE.ATTACKRUN))
			{
				comboContinueSuperCheck(ATTACKTYPE.ATTACKRUN);
				for (int i = 0; i < ComboList.Length; i++)
				{
					if (comboListCanContinue[i])
					{
						tryComboAttack(ATTACKTYPE.ATTACKRUN);
						uMain.uState.CurrentRunState = false;
						return;
					}
				}
			}
			uMain.uState.CurrentRunState = false;
		}

		if (!uMain.uState.StatesReadyToAttack.Contains(uMain.uState.CurrentState)) return;

		//default attack
		//new attack
		if (uMain.uState.CurrentState != UNITSTATE.ATTACK || chargedAttackReady)
		{
			bttnMesh = false;
			tryComboAttack(inputAction);
			return;
		}
		//if meshing
		else
		{
			bttnMesh = true;
			lastAttackInput = inputAction;
			return;
		}
	}

	bool comboListContainAttack(ATTACKTYPE inputAction)
	{
		for (int i = 0; i < ComboList.Length; i++)
		{
			//check on last attack in combo
			for (int j = 0; j < ComboList[i].ComboSequence.Length; j++)
			{
				//check input 
				if (inputAction == ComboList[i].ComboSequence[j].ActionName)
				{
					return true;
				}
			}
		}
		return false;
	}

	void comboContinueSuperCheck(ATTACKTYPE inputAction, bool firstTimeCheck = true)
	{
		//should we continue only on hit
		if (comboContinueOnHit && !targetWasHit && !chargedAttackReady)
		{
			attackNum = 0;
		}
		//reset tree position if first attack
		if (attackNum == 0)
		{
			for (int i = 0; i < ComboList.Length; i++)
			{
				comboListCanContinue[i] = true;
			}
		}
		//check if we get a correct input 
		for (int i = 0; i < ComboList.Length; i++)
		{
			//skip wrong nodes
			if (!comboListCanContinue[i]) continue;
			//check on last attack in combo
			if (attackNum < ComboList[i].ComboSequence.Length)
			{
				//check input 
				if (inputAction != ComboList[i].ComboSequence[attackNum].ActionName)
				{
					comboListCanContinue[i] = false;
				}
			}
			else
			{
				comboListCanContinue[i] = false;
			}
		}
		//check if attack fit in timing 		
		if (attackNum > 0)
		{
			for (int i = 0; i < ComboList.Length; i++)
			{
				//skip wrong nodes
				if (!comboListCanContinue[i]) continue;
				//check timing 
				if (!(Time.time < (lastAttackTime + ComboList[i].ComboSequence[attackNum - 1].ComboResetTimeMax) &&
						(Time.time > (lastAttackTime + ComboList[i].ComboSequence[attackNum - 1].ComboResetTimeMin))))
					comboListCanContinue[i] = false;
			}
		}
		//check if attack fit there is enough stamina 		
		for (int i = 0; i < ComboList.Length; i++)
		{
			//skip wrong nodes
			if (!comboListCanContinue[i]) continue;
			//check stamina 
			if (ComboList[i].ComboSequence[attackNum].StaminaCost > uMain.uStamina.CurrentStamina )
				comboListCanContinue[i] = false;
		}
		//check cooldown
		for (int i = 0; i < ComboList.Length; i++)
		{
			//skip wrong nodes
			if (!comboListCanContinue[i]) continue;
			//check cooldown
			if (Time.time < ComboList[i].ComboSequence[attackNum].LastCurrentAttackTime + ComboList[i].ComboSequence[attackNum].CooldownTime)
				comboListCanContinue[i] = false;
		}
	
		//check if it rest any node
		for (int i = 0; i < ComboList.Length; i++)
		{
			if (comboListCanContinue[i])
			{
				return;
			}
		}

		//combo failed, need to run first attack
		if (firstTimeCheck)
		{
			attackNum = 0;
			comboContinueSuperCheck(inputAction, false);
			return;
		}
		Debug.LogWarning("No " + inputAction + " attack in " + name);
	}

	public void tryComboAttack(ATTACKTYPE inputAction, int comboNum = -1)
	{
		comboContinueSuperCheck(inputAction);
		if (uMain.uState.AirAttacks.Contains(inputAction)) 
		{ 
			airAttackDone = true; 
		}

		if (inputAction == ATTACKTYPE.ATTACKCHARGING)
		{
			for (int i = 0; i < ComboList.Length; i++)
			{
				if (comboListCanContinue[i])
				{
					Invoke(nameof(ResetCharge), ComboList[i].ComboSequence[attackNum].ComboResetTimeMax);
				}
			}
		}

		// forced attack 
		if (comboNum != -1 && comboListCanContinue[comboNum])
		{
			doSingleAttack(ComboList[comboNum].ComboSequence[attackNum], inputAction);
			return;
		}

		// default attack 
		for (int i = 0; i < ComboList.Length; i++)
		{
			if (comboListCanContinue[i])
			{
				doSingleAttack(ComboList[i].ComboSequence[attackNum], inputAction);
				return;
			}
		}
	}

	private void doSingleAttack(AttackObject damageObject, ATTACKTYPE state)
	{
		//save attack data
		lastAttack = damageObject;
		lastAttackTime = Time.time;
		lastAttack.LastCurrentAttackTime = Time.time;
		lastAttackDirection = uMain.uMovement.CurrentDirection;

		//spend stamina
		uMain.uStamina.StaminaDecrease(lastAttack.StaminaCost);

		//animation run		
		uMain.uState.CurrentState = UNITSTATE.ATTACK;
		uMain.uState.CurrentAttackType = state;
		
		attackNum++;
		if (damageObject.ActionName == ATTACKTYPE.SHOOT || damageObject.ActionName == ATTACKTYPE.SHOOTAIR)
        {
			ShootObject so = (ShootObject)damageObject;
			uMain.uAnimator.SetAnimatorFloat("ShootAngle", (float)so.AngleType);
		}
		uMain.uAnimator.SetAnimatorTrigger(damageObject.AnimationTrigger);
	}

	//returns true if the player is facing a gameobject
	public bool isFacingTarget(GameObject g)
	{
		return ((g.transform.position.x < transform.position.x && uMain.uMovement.CurrentDirection == DIRECTION.LEFT) || (g.transform.position.x > transform.position.x && uMain.uMovement.CurrentDirection == DIRECTION.RIGHT));
	}

	public void ResetAttack()
	{
		attackNum = 0;

		StopAllCoroutines();

		attackIsPaused = false;
		attackIsActive = false;
		chargedAttackReady = false;
		uMain.uMovement.ResetState();
	}

	IEnumerator AttackProgressRoutine()
	{
		//a list of enemies that we have hit
		List<GameObject> enemieshit = new List<GameObject>();

		targetWasHit = false;
		HitObject ho = (HitObject)lastAttack;
		ho.inflictor = gameObject;

		//check for hit
		while (attackIsActive && !attackIsPaused)
		{
			//draw a hitbox in front of the character to see which objects it collides with
			Vector2 boxPosition = transform.position + (Vector3.up * ho.ColOffset.y) + Vector3.right * ((int)lastAttackDirection * ho.ColOffset.x);
			Vector2 boxSize = new Vector2(ho.ColRange.x, ho.ColRange.y);
			Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxPosition, boxSize, 0f, ho.HitLayerMask);
			//hit an enemy only once by adding it to a list
			foreach (Collider2D col in hitColliders)
			{

				if (col.gameObject == gameObject) continue;
				if (!enemieshit.Contains(col.gameObject))
				{
					enemieshit.Add(col.gameObject);
					//hit a damagable object
					IDamagable<HitObject> damagableObject = col.GetComponent(typeof(IDamagable<HitObject>)) as IDamagable<HitObject>;
					if (damagableObject != null)
					{
						damagableObject.Hit(ho);
						targetWasHit = true;

						CameraShake.Instance.Shake(ho);

						//CamSlowMotionDelay camSM = Camera.main.GetComponent<CamSlowMotionDelay>();
						//if (camShake != null)
						//	camSM.StartSlowMotionDelay(ho.slowMotionDuration, ho.slowMotionScale);
					}
				}
			}

			yield return null;
		}
	}

	void AE_LowStamina()
	{
		uMain.uStamina.LowStamina = true;
	}

	void AE_ChargedAttackReady()
	{
		chargedAttackReady = true;
	}
	
	void AE_StartAttackCoroutine()
	{
		lastAttackDirection = uMain.uMovement.CurrentDirection;
		attackIsActive = true;
		attackIsPaused = false;
		StartCoroutine(AttackProgressRoutine());
	}

	void AE_PauseAttackCoroutine()
	{
		attackIsPaused = true;
	}
	void AE_StopAttackCoroutine()
	{
		attackIsPaused = false;
		attackIsActive = false;
	}

	void AE_OffAttackAnimation()
	{
		uMain.uMovement.ResetState();
		chargedAttackReady = false;

		if (bttnMesh)
		{
			tryAttack(lastAttackInput);
		}
	}
	void AE_AddAttackSpeed(float multiplier = 1f)
	{
		uMain.rb.velocity = (Vector2.right * (int)lastAttackDirection * lastAttack.ForwardForce.x  + Vector2.up * (uMain.rb.velocity.y + lastAttack.ForwardForce.y));
	}
	void AE_PlayAttackSFX()
	{
		uMain.uSFX.PlayOneShotSFX(lastAttack.AttackSFX);
	}

	//Display hit box in Unity Editor (Debug)
#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (!Application.isPlaying) return;


		if (attackIsActive)
		{
			HitObject ho = (HitObject)lastAttack;
			Gizmos.color = Color.red;
			Vector3 boxPosition = transform.position + (Vector3.up * ho.ColOffset.y) + Vector3.right * ((int)lastAttackDirection * ho.ColOffset.x);
			Vector3 boxSize = new Vector3(ho.ColRange.x, ho.ColRange.y, 0.5f);
			Gizmos.DrawWireCube(boxPosition, boxSize);

		}
	}
#endif

}

