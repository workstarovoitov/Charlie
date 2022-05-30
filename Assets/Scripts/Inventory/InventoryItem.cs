using UnityEngine;

[CreateAssetMenu(menuName = "InventoryObject/Item")]
[System.Serializable]

public class InventoryItem : ScriptableObject
{
	[SerializeField] private string itemName;
	public string ItemName
	{
		get => itemName;
	}

	[TextArea(5, 10)]
	[SerializeField] private string itemDescription;
	public string ItemDescription
	{
		get => itemDescription;
	}

	[SerializeField] private Sprite itemSprite;
	public Sprite ItemSprite
	{
		get => itemSprite;
	}
	
	[SerializeField] private int stackCount;
	public int StackCount
	{
		get => stackCount;
		set => stackCount = value;
	}

	[SerializeField] private INVENTORYPAGESTYPE page;
	public INVENTORYPAGESTYPE Page
	{
		get => page;
	}
	
	[Header("SFX")]
	[SerializeField] private string spawnSFX;
	public string SpawnSFX
	{
		get => spawnSFX;
	}
	[SerializeField] private string grabSFX;
	public string GrabSFX
	{
		get => grabSFX;
	}
	[SerializeField] private string fullSFX;
	public string FullSFX
	{
		get => fullSFX;
	}

	[SerializeField] private string useSFX;
	public string UseSFX
	{
		get => useSFX;
	}

	[SerializeField] private string throwSFX;
	public string ThrowSFX
	{
		get => throwSFX;
	}
	[SerializeField] private string blockedSFX;
	public string BlockedSFX
	{
		get => blockedSFX;
	}

}

[System.Serializable]
public class ItemsStack
{
	[SerializeField] private InventoryItem items = null; //a list of attacks
	public InventoryItem Items
	{
		get => items;
		set => items = value;
	}

	[SerializeField] private int quantity = 1;
	public int Quantity
	{
		get => quantity;
		set => quantity = value;
	}

	[SerializeField] private int price;
	public int Price
	{
		get => price;
	}

	public ItemsStack(ItemsStack item)
	{
		items = item.Items;
		quantity = item.Quantity;
		price = item.Price;
	}
	public ItemsStack()
	{
	}
}