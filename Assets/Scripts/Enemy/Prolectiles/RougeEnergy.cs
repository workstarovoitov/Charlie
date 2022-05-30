using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

class RougeEnergy : MonoBehaviour
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
		if (transform.parent.transform.rotation.y > .1f)
        {
			speed *= -1f;
		}
		if (name.Contains("Left"))
        {
			speed *= -1f;
        }
		rb.velocity = new Vector2(speed, 0f);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (!((CollisionLayer.value & (1 << col.transform.gameObject.layer)) > 0))
		{
			return;
		}
		if (col.gameObject.name.Contains("Rouge"))
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
	}

	void AE_Destroy()
	{
		Destroy(transform.parent.gameObject);
	}
}
