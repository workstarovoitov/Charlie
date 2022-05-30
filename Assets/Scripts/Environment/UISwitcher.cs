using System.Collections;
using UnityEngine;
using TMPro;
public class UISwitcher : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private LayerMask CollisionLayer;

	[SerializeField] [Range(0f, 1f)] private float offSpriteAlpha = 0f;
	[SerializeField] private float speed = 0.5f;

	private float alpha;
	private float alphaTarget;
	private SpriteRenderer[] sr;
	private TextMeshPro[] tm;

    private void OnEnable()
    {
		alpha = offSpriteAlpha;
		sr = GetComponentsInChildren<SpriteRenderer>();
		tm = GetComponentsInChildren<TextMeshPro>();
		foreach (SpriteRenderer sprite in sr)
        {
			sprite.color = new Color(1f, 1f, 1f, alpha);
        }
		foreach (TextMeshPro text in tm)
        {
			text.color = new Color(1f, 1f, 1f, alpha);
        }
	}
    private void OnDisable()
    {
		StopAllCoroutines();
    }
    void OnTriggerExit2D(Collider2D col)
	{
		if (gameObject.activeInHierarchy && (CollisionLayer.value & (1 << col.transform.gameObject.layer)) > 0)
		{
			alphaTarget = offSpriteAlpha;
			StartCoroutine(SetAlpha());
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (gameObject.activeInHierarchy && (CollisionLayer.value & (1 << col.transform.gameObject.layer)) > 0)
		{
			alphaTarget = 1.0f;
			StartCoroutine(SetAlpha());
		}
	}

	public void SetSprite(Sprite newSprite)
    {
		sr[0].sprite = newSprite;
    }

	IEnumerator SetAlpha()
	{
		while ( gameObject.activeSelf && Mathf.Abs(alpha - alphaTarget) > .01f)
		{
			alpha = Mathf.Lerp(alpha, alphaTarget, Time.deltaTime * speed);
			alpha = Mathf.Clamp(alpha, 0f, 1.0f);
			foreach (SpriteRenderer sprite in sr)
			{
				sprite.color = new Color(1f, 1f, 1f, alpha);
			}
			foreach (TextMeshPro text in tm)
			{
				text.color = new Color(1f, 1f, 1f, alpha);
			}
			yield return new WaitForFixedUpdate();
		}
	}

}