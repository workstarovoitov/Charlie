using UnityEngine;
[CreateAssetMenu(menuName = "AttackObject/Hit")]

[System.Serializable]
public class HitObject : AttackObject
{
	[Header("Hit Settings")]
	[SerializeField] private float damage;
	public float Damage
	{
		get => damage;
	}

	[SerializeField] private int hitWeight;
	public int HitWeight => hitWeight;

	[Header("Knockback Settings")]
	[SerializeField] private Vector2 knockBackForce;
	public Vector2 KnockBackForce
	{
		get => knockBackForce;
		set => knockBackForce = value;
	}

	[SerializeField] private float knockDownTime = 0f;
	public float KnockDownTime
	{
		get => knockDownTime;
	}

	[Header("Block Settings")]
	[SerializeField] private bool defenceOverride;
	public bool DefenceOverride
	{
		get => defenceOverride;
	}
	[SerializeField] private float parryCooldown = 0f;
	public float ParryCooldown
	{
		get => parryCooldown;
	}

	[Header("Camera Settings")]
	public float cameraShakeAmplitude;
	public float cameraShakeFrequency;
	public float cameraShakeDuration;
	public float slowMotionDuration = .0f;
	public float slowMotionScale = 1f;

	[Header("Hit Collider Settings")]
	[SerializeField] private Vector2 colRange;
	public Vector2 ColRange
	{
		get => colRange;
	}
	[SerializeField] private Vector2 colOffset;
	public Vector2 ColOffset
	{
		get => colOffset;
	}

	[SerializeField] private LayerMask hitLayerMask;            //layers with hittable objects
	public LayerMask HitLayerMask
	{
		get => hitLayerMask;
	}

	[Header("Hit SFX")]
	[SerializeField] private string hitSFX;
	public string HitSFX
    {
		get => hitSFX;
    }
	[SerializeField] private string hitBlockSFX;
	public string HitBlockSFX
    {
		get => hitBlockSFX;
    }
	
	[HideInInspector]
	public GameObject inflictor;
	public HitObject(GameObject _inflictor)
	{
		inflictor = _inflictor;
	}
}