using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "InventoryObject/AllAvailableItems")]

public class AllAvailableItems : ScriptableObject
{
	[SerializeField] private List<GameObject> itemPrefab;
	public List<GameObject> ItemPrefab
	{
		get => itemPrefab;
	}

}
