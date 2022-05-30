using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionsControl : MonoBehaviour
{

    private GameObject target;			//current target
    private UnitInventoryManager uIO;

    private Image[] flaskImages;
    // Start is called before the first frame update

    public void UpdateFlasks()
    {
        SetFlaskImage();
        SetFlaskQuantity();
    }

    public void SetFlaskImage()
    {
        if (uIO.UInventory.Pages[uIO.PotionsPageIndex].Stack.Count > 0 && uIO.UInventory.Pages[uIO.PotionsPageIndex].Stack[uIO.ActivePotionIndex].Items != null)
        {
            PotionItem po = (PotionItem)uIO.UInventory.Pages[uIO.PotionsPageIndex].Stack[uIO.ActivePotionIndex].Items;
            flaskImages[0].enabled = true;
            flaskImages[0].sprite = po.ItemSprite;
            flaskImages[1].enabled = false;
            if (po.IsCooldown)
            {
                flaskImages[0].color = new Vector4(0.5f, 0.5f, 0.5f, 0.75f);
            }
            else
            {
                flaskImages[0].color = new Vector4(1f, 1f, 1f, 1f);
            }

        }
        else
        {
            flaskImages[0].enabled = false;
            flaskImages[1].enabled = true;
        }
    }
    
    public void UpdateTarget()
    {
        if (!target) target = GameObject.FindGameObjectWithTag("Player");

        uIO = target.GetComponent<UnitInventoryManager>();
        flaskImages = GetComponentsInChildren<Image>();
        UpdateFlasks();
        uIO.flasksChanged += UpdateFlasks;
    }
    public void SetFlaskQuantity()
    {
        Text quantityText = GetComponentInChildren<Text>();
        
        if (uIO.UInventory.Pages[uIO.PotionsPageIndex].Stack.Count > 0 && uIO.UInventory.Pages[uIO.PotionsPageIndex].Stack[uIO.ActivePotionIndex].Items != null && uIO.UInventory.Pages[uIO.PotionsPageIndex].Stack[uIO.ActivePotionIndex].Quantity > 1)
        {
            quantityText.text = uIO.UInventory.Pages[uIO.PotionsPageIndex].Stack[uIO.ActivePotionIndex].Quantity.ToString();
        }
        else
        {
            quantityText.text = "";
        }
    }
}
