using UnityEngine;
using System;
using System.Collections.Generic;
using MenuManagement;
using Random = UnityEngine.Random;

public delegate void UpdateFlasksUI();


public class UnitInventoryManager : MonoBehaviour
{
    [SerializeField] UnitMain uMain;
    [Header("Invenory")]

    UnitInventory uInventory;
    //[SerializeField] private UnitInventory uInventory;
    [SerializeField] private UnitInventory uInventoryBak;
    //[SerializeField] private UnitInventory uInventoryBackup;
    [SerializeField] private AllAvailableItems allItems;
    public UnitInventory UInventory
    {
        get => uInventory;
    }
    //potinos
    private int potionsPageIndex = 0;
    public int  PotionsPageIndex
    {
        get => potionsPageIndex;
    }

    private int activePotionIndex = 0;
    public int ActivePotionIndex
    {
        get => activePotionIndex;
        set => activePotionIndex = value;
    }
    [Header("SFX")]
    [SerializeField] private string emptyFlasksSFX;

    public event UpdateFlasksUI flasksChanged;

    private void Awake()
    {
        uInventory = new UnitInventory();
        if (!ES3.KeyExists("inventoryBak"))
        {
            ES3.Save("inventoryBak", uInventoryBak);
        }


        if (ES3.KeyExists("inventory"))
        {
            ES3.LoadInto("inventory", uInventory);
        }
        else
        {
            ES3.LoadInto("inventoryBak", uInventory);
            ES3.Save("inventory", uInventory);
        }
        //activePotionIndex = ES3.Load("activeflask", 0);

        potionsPageIndex = 0;
        for (potionsPageIndex = 0; potionsPageIndex < uInventory.Pages.Count; potionsPageIndex++)
        {
            if (uInventory.Pages[potionsPageIndex].ItemsThatFitsToPage == INVENTORYPAGESTYPE.POTION) break;
        }
        // remove empty items
        //foreach (InventoryPage page in uInventory.Pages)
        //{
        //    foreach (ItemsStack stack in page.Stack)
        //    {
        //        if (stack.Items == null || stack.Quantity <= 0)
        //        {
        //            page.Stack.Remove(stack);
        //        }
        //    }

        //    while (page.Stack.Count > page.SlotsInInventory)
        //    {
        //        page.Stack.RemoveAt(page.Stack.Count-1);
        //    }

        //    foreach (ItemsStack stack in page.Stack)
        //    { 
        //        if (stack.Quantity > stack.Items.StackCount)
        //        {
        //            stack.Quantity = stack.Items.StackCount;
        //        }
        //    }
        //}

        for (int i = 0; i < allItems.ItemPrefab.Count; i++)
        {
           
            if (allItems.ItemPrefab[i].GetComponent<UsableObject>().Item.Page == INVENTORYPAGESTYPE.POTION)
            { 
                PotionItem potion = (PotionItem)allItems.ItemPrefab[i].GetComponent<UsableObject>().Item;
                potion.LastItemUseTime = 0;
                potion.IsCooldown = false;
            }
        }
    }

    private void Start()
    {
        ControllerSavePoint.OnSave += SaveInventory;
        ControllerSceneSwitch.OnSave += SaveInventory;
        PauseMenu.OnSaveButton += SaveInventory;

        uInventory = new UnitInventory();

        ES3.LoadInto("inventory", uInventory);
        activePotionIndex = ES3.Load("activeflask", 0); 
        Debug.Log("inventory Loaded");
    }
    private void OnDestroy()
    {
        ControllerSavePoint.OnSave -= SaveInventory;
        ControllerSceneSwitch.OnSave -= SaveInventory;
        PauseMenu.OnSaveButton -= SaveInventory;
    }
    public void SaveInventory()
    {
        ES3.Save("inventory", uInventory);
        ES3.Save("activeflask", activePotionIndex);
        Debug.Log("inventory saved");
    }

    public void OnFlask()
    {
        if (uInventory.Pages[potionsPageIndex].Stack.Count < 1)
        {
            uMain.uSFX.PlayOneShotSFX(emptyFlasksSFX);
            return;
        }

        PotionItem po = (PotionItem)uInventory.Pages[potionsPageIndex].Stack[activePotionIndex].Items;
        if (po.CooldownTime + po.LastItemUseTime > Time.time)
        {
            uMain.uSFX.PlayOneShotSFX(emptyFlasksSFX);
            return;
        }
      
        if (uInventory.Pages[potionsPageIndex].Stack[activePotionIndex].Quantity > 0)
        {
            if (po is HealthPotion)
            {
                uMain.uHealth.UseItem((HealthPotion)po);
            }
            if (po is HealPotion)
            {
                uMain.uHealth.UseItem((HealPotion)po);
            } 
            if (po is StaminaPotion)
            {
                uMain.uStamina.UseItem((StaminaPotion)po);
            }
            if (po is BeerPotion)
            {
                uMain.uStamina.UseItem((BeerPotion)po);
            }

            po.LastItemUseTime = Time.time;
            po.IsCooldown = true;
            Invoke(nameof(ResetCooldownFlask), po.CooldownTime + 0.1f);
            uMain.uSFX.PlayOneShotSFX(po.UseSFX, false);

            uInventory.Pages[potionsPageIndex].Stack[activePotionIndex].Quantity--;



            if (uInventory.Pages[potionsPageIndex].Stack[activePotionIndex].Quantity == 0)
            {
                uInventory.Pages[potionsPageIndex].Stack.RemoveAt(activePotionIndex);
                activePotionIndex = 0;
            }

            flasksChanged?.Invoke();
        }
    }

    public void OnFlaskChange()
    {
        if (uInventory.Pages[potionsPageIndex].Stack.Count < 1)
        {
            uMain.uSFX.PlayOneShotSFX(emptyFlasksSFX);
            return; 
        }
        uMain.uSFX.PlayOneShotSFX(uInventory.Pages[potionsPageIndex].Stack[activePotionIndex].Items.GrabSFX);
        activePotionIndex++;
        if (activePotionIndex == uInventory.Pages[potionsPageIndex].Stack.Count)
        {
            activePotionIndex = 0;
        }
        flasksChanged?.Invoke();
    }
   
    private void ResetCooldownFlask()
    {
        for(int i = 0; i < uInventory.Pages[potionsPageIndex].Stack.Count; i++)
        {
            PotionItem po = (PotionItem)uInventory.Pages[potionsPageIndex].Stack[i].Items;
            if (po.IsCooldown && po.CooldownTime + po.LastItemUseTime < Time.time)
            {
                po.IsCooldown = false;
                flasksChanged?.Invoke();
            }
        }
       
    }
   
    public void ThrowItem(int pageNum, int stackNum)
    {
        int itemPrefabNum = 0;

        foreach (GameObject itemPrefab in allItems.ItemPrefab)
        {
            if (uInventory.Pages[pageNum].Stack[stackNum].Items.ItemName.Contains(itemPrefab.GetComponent<UsableObject>().Item.ItemName))
            {
                break;
            }
            itemPrefabNum++;
        }


        if (allItems.ItemPrefab[itemPrefabNum].GetComponent<CoinBagController>() != null)
        {
            GameObject coinsBag = Instantiate(allItems.ItemPrefab[itemPrefabNum], new Vector3(transform.position.x + Random.Range(-1.5f, 1.5f), transform.position.y + Random.Range(1f, 2f), 0f), Quaternion.Euler(Vector3.zero));
            coinsBag.GetComponent<CoinBagController>().Amount = uInventory.Pages[pageNum].Stack[stackNum].Quantity;
        }
        else if (allItems.ItemPrefab[itemPrefabNum].GetComponent<MeatController>() != null)
        {
            GameObject meatPiece = Instantiate(allItems.ItemPrefab[itemPrefabNum], new Vector3(transform.position.x + Random.Range(-1.5f, 1.5f), transform.position.y + Random.Range(1f, 2f), 0f), Quaternion.Euler(Vector3.zero));
            MeatItem mi = (MeatItem)uInventory.Pages[pageNum].Stack[stackNum].Items;
            meatPiece.GetComponent<MeatController>().ActiveItemNum = mi.MeatIndex;
        }
        else
        {            
            for (int k = 0; k < uInventory.Pages[pageNum].Stack[stackNum].Quantity; k++)
            {
                Instantiate(allItems.ItemPrefab[itemPrefabNum], new Vector3(transform.position.x + Random.Range(-1.5f, 1.5f), transform.position.y + Random.Range(1f, 2f), 0f), Quaternion.Euler(Vector3.zero));
            }
        }

        

        uMain.uSFX.PlayOneShotSFX(uInventory.Pages[pageNum].Stack[stackNum].Items.SpawnSFX);
        uInventory.Pages[pageNum].Stack.RemoveAt(stackNum);

        OnInventoryChanges();
    }

    public void DecreaseCoinsAmount(int coins) 
    {
        for (int stackNum = uInventory.Pages[0].Stack.Count-1; stackNum >= 0; stackNum--)
        {
            if (uInventory.Pages[0].Stack[stackNum].Items.ItemName.Contains("Coin"))
            {
                if (uInventory.Pages[0].Stack[stackNum].Quantity <= coins)
                {
                    coins -= uInventory.Pages[0].Stack[stackNum].Quantity;
                    uInventory.Pages[0].Stack.RemoveAt(stackNum);
                }
                else
                {
                    uInventory.Pages[0].Stack[stackNum].Quantity -= coins;
                }
                if (coins == 0) return;
            }
        }

    }

    public bool AddItemToInventory(GameObject newItemGO, int amount = 1)
    {
        InventoryItem newItem = newItemGO.GetComponent<UsableObject>().Item;
        int itemPrefabNum = 0;
       
        foreach (GameObject itemPrefab in allItems.ItemPrefab)
        {
            if (newItem.ItemName.Contains(itemPrefab.GetComponent<UsableObject>().Item.ItemName))
            {
                break;
            }
            itemPrefabNum++;
        }

        int itemPageIndex = GetPageIndex(newItem);
        //check if we have the same item
        foreach (ItemsStack stack in uInventory.Pages[itemPageIndex].Stack)
        {
            if (newItem.ItemName.Contains(stack.Items.ItemName) && stack.Quantity < stack.Items.StackCount)
            {
                if (stack.Quantity + amount > stack.Items.StackCount)
                {
                    amount = stack.Quantity + amount - stack.Items.StackCount;
                    stack.Quantity = stack.Items.StackCount;
                    newItemGO.GetComponent<UsableObject>().Amount = amount;
                    if (AddItemToInventory(newItemGO, amount))
                    {
                        return true;
                    }
                    return false;
                }
                stack.Quantity += amount;
                OnInventoryChanges();
                return true;
            }
        }

        //add item into empty slot
        if (uInventory.Pages[itemPageIndex].Stack.Count < uInventory.Pages[itemPageIndex].SlotsInInventory)
        {
            if (amount > newItem.StackCount)
            {
                ItemsStack newStack = new ItemsStack
                {
                    Quantity = newItem.StackCount,
                    Items = newItem
                };
                uInventory.Pages[itemPageIndex].Stack.Add(newStack);

                amount = amount - newItem.StackCount;

                newItemGO.GetComponent<UsableObject>().Amount = amount;
                if (AddItemToInventory(newItemGO, amount))
                {
                    return true;
                }
                return false;
            }
            else
            {
                ItemsStack newStack = new ItemsStack
                {
                    Quantity = amount,
                    Items = newItem
                };

                uInventory.Pages[itemPageIndex].Stack.Add(newStack);
            }


            OnInventoryChanges();
            return true;
        }

        return false;
    }

    public void RemoveItemFromInventory(InventoryItem newItem)
    {
        int itemPageIndex = GetPageIndex(newItem);

        int itemsIndex = 0;
        foreach (ItemsStack stack in uInventory.Pages[itemPageIndex].Stack)
        {
            if (newItem.ItemName.Contains(stack.Items.ItemName))
            {
                if (stack.Quantity > 1)
                {
                    stack.Quantity--;
                }
                else
                {
                    uInventory.Pages[itemPageIndex].Stack.Remove(stack);
                }
                OnInventoryChanges();
                return;
            }
            itemsIndex++;
        }
    }

    public void OnInventoryChanges()
    {
        flasksChanged?.Invoke();
    }
    public int GetPageIndex(InventoryItem newItem)
    {
        int index = 0;
        for (index = 0; index < uInventory.Pages.Count; index++)
        {
            if (uInventory.Pages[index].ItemsThatFitsToPage == newItem.Page) break;
        }

        return index;
    }

}
