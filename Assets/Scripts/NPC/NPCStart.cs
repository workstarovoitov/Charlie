using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class NPCStart : MonoBehaviour {
		

    void Start()
	{
		gameObject.GetComponent<Animator>().speed = Random.Range(0.9f, 1.1f);

	}

}
