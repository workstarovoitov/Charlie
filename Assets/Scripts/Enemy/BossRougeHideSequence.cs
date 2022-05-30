using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(UnitMain))]

public class BossRougeHideSequence : SpecialAttack, ISpawnUnit
{
	[SerializeField] UnitMain uMain;
	[SerializeField] EnemyActions eActions;

	[SerializeField] private LayerMask invictibleLayer;

	[SerializeField] private float hideMinTime = 0.2f;
	[SerializeField] private float hideMaxTime = 2f;
	[SerializeField] private float[] heightSpawn;

	[SerializeField] private float staminaCost;
	[SerializeField] private int[] comboIndexes;

	[SerializeField] private string hideSFX;
	[SerializeField] private string unhideSFX;

	[SerializeField] private Transform spawnPoint;

	private int attackIndex;

	private List<UNITSTATE> StatesReadyToHide = new List<UNITSTATE>
	{
		UNITSTATE.IDLE,
		UNITSTATE.WALK,
		UNITSTATE.SLIDE,
		UNITSTATE.DEFEND,
	};

	
	public override void OnSpecialAttack()
	{
		OnHide();
	}

	public void OnHide()
	{
		uMain.uSFX.PlayOneShotSFX(hideSFX);

		uMain.uState.CurrentState = UNITSTATE.HIDE;
		uMain.uAnimator.SetAnimatorTrigger("Hide");		
		attackIndex = Random.Range(0, comboIndexes.Length);
	}

	public void AE_EndHideAnimation()
	{
		gameObject.layer = (int)Mathf.Log(invictibleLayer.value, 2);

		Invoke(nameof(UnhideSequence), Random.Range(hideMinTime, hideMaxTime));
	}
	public void UnhideSequence()
	{
		transform.position = eActions.targetAttack.transform.position + Vector3.up * heightSpawn[attackIndex];
		uMain.rb.velocity = Vector2.zero;

		eActions.LookAtTarget(eActions.targetAttack.transform);
		
		uMain.uSFX.PlayOneShotSFX(unhideSFX);

		uMain.uAnimator.SetAnimatorTrigger("Unhide");
		//gameObject.layer = uMain.uState.DefaultLayer;
	}

	void AE_ResetUnhide()
	{
		eActions.LookAtTarget(eActions.targetAttack.transform);
		uMain.uMovementAttack.tryComboAttack(uMain.uMovementAttack.ComboList[comboIndexes[attackIndex]].ComboSequence[0].ActionName, comboIndexes[attackIndex]);
	}

	public void OnSpawn()
	{
		transform.position = spawnPoint.position;
		uMain.rb.velocity = Vector2.zero;

		eActions.LookAtTarget(eActions.targetAttack.transform);

		uMain.uAnimator.SetAnimatorTrigger("Unhide");
		while (!uMain.uState.CurrentGroundedState)
		{
			new WaitForFixedUpdate();
		}
		gameObject.layer = uMain.uState.DefaultLayer;
	}

}
