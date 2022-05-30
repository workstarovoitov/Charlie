using UnityEngine;
using System.Collections.Generic;

//public class MerchantInventory : ScriptableObject
//{
//	private string storeName;
//	public string StoreName
//	{
//		get => storeName;
//	}

//	private string storeDescription;
//	public string StoreDescription
//	{
//		get => storeDescription;
//	}
	
//	private int slotsInInventory;
//	public int SlotsInInventory
//	{
//		get => slotsInInventory;
//	}
//	private Sprite itemFrame;
//	public Sprite ItemFrame
//	{
//		get => itemFrame;
//	}
//	private List<ItemsStack> stack;
//	public List<ItemsStack> Stack
//	{
//		get => stack;
//	}

//	public MerchantInventory (MerchantInventoryScriptable mis)
//    {
//		storeName = mis.StoreName;
//		storeDescription = mis.StoreDescription;
//		slotsInInventory = mis.SlotsInInventory;
//		itemFrame = mis.ItemFrame;
//		stack = new List<ItemsStack>();
//		mis.Stack.ForEach((item) =>
//		{
//			stack.Add(new ItemsStack (item));
//		});
//	}
//}