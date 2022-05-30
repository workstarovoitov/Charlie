using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFader : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private LayerMask CollisionLayer;
	[SerializeField] private float waitTime = 1f;

	private GameObject go;
	void OnTriggerEnter2D(Collider2D col)
	{
		if ((CollisionLayer.value & (1 << col.transform.gameObject.layer)) > 0)
		{
			UnitSpawnSequence uisw = col.GetComponentInChildren<UnitSpawnSequence>();
			if (uisw != null)
            {
				go = col.transform.gameObject;
				Invoke(nameof(Fade), waitTime);
			}
		}
	}
	
	private bool IsTriggered()
    {
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position , 1f, 1 << go.layer);

		foreach (Collider2D col in hitColliders)
		{
			if (col.gameObject == go) return true;
		}
		return false;
    }


	private void Fade()
    {
		if (IsTriggered())	go.GetComponentInChildren<UnitSpawnSequence>().OnFade();
	}
	
}