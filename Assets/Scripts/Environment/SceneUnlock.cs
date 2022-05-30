using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using System;

public class SceneUnlock : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private GameObject walls;

	public void UnlockScene()
	{
		GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
		CinemachineConfiner confiner = camera.GetComponentInChildren<CinemachineConfiner>();
		PolygonCollider2D mainConfiner = camera.GetComponentInChildren<PolygonCollider2D>();
		confiner.m_BoundingShape2D = mainConfiner;

		foreach (BoxCollider2D bc in walls.GetComponentsInChildren<BoxCollider2D>()) bc.enabled = false;
	}


}


