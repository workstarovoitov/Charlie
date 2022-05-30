using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DanceSwitch : MonoBehaviour {

	[SerializeField] private float danceSwitchTime = 10f;
	private UnitMain uMain;

    void Start()
	{
		uMain = GetComponent<UnitMain>();
		OnSwitch();
	}


	private void OnSwitch()
    {
		uMain.uAnimator.SetAnimatorFloat("IdleNum", Mathf.RoundToInt(Random.Range(0f, 3f)));
		Invoke(nameof(OnSwitch), danceSwitchTime);
	}
}
