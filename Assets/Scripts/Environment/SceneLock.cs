using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SceneLock : MonoBehaviour
{
	[Header("Settings")]
	
	[SerializeField] private PolygonCollider2D Room;
	[SerializeField] private GameObject walls;

	public void LockScene()
    {
		GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
		CinemachineConfiner confiner = camera.GetComponentInChildren<CinemachineConfiner>();

		confiner.m_BoundingShape2D = Room;

		foreach (BoxCollider2D bc in walls.GetComponentsInChildren<BoxCollider2D>()) bc.enabled = true;
		GetComponent<Collider2D>().enabled = false;
    }
}


