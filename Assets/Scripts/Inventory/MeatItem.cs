using UnityEngine;
[CreateAssetMenu(menuName = "InventoryObject/Meat")]

[System.Serializable]
public class MeatItem : InventoryItem
{
	[Header("Meat type")]
	[SerializeField] private int meatIndex;
	public int MeatIndex
	{
		get => meatIndex;
	}
}
