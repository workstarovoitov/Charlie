using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(UnitMain))]

public class UnitSpawnSequence : MonoBehaviour, ISpawnUnit
{
	[SerializeField] private Transform spawnPoint;
	[SerializeField] private float speed = 5f;

	private UnitMain uMain;
	private EnemyActions eActions;
	private float alpha = 0;
	private float alphaTarget = 1;
	private SpriteRenderer sr;

	public void OnSpawn()
	{
		sr = GetComponent<SpriteRenderer>();
		uMain = GetComponent<UnitMain>();
		eActions = GetComponent<EnemyActions>();
		alpha = 0;
		alphaTarget = 1;
		sr.color = new Color(1f, 1f, 1f, alpha);
		transform.position = spawnPoint.position;
		uMain.rb.velocity = Vector2.zero;

		eActions.LookAtTarget(eActions.targetAttack.transform);
		eActions.EnableAI = true;
		uMain.uMovement.EnableMovement = true;
		StartCoroutine(SetAlpha());
	}

	public void OnFade()
    {
		sr = GetComponent<SpriteRenderer>();
		alpha = sr.color.a;
		if (alpha < 0.1f)
        {
			alphaTarget = 1f;

		}
        else
        {
			alphaTarget = 0f;

		}
		StartCoroutine(SetAlpha());

	}

	IEnumerator SetAlpha()
	{
		while (Mathf.Abs(alpha - alphaTarget) > .01f)
		{
			alpha = Mathf.Lerp(alpha, alphaTarget, Time.deltaTime * speed);
			sr.color = new Color(1f, 1f, 1f, alpha);
			yield return new WaitForFixedUpdate();
		}
	}
}
