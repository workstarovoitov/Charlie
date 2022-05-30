using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;


[RequireComponent(typeof(UnitSFX))]

class RougeShuriken : MonoBehaviour
{	
	private Rigidbody2D rb;
	
	[SerializeField] private LayerMask CollisionLayer;

	[SerializeField] private float speed = 6f;

	[SerializeField] private HitObject attack;
	
	private UnitSFX uSFX;

	// Use this for initialization
	void Start()
	{
		uSFX = GetComponent<UnitSFX>();
		uSFX.PlayOneShotSFX(attack.AttackSFX);
		if (!rb) rb = GetComponent<Rigidbody2D>();
		rb.gravityScale = 0;
		if (transform.rotation.y > .1f)
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
        else
        {
			FMODUnity.RuntimeManager.PlayOneShot(attack.HitBlockSFX, transform.position);
		}
		GetComponent<Animator>().SetTrigger("Destroy");
		GetComponent<Collider2D>().enabled = false;
		rb.velocity = new Vector2(rb.velocity.x * 0.25f, 0f);

	}

	private void AE_DestroyShuriken()
    {
		Destroy(gameObject);
    }
}
