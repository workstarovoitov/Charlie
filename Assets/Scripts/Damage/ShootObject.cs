using UnityEngine;
[CreateAssetMenu(menuName = "AttackObject/Shoot")]

[System.Serializable]
public class ShootObject : AttackObject 
{
		
	[Header ("Shoot Settings")]

	[SerializeField] private ANGLETYPE angleType;
	public ANGLETYPE AngleType
	{
		get => angleType;
		set => angleType = value;
	}


	[SerializeField] private Vector2 colOffsetMid;
	public Vector2 ColOffsetMid
	{
		get => colOffsetMid;
	}
	
	[SerializeField] private Vector2 colOffsetHigh;
	public Vector2 ColOffsetHigh
	{
		get => colOffsetHigh;
	}
	
	[SerializeField] private Vector2 colOffsetLow;
	public Vector2 ColOffsetLow
	{
		get => colOffsetLow;
	}
	
	[SerializeField] private Vector2 shootAngle;
	public Vector2 ShootAngle
	{
		get => shootAngle;
		set => shootAngle = value;
	}
	
	[SerializeField] private int shootDirection;
	public int ShootDirection
	{
		get => shootDirection;
		set => shootDirection = value;
	}
	
	[Header("Projectile")]
	[SerializeField] private GameObject projectile;
	public GameObject Projectile
    {
		get => projectile;
    }

}
public enum ANGLETYPE
{
	MID = 0,
	HIGH = 1,
	LOW = 2,
};