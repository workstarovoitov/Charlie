using UnityEngine;
[CreateAssetMenu(menuName = "AttackObject/Throw")]

[System.Serializable]
public class ThrowObject : AttackObject 
{
		
	[Header ("Throw Settings")]

	[SerializeField] private Vector2 colOffset;
	public Vector2 ColOffset
	{
		get => colOffset;
	}
		
	[SerializeField] private Vector2 throwAngle;
	public Vector2 ThrowAngle
	{
		get => throwAngle;
		set => throwAngle = value;
	}
	
	[SerializeField] private int throwDirection;
	public int ThrowDirection
	{
		get => throwDirection;
		set => throwDirection = value;
	}
	[Header("Projectile")]
	[SerializeField] private GameObject projectile;
	public GameObject Projectile
    {
		get => projectile;
    }

}
