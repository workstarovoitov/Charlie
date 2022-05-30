using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class EnemyActions : MonoBehaviour {
		
	[SerializeField] UnitMain uMain;
	[Space(10)]
	[Header ("Linked components")]
	public GameObject targetAttack;           //current target
	[SerializeField] private GameObject[] waypoints;
	[SerializeField] private float waypointWaitTime;
	[SerializeField] private float anotherEnemyTouchWaitTime;
	private int currentWaypointIndex = 0;
	private float waypointSpottedTime = 0;
	private float anotherEnemyTouchTime = 0;

	[Header("Combos indexes")]

	[SerializeField] public int[] meleeAttackComboIndexes;
	[SerializeField] public int[] rangedAttackComboIndexes;
	private SpecialMove[] specialMoves;
	private SpecialAttack[] specialAttacks;

	[Header("Attack Data")]
	public float hitYRange = 0f; //the y range of all attacks
	public float hitAngleRange = 0f; //the y range of all attacks
	[Range(0f, 1f)] public float defendChance = 0; //the chance that an incoming attack is defended %
	[Range(0f, 1f)] public float specialAttackChance = 0; //the chance that an incoming attack is defended %
	public float defenceCooldown = 4; //the chance that an incoming attack is defended %
	private float lastDefendTime = 0; //the chance that an incoming attack is defended %
	public float specialAttackCooldown = 4; //the chance that an incoming attack is defended %
	private float lastSpecialAttackTime = 0; //the chance that an incoming attack is defended %
	private bool enforceMeleeAttack = false;

	public float attackInterval = 5f; //the time inbetween attacking
	
	[Header("Attack Distances")]
	public float meleeAttackDistance = 1f; //the distance from the target where the enemy is able to attack
	public float rangedAttackDistance = 5f; //the distance from the target where the enemy is able to attack
	public float closeRangeDistance = 3f; //the distance from the target at close range
	public float midRangeDistance = 5f; //the distance from the target at mid range
	public float farRangeDistance = 10f; //the distance from the target at mid range
	public float rangeMarging = 0.5f; //the amount of space that is allowed between the player and enemy before we reposition ourselves
	public float sightDistance = 5f; //the distance when we can see the target
	
	[Header("Stats")]
	[SerializeField] private RANGE range;
	[SerializeField] private ENEMYTACTIC currentEnemyTactic;
	public ENEMYTACTIC CurrentEnemyTactic
	{
		get => currentEnemyTactic;
		set
		{
			currentEnemyTactic = value;
		}
	}

	private bool inMeleeAttackRange = false;
	private bool inRangedAttackRange = false;

	[SerializeField] private bool enableAI;
	public bool EnableAI
	{
		get => enableAI;
		set => enableAI = value;
	}
	private int comboNum;
	private bool needToDefend = false;
	private bool chillDefenceChanceChecked = false;
	private bool obstacleTouched = false;
    //list of states where the enemy cannot move
    private List<UNITSTATE> NoMovementStates = new List<UNITSTATE> 
	{
		UNITSTATE.DEATH,
		UNITSTATE.ATTACK,
		UNITSTATE.DEFEND,
		UNITSTATE.HITTED,
		UNITSTATE.STUNNED,
	};

	private List<UNITSTATE> ActiveAIStates = new List<UNITSTATE> 
	{
		UNITSTATE.IDLE,
		UNITSTATE.SLEEP,
		UNITSTATE.WALK,
		UNITSTATE.SLIDE,
		UNITSTATE.FALL,
	};

    void Start()
	{
		//randomize values to avoid synchronous movement
		SetRandomValues();

		specialMoves = GetComponents<SpecialMove>();
		specialAttacks = GetComponents<SpecialAttack>();
		if (targetAttack == null)
        {
			targetAttack = GameObject.Find("Ghost");
        }
	}

	void Update()
	{		
		if (!enableAI) return;

		if (uMain.uState.CurrentState == UNITSTATE.DEATH)
        {
			uMain.uMovement.SetVelocity(Vector2.zero);
			enableAI = false;
			return;
		}

        if (uMain.uState.CurrentState == UNITSTATE.DEFEND)
        {
            LookAtTarget(targetAttack.transform);
			return;
        }

        if (ActiveAIStates.Contains(uMain.uState.CurrentState) && !IsInvoking(nameof(Attack)))
		{
			AI();
		}
	}

	void AI()
	{
		//check distance to target
		range = GetDistanceToTarget(targetAttack);
		
		//patrool if no target 
		if ((targetAttack == null || !IsTargetSpotted(targetAttack)) && waypoints.Length > 0 && uMain.uState.CurrentState != UNITSTATE.SLEEP)
        {
			OnPatrool();
			return;
        }

		//return if no target and no waypoints
		if (targetAttack == null || !IsTargetSpotted(targetAttack)) return;
        		
		//wake up
		if (uMain.uState.CurrentState == UNITSTATE.SLEEP)
		{
			uMain.uState.CurrentState = UNITSTATE.IDLE;
			uMain.uAnimator.SetAnimatorTrigger("Idle");
		}

		//if ready to attack
		if (Time.time - uMain.uMovementAttack.LastAttackTime > attackInterval)
		{
			currentEnemyTactic = ENEMYTACTIC.CHILL;
			//try special attack
			if (!enforceMeleeAttack)
            {
				if (TryToSpecialAttack())
				{
					return;
				}
            }
			
			//Stop waiting melee attack
			if (Time.time - uMain.uMovementAttack.LastAttackTime > 3 * attackInterval)
            {
				enforceMeleeAttack = false;
            }
			
			//attack immediately
			if (inMeleeAttackRange || (inRangedAttackRange && meleeAttackDistance < 0.1f))
            {
				enforceMeleeAttack = false;
				LookAtTarget(targetAttack.transform);
				Attack();
				return;
            }

			//if we have melee  and ranged attacks shoot or move to melee
			if (inRangedAttackRange && meleeAttackDistance > 0f && !enforceMeleeAttack)
            {
				float dist = (targetAttack.transform.position - transform.position).magnitude;
				float optDist = Mathf.Abs(rangedAttackDistance * 0.75f - dist);
				float chanceDist = Mathf.Abs(rangedAttackDistance * 0.75f - Random.Range(0f, rangedAttackDistance));
				enforceMeleeAttack = true;
				uMain.uMovementAttack.LastAttackTime = Time.time;
				currentEnemyTactic = ENEMYTACTIC.ENGAGE;

				//shoot
				if (optDist < chanceDist)
				{
					LookAtTarget(targetAttack.transform);
					Attack();
                }
				return;
            }

			//if time to hit but target out of range
			if (range == RANGE.CLOSERANGE || range == RANGE.MIDRANGE)
            {
				currentEnemyTactic = ENEMYTACTIC.ENGAGE;
            }
		}
		//else set tactic or defend
		else
		{
			switch (range)
			{
				//try to defence or move
				case RANGE.CLOSERANGE:
					
					if (currentEnemyTactic == ENEMYTACTIC.KEEPCLOSEDISTANCE)
					{
						currentEnemyTactic = ENEMYTACTIC.CHILL;
                    }
                    else
                    {
						TryToDefence();
                    }
					break;
				case RANGE.MIDRANGE:
					if (currentEnemyTactic == ENEMYTACTIC.KEEPMEDIUMDISTANCE)
						currentEnemyTactic = ENEMYTACTIC.CHILL;
					break;
				case RANGE.FARRANGE:
					if (currentEnemyTactic == ENEMYTACTIC.KEEPFARDISTANCE)
					{
						currentEnemyTactic = ENEMYTACTIC.CHILL;
                    }
                    else
                    {
						currentEnemyTactic = ENEMYTACTIC.KEEPMEDIUMDISTANCE;
                    }
					break;
			}
		}

		//walk depend on tactic

		switch (currentEnemyTactic)
		{

			case ENEMYTACTIC.ENGAGE:
				if (meleeAttackDistance > 0f)
				{
					WalkTo(targetAttack, meleeAttackDistance, rangeMarging);
				}
				else
				{
					WalkTo(targetAttack, rangedAttackDistance * 0.75f, rangeMarging);
				}
				break;

			case ENEMYTACTIC.KEEPCLOSEDISTANCE:
				WalkTo(targetAttack, closeRangeDistance, rangeMarging);
				break;

			case ENEMYTACTIC.KEEPMEDIUMDISTANCE:
				WalkTo(targetAttack, midRangeDistance, rangeMarging);
				break;

			case ENEMYTACTIC.KEEPFARDISTANCE:
				WalkTo(targetAttack, farRangeDistance, rangeMarging);
				break;

			case ENEMYTACTIC.CHILL:
				LookAtTarget(targetAttack.transform);
				StopWalk();
				break;
		}
		ObstaclesCheck();
	}
	
	public void OnPatrool()
    {
		
		if (Time.time - waypointSpottedTime > waypointWaitTime && currentEnemyTactic == ENEMYTACTIC.CHILL)
		{
			currentEnemyTactic = ENEMYTACTIC.PATROL;
			currentWaypointIndex++;

			if (currentWaypointIndex >= waypoints.Length)
			{
				currentWaypointIndex = 0;
			}
		}

		if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < rangeMarging * 2)
		{
			if (currentEnemyTactic == ENEMYTACTIC.PATROL)
			{
				currentEnemyTactic = ENEMYTACTIC.CHILL;

				waypointSpottedTime = Time.time;
			}
		}

		WalkTo(waypoints[currentWaypointIndex], rangeMarging * 2, rangeMarging);
		ObstaclesCheck();
	}
		
	//Attack 
	public void Attack() 
	{
		int attackNum;
		
		uMain.uMovement.SetVelocity(Vector2.zero);
		Move(Vector2.zero);
		uMain.uMovement.ResetState();
		LookAtTarget(targetAttack.transform);

		//check is it a first hit in combo
		if ( (!uMain.uMovementAttack.TargetWasHit && (uMain.uMovementAttack.ComboContinueOnHit || !uMain.uMovementAttack.ChargedAttackReady)) || Time.time - uMain.uMovementAttack.LastAttackTime > attackInterval)
        {
			attackNum = 0;
			if (inRangedAttackRange)
            {
				comboNum = rangedAttackComboIndexes[Random.Range(0, rangedAttackComboIndexes.Length)];
            }			
			if (inMeleeAttackRange)
            {
				comboNum = meleeAttackComboIndexes[Random.Range(0, meleeAttackComboIndexes.Length)];
            }
        }
        else
        {
			attackNum = uMain.uMovementAttack.AttackNum;
		}

		//try to attack
		if (attackNum < uMain.uMovementAttack.ComboList[comboNum].ComboSequence.Length)
		{
			//shoot preparements
			if (uMain.uMovementAttack.ComboList[comboNum].ComboSequence[attackNum].ActionName == ATTACKTYPE.SHOOT 
				|| uMain.uMovementAttack.ComboList[comboNum].ComboSequence[attackNum].ActionName == ATTACKTYPE.SHOOTAIR)
            {
				ShootObject so = (ShootObject)uMain.uMovementAttack.ComboList[comboNum].ComboSequence[attackNum];

				Vector2 dir = targetAttack.transform.position - transform.position;
				float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
				so.ShootDirection = (int)uMain.uMovement.CurrentDirection;

				if (Mathf.Abs(angle) <= 15 || Mathf.Abs(angle) >= 165)
				{
					so.AngleType = ANGLETYPE.MID;				
					so.ShootAngle = (dir - Vector2.right * so.ColOffsetMid.x * (int)uMain.uMovement.CurrentDirection - Vector2.up * so.ColOffsetMid.y).normalized;
				}
				if ((angle > 15 && angle < 60) || (angle < 165 && angle > 120))
				{
					so.AngleType = ANGLETYPE.HIGH;
					so.ShootAngle = (dir - Vector2.right * so.ColOffsetHigh.x * (int)uMain.uMovement.CurrentDirection - Vector2.up * so.ColOffsetHigh.y).normalized;
				}
				if ((-angle > 15 && -angle < 60) || (-angle < 165 && -angle > 120))
				{
					so.AngleType = ANGLETYPE.LOW;
					so.ShootAngle = (dir - Vector2.right * so.ColOffsetLow.x * (int)uMain.uMovement.CurrentDirection - Vector2.up * so.ColOffsetLow.y).normalized;
				}

			}
			
			//throw preparements
			if (uMain.uMovementAttack.ComboList[comboNum].ComboSequence[attackNum].ActionName == ATTACKTYPE.THROW 
				|| uMain.uMovementAttack.ComboList[comboNum].ComboSequence[attackNum].ActionName == ATTACKTYPE.THROW)
            {
				ThrowObject to = (ThrowObject)uMain.uMovementAttack.ComboList[comboNum].ComboSequence[attackNum];

				to.ThrowDirection = (int)uMain.uMovement.CurrentDirection;
			}
			
			//dash preparements
			if (uMain.uMovementAttack.ComboList[comboNum].ComboSequence[attackNum].ActionName == ATTACKTYPE.ATTACKDASH)
            {
				Vector2 dir = targetAttack.transform.position - transform.position;

				uMain.uMovementAttack.ComboList[comboNum].ComboSequence[attackNum].ForwardForce = (Mathf.Abs(dir.normalized.x ) * Vector2.right + dir.normalized.y * Vector2.up) * midRangeDistance*2;
			}

			uMain.rb.velocity = Vector2.zero;
			//try to attack
			uMain.uMovementAttack.tryComboAttack(uMain.uMovementAttack.ComboList[comboNum].ComboSequence[attackNum].ActionName, comboNum);

			//try next hit in combo
			if (attackNum < uMain.uMovementAttack.ComboList[comboNum].ComboSequence.Length - 1)
			{
				Invoke(nameof(Attack), uMain.uMovementAttack.ComboList[comboNum].ComboSequence[attackNum].ComboResetTimeMax * 0.9f);
			}
			return;
		}

		uMain.uMovementAttack.TargetWasHit = false;
		uMain.uMovementAttack.AttackNum = 0;
		return;
	}

	public bool TryToSpecialAttack()
    {
		if (Time.time - lastSpecialAttackTime > specialAttackCooldown)
		{
			lastSpecialAttackTime = Time.time;
			if (Random.Range(0f, 1f) < specialAttackChance)
			{
				//try special move
				if (specialAttacks.Length > 0)
				{	
					uMain.rb.velocity = Vector2.zero;
					LookAtTarget(targetAttack.transform);
					specialAttacks[Random.Range(0, specialAttacks.Length)].OnSpecialAttack();
					return true;
				}
			}
		}
		return false;
	}
	
	public void TryToDefence()
    {
		//check defence def chance after cooldown
		if (Time.time - lastDefendTime > defenceCooldown)
		{
			lastDefendTime = Time.time;
			chillDefenceChanceChecked = false;
			if (Random.Range(0f, 1f) < defendChance)
			{
				needToDefend = true;
			}
			else
			{
				currentEnemyTactic = ENEMYTACTIC.KEEPMEDIUMDISTANCE;
			}
		}

		//check defence chance if main character approached
		if (currentEnemyTactic == ENEMYTACTIC.CHILL && !chillDefenceChanceChecked)
		{
			lastDefendTime = Time.time;
			chillDefenceChanceChecked = true;
			if (Random.Range(0f, 1f) < defendChance)
			{
				needToDefend = true;
			}
		}

		//try special move
		if (needToDefend && specialMoves.Length > 0)
        {
			needToDefend = false;
			int moveIndex = Random.Range(0, specialMoves.Length);
			if (specialMoves[moveIndex].IsSpecialMoveFaced())
			{
				LookAtTarget(targetAttack.transform);
            }
			specialMoves[Random.Range(0, specialMoves.Length)].OnSpecialMove();
        }
    }

	private bool IsTargetSpotted(GameObject target)
    {
		if (target == null) return false;

		bool wallBetween = uMain.uCollisions.IsWallBetweenTarget(target);
		return (Vector2.Distance(target.transform.position, transform.position) < sightDistance && !wallBetween);
	}

	//update the current range
	private RANGE GetDistanceToTarget(GameObject target)
	{
		inMeleeAttackRange = false;
		inRangedAttackRange = false;

		if (target == null)
		{
			return RANGE.FARRANGE;
		}

		//get distance from the target
		Vector2 distance = target.transform.position - transform.position;
		bool wallBetween = uMain.uCollisions.IsWallBetweenTarget(target);
		if (wallBetween) 
		{
			return RANGE.FARRANGE;
		}

		if (Mathf.Abs(distance.x) <= meleeAttackDistance)
		{
			if (InVerticalMeleeAttackRange(target))
			{
				inMeleeAttackRange = true;
			}
		}

		if (Mathf.Abs(distance.x) <= rangedAttackDistance)
		{
			if (InVerticalRangedAttackRange(target))
			{
				inRangedAttackRange = true;
			}
		}

		if (InVerticalMeleeAttackRange(target) || InVerticalRangedAttackRange(target))
        {
			if (Mathf.Abs(distance.x) <= closeRangeDistance)
			{
				return RANGE.CLOSERANGE;
			}

			if (Mathf.Abs(distance.x) > closeRangeDistance && Mathf.Abs(distance.x) <= midRangeDistance)
			{
				return RANGE.MIDRANGE;
			}
        }

		return RANGE.FARRANGE;
	}
	
	private bool InVerticalMeleeAttackRange(GameObject target)
    {
		Vector2 distance = target.transform.position - transform.position;
		if (Mathf.Abs(distance.y) < hitYRange)
        {
			return true;
        }
		return false;
    }
	private bool InVerticalRangedAttackRange(GameObject target)
    {
		Vector2 distance = target.transform.position - transform.position;
		float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
		if (Mathf.Abs(angle) < hitAngleRange || 180f - Mathf.Abs(angle) < hitAngleRange)
        {
			return true;
        }
		return false;
    }
	
	public void StopWalk()
    {
		Move(Vector2.zero);
    }

	//walk to target
	public void WalkTo(GameObject destination, float proximityRange, float movementMargin)
	{
		
		Vector2 dirToTarget = destination.transform.position - transform.position;
		float dist = Vector2.Distance(destination.transform.position, transform.position);
		Vector2 moveDirection;

		

		if (dist >= proximityRange)
		{
			moveDirection = new Vector2(dirToTarget.x, dirToTarget.y);
			if (uMain.uState.CurrentGroundedState  || uMain.uState.Flying)
			{
				Move(moveDirection.normalized);
				return;
			}
		}
		//we are too close, move away
		if (dist <= proximityRange - movementMargin)
		{
			moveDirection = new Vector2(-dirToTarget.x, dirToTarget.y);
			if (uMain.uState.CurrentGroundedState || uMain.uState.Flying )
			{
				Move(moveDirection.normalized);
				return;
			}
		}

		Move(Vector3.zero);
	}

	public void Move(Vector2 vector)
	{
		

		if (NoMovementStates.Contains(uMain.uState.CurrentState)) 
		{
			//uMain.uMovement.SetVelocity(Vector2.zero);
			uMain.uMovement.OnMoveHorizontal(0f);
			uMain.uMovement.OnMoveVertical(0f);
		}
		else 
		{
			uMain.uMovement.OnMoveHorizontal(vector.x);
			uMain.uMovement.OnMoveVertical(vector.y);

			UnitMovementFlap umf = GetComponent<UnitMovementFlap>();
			if (umf != null)
			{
				umf.SetFlapForce(vector.y);
            }
        }
	}
	
	private void ObstaclesCheck()
    {
		//check other enemies in front
		if (!obstacleTouched)
        {
			GameObject enemyInTouch = IsAnotherEnemyTouch();
			if (enemyInTouch != null && !uMain.uState.Flying)
			{		
				anotherEnemyTouchTime = Time.time;
				obstacleTouched = true;
			}
			//check for cliff
			if ((uMain.uCollisions.CliffInFrontX() && !uMain.uState.Flying) || uMain.uCollisions.WallInFrontX())
			{
				anotherEnemyTouchTime = Time.time;
				obstacleTouched = true;
			}
        }

		if (Time.time - anotherEnemyTouchTime < anotherEnemyTouchWaitTime)
		{			
			StopWalk();
        }
        else
        {
			obstacleTouched = false;
        }
    }

	GameObject IsAnotherEnemyTouch()
	{
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(uMain.bc.bounds.center + Vector3.right * (int) uMain.uMovement.CurrentDirection * 2 * (uMain.bc.edgeRadius), uMain.bc.size.x / 2f + uMain.bc.edgeRadius * 2f, 1 << gameObject.layer);

		foreach (Collider2D col in hitColliders)
		{
			if (col.gameObject == gameObject) continue;
			return col.gameObject;
		}
		return null;
	}

	public void RunStunnedSequence()
    {
		CancelInvoke(nameof(Attack));
		currentEnemyTactic = ENEMYTACTIC.KEEPMEDIUMDISTANCE;
		SetRandomValues();
		//Invoke(nameof(SetEngageTactic), attackInterval);
	}
	
	public void RunHittedSequence()
    {
		CancelInvoke(nameof(Attack));
		//currentEnemyTactic = ENEMYTACTIC.KEEPMEDIUMDISTANCE;
		SetRandomValues();

		currentEnemyTactic = ENEMYTACTIC.CHILL;
		chillDefenceChanceChecked = false;
		
		TryToDefence();
		//Invoke(nameof(SetEngageTactic), attackInterval);
	}
	
	public void SetEngageTactic()
    {
		currentEnemyTactic = ENEMYTACTIC.ENGAGE;
	}

	//look at the current target
	public void LookAtTarget(Transform target)
	{
		if (target == null) return;
		
		int dir = target.transform.position.x >= transform.position.x ? 1 : -1;
		uMain.uMovement.SetDirection((DIRECTION)dir);
	}

	//randomize values
	public void SetRandomValues()
	{
		gameObject.GetComponent<Animator>().speed = Random.Range(0.9f, 1.1f);
		attackInterval *= Random.Range(0.75f, 1.25f);
		uMain.uMovement.WalkSpeed *= Random.Range(.8f, 1.2f);
		uMain.uMovement.RunSpeed *= Random.Range(.8f, 1.2f);
	}
}

public enum ENEMYTACTIC
{
	ENGAGE = 0,
	KEEPCLOSEDISTANCE = 1,
	KEEPMEDIUMDISTANCE = 2,
	KEEPFARDISTANCE = 3,
	PATROL = 4,
	CHILL = 5,
}

public enum RANGE
{
	CLOSERANGE,
	MIDRANGE,
	FARRANGE,
}