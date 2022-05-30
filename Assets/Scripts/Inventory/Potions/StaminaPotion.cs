using UnityEngine;
[CreateAssetMenu(menuName = "InventoryObject/StaminaPotion")]

[System.Serializable]
public class StaminaPotion : PotionItem
{
	[Header("Potion Settings")]
	[SerializeField] private float staminaRestoreSpeed;
	public float StaminaRestoreSpeed
	{
		get => staminaRestoreSpeed;
	}
	
	[SerializeField] private int staminaRestoreTime;
	public int StaminaRestoreTime
	{
		get => staminaRestoreTime;
	}
	[SerializeField] private float staminaRestorePortion;
	public float StaminaRestorePortion
	{
		get => staminaRestorePortion;
	}
}
