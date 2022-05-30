using UnityEngine;
[System.Serializable]
public class AttackObject : ScriptableObject
{
	[SerializeField] private string attackName = "";
	public string AttackName => attackName;
	
	[SerializeField] private ATTACKTYPE actionType;
	public ATTACKTYPE ActionName => actionType;

	[SerializeField] private string animationTrigger = "";

    public string AnimationTrigger => animationTrigger;

	[SerializeField] private float staminaCost;
	public float StaminaCost => staminaCost;

	[Header("Timing Settings")]
	[SerializeField] private float cooldownTime = .1f;
	public float CooldownTime => cooldownTime;
	private float lastCurrentAttackTime = 0f;
	public float LastCurrentAttackTime
    {
		get => lastCurrentAttackTime;
		set => lastCurrentAttackTime = value;
    }
	[SerializeField] private float comboResetTimeMin = .0f;
	public float ComboResetTimeMin => comboResetTimeMin;
	[SerializeField] private float comboResetTimeMax = 1f;
	public float ComboResetTimeMax => comboResetTimeMax;


	[Header("Impulse Settings")]
	[SerializeField] private Vector2 forwardForce;
	public Vector2 ForwardForce
	{
		get => forwardForce;
		set => forwardForce = value;
	}

	[Header("SFX")]
	[SerializeField] private string attackSFX;
	public string AttackSFX
	{
		get => attackSFX;
	}
}

[System.Serializable]
public class AttackObjectsList
{
	public string comboName = "";
	public AttackObject[] ComboSequence = null; //a list of attacks
}