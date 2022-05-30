using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RougeRangedAttacks : MonoBehaviour
{

	[SerializeField] UnitMain uMain;

	void AE_RougeEnergyThrow()
	{
		ThrowObject to = (ThrowObject)uMain.uMovementAttack.LastAttack;
		if(uMain.uMovement.CurrentDirection == DIRECTION.RIGHT)
        {
			Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffset.x * to.ThrowDirection, transform.position.y + to.ColOffset.y, 0f), Quaternion.Euler(Vector3.zero));
        }
        else
        {
			Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffset.x * to.ThrowDirection, transform.position.y + to.ColOffset.y, 0f), Quaternion.Euler(Vector3.up * 180));
		}
	}

	void AE_RougeShurikenThrow()
	{
		ThrowObject to = (ThrowObject)uMain.uMovementAttack.LastAttack;
		if (uMain.uMovement.CurrentDirection == DIRECTION.RIGHT)
		{
			Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffset.x * to.ThrowDirection, transform.position.y + to.ColOffset.y, 0f), Quaternion.Euler(Vector3.zero));
		}
		else
		{
			Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffset.x * to.ThrowDirection, transform.position.y + to.ColOffset.y, 0f), Quaternion.Euler(Vector3.up * 180));
		}
	}

}
