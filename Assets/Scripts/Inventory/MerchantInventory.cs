using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "InventoryObject/MerchantInventory")]

[System.Serializable]
 public class MerchantInventory : ScriptableObject
{
	[SerializeField] private string storeName;
	public string StoreName
	{
		get => storeName;
	}

	[TextArea(5, 10)]
	[SerializeField] private string storeDescription;
	public string StoreDescription
	{
		get => storeDescription;
	}
	
	[SerializeField] private int slotsInInventory;
	public int SlotsInInventory
	{
		get => slotsInInventory;
	}
	[SerializeField] private Sprite itemFrame;
	public Sprite ItemFrame
	{
		get => itemFrame;
	}
	[SerializeField] private List<ItemsStack> stack;
	public List<ItemsStack> Stack
	{
		get => stack;
	}
}