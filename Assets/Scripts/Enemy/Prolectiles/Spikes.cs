using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

class Spikes : MonoBehaviour
{	
	[SerializeField] private float invictibleTime = 1f;                                //a list of all hittable objects

	[SerializeField] private HitObject attack;
	private int layer;

	void OnTriggerStay2D(Collider2D collision)
	{
		attack.inflictor = gameObject;
		IDamagable<HitObject> damagableObject = collision.GetComponent(typeof(IDamagable<HitObject>)) as IDamagable<HitObject>;
		if (damagableObject != null)
		{
			FMODUnity.RuntimeManager.PlayOneShot(attack.HitSFX, transform.position);

			damagableObject.Hit(attack);
			//CamShake camShake = Camera.main.GetComponent<CamShake>();
			//if (camShake != null)
			//	camShake.Shake(1f);

			//CamSlowMotionDelay camSM = Camera.main.GetComponent<CamSlowMotionDelay>();
			//if (camShake != null)
			//	camSM.StartSlowMotionDelay(attack.slowMotionDuration, attack.slowMotionScale);
			layer = collision.gameObject.layer;
			Physics2D.IgnoreLayerCollision(transform.gameObject.layer, layer, true);
			Invoke(nameof(EnableCollision), invictibleTime);
		}
	}

    private void EnableCollision()
    {
		Physics2D.IgnoreLayerCollision(transform.gameObject.layer, layer, false);
	}
}
