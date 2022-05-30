using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using System;

public class OneWayTrigger: MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private LayerMask CollisionLayer;

	[SerializeField] private PASSDIRECTION passDir;
	public UnityEvent OnStart;
	public UnityEvent OnTriggerPass;

	public void Start()
	{
		OnStart.Invoke();
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if ((CollisionLayer.value & (1 << col.transform.gameObject.layer)) > 0)
		{
			if (IsRightDirection(col))
            {
				OnTriggerPass.Invoke();
			}
		}
	}

	public bool IsRightDirection(Collider2D col)
    {
		switch (passDir)
        {
			case PASSDIRECTION.LEFT_RIGHT:
				return col.transform.position.x > transform.position.x;
			case PASSDIRECTION.RIGHT_LEFT:
				return col.transform.position.x < transform.position.x; 
			case PASSDIRECTION.TOP_DOWN: 
				return col.transform.position.y < transform.position.y; 
			case PASSDIRECTION.DOWN_TOP: 
				return col.transform.position.y > transform.position.y; 
		}
        return false;
    }

	
}



public enum PASSDIRECTION
{
	LEFT_RIGHT = 1,
	RIGHT_LEFT = 2,

	TOP_DOWN = 3,
	DOWN_TOP = 4,

};