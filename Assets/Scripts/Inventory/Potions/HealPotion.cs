using UnityEngine;
[CreateAssetMenu(menuName = "InventoryObject/HealPotion")]

[System.Serializable]
public class HealPotion : PotionItem
{
	[Header("Potion Settings")]
	[SerializeField] private float healthRestorePortion;
	public float HealthRestorePortion
	{
		get => healthRestorePortion;
	}
}
