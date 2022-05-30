using UnityEngine;


public class Wind : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private LayerMask CollisionLayer;

	[SerializeField] private Vector3 direction;
	public Vector3 Direction
	{
		get => direction;
	}
	[SerializeField] private float maxSpeed = 5f;

	public float onDuration = 3f;
	public float offDuration = 3f;
	public float onAcc = 2f;
	public float offAcc = 5f;
	
	[SerializeField] private float currentSpeed = 0f;
	public float CurrentSpeed
	{
		get => currentSpeed;
	}
	private bool isOn = false;
	private float lastOnTime = 0;
	private float lastOffTime = 0;

	[HideInInspector]
	public GameObject inflictor;

	public Wind(GameObject _inflictor)
	{
		inflictor = _inflictor;
	}

    private void Update()
    {
		if (isOn)
        {
			if (Time.time - lastOnTime < onDuration)
			{
				currentSpeed += onAcc*Time.deltaTime;
				if (currentSpeed >=  maxSpeed)
                {
					currentSpeed = maxSpeed;
				}
            }
            else
            {
				isOn = false;
				lastOffTime = Time.time;
            }
        }
		if (!isOn)
		{
			if (Time.time - lastOffTime < offDuration)
			{
				currentSpeed -= offAcc * Time.deltaTime;
				if (currentSpeed <= 0f)
				{
					currentSpeed = 0f;
				}
			}
			else
			{
				isOn = true;
				lastOnTime = Time.time;
			}
		}

	}


	void OnTriggerStay(Collider col)
	{
		if ((CollisionLayer.value & (1 << col.transform.gameObject.layer)) > 0)
		{
			col.GetComponent<UnitMain>().uMovement.SetWindForce(this);
			col.GetComponent<UnitMain>().uMovement.WindEnabled = true;
		}
	
	}

	void OnTriggerExit(Collider col)
	{
		if ((CollisionLayer.value & (1 << col.transform.gameObject.layer)) > 0)
		{
			col.GetComponent<UnitMain>().uMovement.WindEnabled = false;
		}
	}
}
