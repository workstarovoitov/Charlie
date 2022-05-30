using UnityEngine;
[CreateAssetMenu(menuName = "InventoryObject/HealthPotion")]

[System.Serializable]
public class HealthPotion : PotionItem
{
	[Header("Potion Settings")]
	[SerializeField] private float healthRestoreSpeed;
	public float HealthRestoreSpeed
	{
		get => healthRestoreSpeed;
	}
	
	[SerializeField] private int healthRestoreNum;
	public int HealthRestoreNum
	{
		get => healthRestoreNum;
	}
	[SerializeField] private float healthRestorePortion;
	public float HealthRestorePortion
	{
		get => healthRestorePortion;
	}
}
