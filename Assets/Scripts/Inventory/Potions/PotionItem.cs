using UnityEngine;

[System.Serializable]
public class PotionItem : InventoryItem
{

	[Header("Timing Settings")]
	[SerializeField] private float cooldownTime = .1f;
	public float CooldownTime => cooldownTime;
	
	private float lastItemUseTime = 0f;
	public float LastItemUseTime
	{
		get => lastItemUseTime;
		set => lastItemUseTime = value;
	}
	
	private bool isCooldown = false;
	public bool IsCooldown
	{
		get => isCooldown;
		set => isCooldown = value;
	}
}

[System.Serializable]
public class PotionsStack
{
	[SerializeField] private PotionItem potions = null; //a list of attacks
	public PotionItem Potions
    {
		get => potions;
		set => potions = value;
	}
	
	[SerializeField] private int quantity = 1;
	public int Quantity
	{
		get => quantity;
		set => quantity = value;
	}
	

}