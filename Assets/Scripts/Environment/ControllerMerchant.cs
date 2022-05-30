using UnityEngine;
using System;

public class ControllerMerchant : UsableObject, IUsable
{
    public static event Action<ControllerMerchant> OnTradeStart;

    [SerializeField] private MerchantInventory mInventory;
    
    public  MerchantInventory MInventory
    {
        get => mInventory;
        set => mInventory = value;
    }
    
    [SerializeField] private AllAvailableItems allItems;
    public AllAvailableItems AllItems
    {
        get => allItems;
        set => allItems = value;
    }


    public new void Use()
    {
        OnTradeStart?.Invoke(this);
    }

    public void RemoveItemFromInventory(int currentItem)
    {
        if (mInventory.Stack[currentItem].Quantity == 1)
        {
            mInventory.Stack.RemoveAt(currentItem);
            //menu.OnChangePageMerchant();
            //menu.OnItemClose();
        }
        else
        {
            mInventory.Stack[currentItem].Quantity--;
            //menu.OnChangePageMerchant();
            //menu.OnItemChoose(currentItem);
        }
    }

    public int GetPlayerAmount()
    {
        int coinsAmount = 0;
        foreach (ItemsStack stack in Inventory.UInventory.Pages[0].Stack)
        {
            if (stack.Items.ItemName.Contains("Coin"))
            {
                coinsAmount += stack.Quantity;
            }
        }
        return coinsAmount;
    }

    public bool SellItem(int currentItem)
    {
        if (GetPlayerAmount() < mInventory.Stack[currentItem].Price)
        {
            return false;
        }

        Inventory.DecreaseCoinsAmount(mInventory.Stack[currentItem].Price);

        int itemPrefabNum = 0;

        foreach (GameObject itemPrefab in allItems.ItemPrefab)
        {
            if (mInventory.Stack[currentItem].Items.ItemName.Contains(itemPrefab.GetComponent<UsableObject>().Item.ItemName))
            {
                break;
            }
            itemPrefabNum++;
        }

        GameObject item = Instantiate(allItems.ItemPrefab[itemPrefabNum], new Vector3(transform.position.x + UnityEngine.Random.Range(-1.5f, 1.5f), transform.position.y + UnityEngine.Random.Range(1f, 2f), 0f), Quaternion.Euler(Vector3.zero));


        if (item.GetComponent<MeatController>() != null)
        {
            MeatItem mi = (MeatItem)mInventory.Stack[currentItem].Items;
            item.GetComponent<MeatController>().ActiveItemNum = mi.MeatIndex;
        }

        if (Inventory.AddItemToInventory(item))
        {
            Destroy(item);
        }

        RemoveItemFromInventory(currentItem);
        return true;
    }
}