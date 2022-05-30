using UnityEngine;

public class SurfaceFriction : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private LayerMask CollisionLayer;
	public float walkSpeed = 5f;
	public float runSpeed = 8f;

	public float walkOnAcc = 3f;
	public float walkOffAcc = 8f;
	public float runOnAcc = 5f;
	public float runOffAcc = 3f;

	[HideInInspector]
	public GameObject inflictor;

	public SurfaceFriction(GameObject _inflictor)
	{
		inflictor = _inflictor;
	}

	void OnTriggerEnter(Collider col)
	{
		if ((CollisionLayer.value & (1 << col.transform.gameObject.layer)) > 0)
		{
			col.GetComponent<UnitMain>().uMovement.SetSurfaceSpeedLimits(this);
		}
	}
	void OnTriggerExit(Collider col)
	{
		if ((CollisionLayer.value & (1 << col.transform.gameObject.layer)) > 0)
		{
			col.GetComponent<UnitMain>().uMovement.SetSurfaceSpeedDefaults();
		}
	}
}
