using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

class Arrow : MonoBehaviour
{	
	private Rigidbody2D rb;

	[SerializeField] private LayerMask CollisionLayer;

	[SerializeField] private float speed = 3f;

	[SerializeField] private HitObject attack;


	// Use this for initialization
	void Start()
	{
		FMODUnity.RuntimeManager.PlayOneShot(attack.AttackSFX, transform.position);

		if (!rb) rb = GetComponent<Rigidbody2D>();
		rb.gravityScale = 0;

		rb.velocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (transform.eulerAngles.z)) * speed, Mathf.Sin(Mathf.Deg2Rad * (transform.eulerAngles.z)) * speed);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (!((CollisionLayer.value & (1 << col.transform.gameObject.layer)) > 0))
		{
			return;
		}

		attack.inflictor = gameObject;

		IDamagable<HitObject> damagableObject = col.GetComponent(typeof(IDamagable<HitObject>)) as IDamagable<HitObject>;
		if (damagableObject != null)
		{

			FMODUnity.RuntimeManager.PlayOneShot(attack.HitSFX, transform.position);

			damagableObject.Hit(attack);
        }
        else
        {
			FMODUnity.RuntimeManager.PlayOneShot(attack.HitBlockSFX, transform.position);
		}		

		Invoke(nameof(DestroyArrow), Random.Range(0.25f, 1f));
		GetComponent<Collider2D>().enabled = false;

		
		transform.parent = col.transform;
		rb.isKinematic = true;
		rb.velocity = Vector2.zero;



	}

	private void DestroyArrow()
    {
		Destroy(gameObject);
    }
}
