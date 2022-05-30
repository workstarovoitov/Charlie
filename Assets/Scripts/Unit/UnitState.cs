using UnityEngine;
using System.Collections.Generic;

public class UnitState : MonoBehaviour{

	[SerializeField] private bool flying = false;
	public bool Flying
	{
		get => flying;
		set => flying = value;
	}

	[SerializeField] private UNITSTATE currentState = UNITSTATE.IDLE;
	public UNITSTATE CurrentState
	{
		get => currentState;
		set
		{
			if (previousState != UNITSTATE.DEATH)
            {
				previousState = currentState;
				currentState = value;
            }
		}
	}

	private UNITSTATE previousState = UNITSTATE.IDLE;
	public UNITSTATE PreviousState => previousState;

	[SerializeField] private ATTACKTYPE currentAttackType;
	public ATTACKTYPE CurrentAttackType
	{
		get => currentAttackType;
		set => currentAttackType = value;
	}


	private bool currentGroundedState = false;
	public bool CurrentGroundedState
	{
		get => currentGroundedState;
		set
		{
			previousGroundedState = currentGroundedState;
			currentGroundedState = value;
		}
	}

	private bool previousGroundedState = false;
	public bool PreviousGroundedState => previousGroundedState;

	[SerializeField] private bool currentRunState = false;
	public bool CurrentRunState
	{
		get => currentRunState;
		set => currentRunState = value;
	}
	
	private string surfaceMaterial = null;
	public string SurfaceMaterial
	{
		get => surfaceMaterial;
		set => surfaceMaterial = value;
	}
	
	private int defaultLayer;
	public int DefaultLayer
	{
		get => defaultLayer;
		set => defaultLayer = value;
	}
	
	private bool onPlatform = false;
	public bool OnPlatform
	{
		get => onPlatform;
		set => onPlatform = value;
	}
	
	public bool IsStateChanged()
    {
		return currentState != previousState ? true : false;
	}
	public void UpdatePreviousState()
    {
		previousState = currentState;
	}
	
	//a list of states where movement can take place
	private List<UNITSTATE> statesReadyToSlide = new List<UNITSTATE> 
	{
		UNITSTATE.IDLE,
		UNITSTATE.WALK,
		UNITSTATE.SLIDE,
	};
	public List<UNITSTATE> StatesReadyToSlide
    {
		get => statesReadyToSlide;
    }


	private List<UNITSTATE> statesReadyToFall = new List<UNITSTATE> 
	{
		UNITSTATE.IDLE,
		UNITSTATE.WALK,
		UNITSTATE.SLIDE,
        UNITSTATE.FALL,
		UNITSTATE.DEFEND,
	};
	public List<UNITSTATE> StatesReadyToFall
	{
		get => statesReadyToFall;
	}


	private List<UNITSTATE> statesReadyToLand = new List<UNITSTATE> 
	{
		UNITSTATE.IDLE,
		UNITSTATE.WALK,
		UNITSTATE.SLIDE,
		UNITSTATE.JUMP,
		UNITSTATE.WALLJUMP,
		UNITSTATE.SECONDJUMP,
		UNITSTATE.FALL,
        UNITSTATE.DEFEND,
	};
	public List<UNITSTATE> StatesReadyToLand
	{
		get => statesReadyToLand;
	}


	private List<UNITSTATE> statesReadyToRotate = new List<UNITSTATE> 
	{
		UNITSTATE.IDLE,
		UNITSTATE.WALK,
		UNITSTATE.SLIDE,
		UNITSTATE.JUMP,
        UNITSTATE.ATTACK,
        UNITSTATE.SECONDJUMP,
		UNITSTATE.FALL,
		UNITSTATE.DEFEND,
		UNITSTATE.HIDE,
	};
	public List<UNITSTATE> StatesReadyToRotate
	{
		get => statesReadyToRotate;
	}


	private List<UNITSTATE> statesNoRotate = new List<UNITSTATE> {
		UNITSTATE.WALLJUMP,
		UNITSTATE.WALLSLIDE,
		UNITSTATE.GRABLEDGE,
		UNITSTATE.GRABSTAIRS,
		UNITSTATE.CLIMBUP,
	};
	public List<UNITSTATE> StatesNoRotate
	{
		get => statesNoRotate;
	}


	private List<UNITSTATE> statesWithoutSpeedCalculation = new List<UNITSTATE> {
		UNITSTATE.WALLJUMP,
		UNITSTATE.GRABLEDGE,
		UNITSTATE.GRABSTAIRS,
		UNITSTATE.CLIMBUP,
        UNITSTATE.SLEEP,
		UNITSTATE.HIDE,
	};
	public List<UNITSTATE> StatesWithoutSpeedCalculation
	{
		get => statesWithoutSpeedCalculation;
	}
	private List<UNITSTATE> statesReadyToRun = new List<UNITSTATE> {
		UNITSTATE.IDLE,
		UNITSTATE.WALK,
		UNITSTATE.SLIDE,
	};
	public List<UNITSTATE> StatesReadyToRun
	{
		get => statesReadyToRun;
	}


	private List<UNITSTATE> statesWithNoMove = new List<UNITSTATE> {
		UNITSTATE.HITTED,
		UNITSTATE.STUNNED,
		UNITSTATE.PARRY,
		UNITSTATE.DEFEND,
		UNITSTATE.DEATH,
	};
	public List<UNITSTATE> StatesWithNoMove
	{
		get => statesWithNoMove;
	}


	private List<UNITSTATE> statesWithOwnLand = new List<UNITSTATE> {
		UNITSTATE.HITTED,
		UNITSTATE.STUNNED,
		UNITSTATE.ATTACK,
		UNITSTATE.CLIMBUP,
		UNITSTATE.HIDE,
		UNITSTATE.DEATH,

	};
	public List<UNITSTATE> StatesWithOwnLand
	{
		get => statesWithOwnLand;
	}


	private List<UNITSTATE> statesReadyToAttack = new List<UNITSTATE>
	{
		UNITSTATE.IDLE,
		UNITSTATE.WALK,
		UNITSTATE.SLIDE,
		UNITSTATE.ATTACK,
		UNITSTATE.DEFEND
	};
	public List<UNITSTATE> StatesReadyToAttack
	{
		get => statesReadyToAttack;
	}


	private List<UNITSTATE> statesReadyToAirAttack = new List<UNITSTATE>
	{
		UNITSTATE.FALL,
		UNITSTATE.SECONDJUMP,
	};
	public List<UNITSTATE> StatesReadyToAirAttack
	{
		get => statesReadyToAirAttack;
	}


	private List<UNITSTATE> statesReadyToHit = new List<UNITSTATE> 
	{
			UNITSTATE.IDLE,
			UNITSTATE.WALK,
			UNITSTATE.SLIDE,
			UNITSTATE.JUMP,
			UNITSTATE.WALLJUMP,
			UNITSTATE.SECONDJUMP,
			UNITSTATE.FALL,
			UNITSTATE.WALLSLIDE,
			UNITSTATE.GRABLEDGE,
			UNITSTATE.CLIMBUP,
			UNITSTATE.LAND,
			UNITSTATE.ATTACK,
			UNITSTATE.PARRY,
			UNITSTATE.DEFEND,
			UNITSTATE.HITTED,
			UNITSTATE.STUNNED,
			UNITSTATE.SLEEP,
		};
	public List<UNITSTATE> StatesReadyToHit
	{
		get => statesReadyToHit;
	}

	private List<UNITSTATE> statesReadyToDefend = new List<UNITSTATE>
	{
		UNITSTATE.IDLE,
		UNITSTATE.WALK,
		UNITSTATE.SLIDE,
		//UNITSTATE.DEFEND,
	};

	public List<UNITSTATE> StatesReadyToDefend
	{
		get => statesReadyToDefend;
	}

	private List<UNITSTATE> statesReadyToDash = new List<UNITSTATE>
	{
		UNITSTATE.IDLE,
		UNITSTATE.WALK,
		UNITSTATE.SLIDE,
		UNITSTATE.DEFEND,
	};

	public List<UNITSTATE> StatesReadyToDash
	{
		get => statesReadyToDash;
	}	
	private List<UNITSTATE> statesReadyToAirDash = new List<UNITSTATE>
	{
		UNITSTATE.FALL,
		UNITSTATE.JUMP,
		UNITSTATE.SECONDJUMP,
	};

	public List<UNITSTATE> StatesReadyToAirDash
	{
		get => statesReadyToAirDash;
	}

	private List<ATTACKTYPE> airAttacks = new List<ATTACKTYPE>
	{
		ATTACKTYPE.ATTACKAIR,
		ATTACKTYPE.SHOOTAIR,
		ATTACKTYPE.THROWAIR,
	};
	public List<ATTACKTYPE> AirAttacks
	{
		get => airAttacks;
	}


}

public enum UNITSTATE {
	IDLE,
	WALK,
	SLIDE,
	DASH,

	JUMP,
	JUMPDOWN,
	WALLJUMP,
	SECONDJUMP,
	FALL,
	LAND,

	WALLSLIDE,
	GRABSTAIRS,
	GRABLEDGE,
	CLIMBSTAIRS,
	CLIMBUP,

	ATTACK,
	PARRY,
	DEFEND,
	HITTED,
	STUNNED,

	PICKUPITEM,

	DEATH,	
	SLEEP,
	HIDE,
};

public enum ATTACKTYPE
{
	DEFAULT,
	ATTACKAIR,
	ATTACKRUN,
	ATTACKCHARGING,
	ATTACKCHARGED,
	ATTACKDASH,
	THROW,
	THROWAIR,
	SHOOT,
	SHOOTAIR,
	ATTACKSPECIAL,
};