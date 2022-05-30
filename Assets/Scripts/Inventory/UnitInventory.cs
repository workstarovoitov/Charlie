using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "InventoryObject/Inventory")]

[System.Serializable]
public class UnitInventory : ScriptableObject
{
	[SerializeField] private List<InventoryPage> pages;
	public List<InventoryPage> Pages
	{
		get => pages;
		set 
		{ 
			pages.Clear();
			foreach  (InventoryPage item in value) pages.Add(item);
		} 
	}

}

[System.Serializable]
public class InventoryPage 
{
	[SerializeField] private string inventoryPageName;
	public string InventoryPageName
	{
		get => inventoryPageName;
	}
	
	[SerializeField] private Sprite itemFrame;
	public Sprite ItemFrame
	{
		get => itemFrame;
	}

	[SerializeField] private int slotsInInventory;
	public int SlotsInInventory
	{
		get => slotsInInventory;
	}

	[SerializeField] private INVENTORYPAGESTYPE itemsThatFitsToPage;
	public INVENTORYPAGESTYPE ItemsThatFitsToPage
	{
		get => itemsThatFitsToPage;
	}

	[SerializeField] private List<ItemsStack> stack;
	public List<ItemsStack> Stack
	{
		get => stack;
	}
}



public enum INVENTORYPAGESTYPE
{
	DEFAULT,
	POTION,
	WEAPON,
	MAGIC,
};