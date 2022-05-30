using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFader : MonoBehaviour
{
	[Header("Settings")]

	[SerializeField] private PASSDIRECTION passDir;
	
	[SerializeField] private GameObject fadeOff;
	[SerializeField] private GameObject fadeOn;
	
	[SerializeField] [Range(0f, 1f)] private float offSpriteAlpha = 1f;
	[SerializeField] [Range(0f, 1f)] private float onSpriteAlpha = 1f;

	[SerializeField] private float speed = 5;
	[SerializeField] private bool ignoreAlpha = true;

	private float alpha;
	private float alphaTarget;

	[HideInInspector]
	public GameObject inflictor;

	public void Fade()
    {
		if (fadeOff)
		{
			if (ignoreAlpha)
			{
				foreach (SpriteRenderer sr in fadeOff.GetComponentsInChildren<SpriteRenderer>()) sr.enabled = false;
			}
			else
			{
				alphaTarget = offSpriteAlpha;
				alpha = fadeOff.GetComponentInChildren<SpriteRenderer>().color.a;
				StartCoroutine(SetAlpha(fadeOff));
			}
		}
		if (fadeOn)
		{
			if (ignoreAlpha)
			{
				foreach (SpriteRenderer sr in fadeOn.GetComponentsInChildren<SpriteRenderer>()) sr.enabled = true;
			}
			else
			{
				alphaTarget = onSpriteAlpha;
				alpha = fadeOn.GetComponentInChildren<SpriteRenderer>().color.a;
				StartCoroutine(SetAlpha(fadeOn));
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

	IEnumerator SetAlpha(GameObject fade)
	{
		while (Mathf.Abs(alpha - alphaTarget) > .01f)
		{
			alpha = Mathf.Lerp(alpha, alphaTarget, Time.deltaTime * speed);
			foreach (SpriteRenderer sr in fade.GetComponentsInChildren<SpriteRenderer>())
			{
				sr.color = new Color(1f, 1f, 1f, alpha);
			}
			yield return new WaitForFixedUpdate();
		}
	}
}
