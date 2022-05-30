using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

class ItemsSpawnerOnTrigger : MonoBehaviour
{	
	private Rigidbody2D rb;

	[SerializeField] private float itemsNum = 5f;

	[SerializeField] private GameObject item;
	[SerializeField] private string spawnSFX;
	private Vector2 spawnLocation;
	void OnTriggerEnter2D(Collider2D col)
	{		
		BoxCollider2D bc = GetComponent<BoxCollider2D>();
		spawnLocation.x = bc.bounds.size.x;
		spawnLocation.y = bc.bounds.size.y;
		GetComponent<Collider2D>().enabled = false;

		for (int i = 0; i < itemsNum; i++)
        {
			Invoke(nameof(SpawnItem), Random.Range(0.5f + 0.25f * i, 1f + i));
        }
	}

	public void SpawnItem()
    {		
		FMODUnity.RuntimeManager.PlayOneShot(spawnSFX, transform.position);
		Instantiate(item, new Vector3(transform.position.x + Random.Range(-spawnLocation.x / 3, spawnLocation.x / 3), transform.position.y + Random.Range(0, spawnLocation.y / 3), 0f), Quaternion.Euler(Vector3.zero));
	}
}
