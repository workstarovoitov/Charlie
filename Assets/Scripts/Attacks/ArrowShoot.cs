using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrowShoot : MonoBehaviour
{

	[SerializeField] UnitMain uMain;

	void AE_ArrowShoot()
	{
		ShootObject to = (ShootObject)uMain.uMovementAttack.LastAttack;
		float angle = Mathf.Atan2(to.ShootAngle.y, to.ShootAngle.x) * Mathf.Rad2Deg;
		switch (to.AngleType)
		{
			case ANGLETYPE.MID:
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetMid.x * to.ShootDirection, transform.position.y + to.ColOffsetMid.y, 0f), Quaternion.Euler(Vector3.forward * angle));
				break;
			case ANGLETYPE.HIGH:
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetHigh.x * to.ShootDirection, transform.position.y + to.ColOffsetHigh.y, 0f), Quaternion.Euler(Vector3.forward * angle));
				break;
			case ANGLETYPE.LOW:
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetLow.x * to.ShootDirection, transform.position.y + to.ColOffsetLow.y, 0f), Quaternion.Euler(Vector3.forward * angle));
				break;
		}
	}

	void AE_ArrowShootDouble()
	{
		ShootObject to = (ShootObject)uMain.uMovementAttack.LastAttack;
		float angle = Mathf.Atan2(to.ShootAngle.y, to.ShootAngle.x) * Mathf.Rad2Deg;
		switch (to.AngleType)
		{
			case ANGLETYPE.MID:
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetMid.x * to.ShootDirection, transform.position.y + to.ColOffsetMid.y, 0f), Quaternion.Euler(Vector3.forward * (angle + 2)));
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetMid.x * to.ShootDirection, transform.position.y + to.ColOffsetMid.y, 0f), Quaternion.Euler(Vector3.forward * (angle - 2)));
				break;
			case ANGLETYPE.HIGH:
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetHigh.x * to.ShootDirection, transform.position.y + to.ColOffsetHigh.y, 0f), Quaternion.Euler(Vector3.forward * (angle + 2)));
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetHigh.x * to.ShootDirection, transform.position.y + to.ColOffsetHigh.y, 0f), Quaternion.Euler(Vector3.forward * (angle - 2)));
				break;
			case ANGLETYPE.LOW:
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetLow.x * to.ShootDirection, transform.position.y + to.ColOffsetLow.y, 0f), Quaternion.Euler(Vector3.forward * (angle + 2)));
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetLow.x * to.ShootDirection, transform.position.y + to.ColOffsetLow.y, 0f), Quaternion.Euler(Vector3.forward * (angle - 2)));
				break;
		}
	}

	void AE_ArrowShootTriple()
	{
		ShootObject to = (ShootObject)uMain.uMovementAttack.LastAttack;
		float angle = Mathf.Atan2(to.ShootAngle.y, to.ShootAngle.x) * Mathf.Rad2Deg;
		switch (to.AngleType)
		{
			case ANGLETYPE.MID:
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetMid.x * to.ShootDirection, transform.position.y + to.ColOffsetMid.y, 0f), Quaternion.Euler(Vector3.forward * angle));
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetMid.x * to.ShootDirection, transform.position.y + to.ColOffsetMid.y, 0f), Quaternion.Euler(Vector3.forward * (angle + 5)));
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetMid.x * to.ShootDirection, transform.position.y + to.ColOffsetMid.y, 0f), Quaternion.Euler(Vector3.forward * (angle - 5)));
				break;
			case ANGLETYPE.HIGH:
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetHigh.x * to.ShootDirection, transform.position.y + to.ColOffsetHigh.y, 0f), Quaternion.Euler(Vector3.forward * angle));
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetHigh.x * to.ShootDirection, transform.position.y + to.ColOffsetHigh.y, 0f), Quaternion.Euler(Vector3.forward * (angle + 5)));
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetHigh.x * to.ShootDirection, transform.position.y + to.ColOffsetHigh.y, 0f), Quaternion.Euler(Vector3.forward * (angle - 5)));
				break;
			case ANGLETYPE.LOW:
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetLow.x * to.ShootDirection, transform.position.y + to.ColOffsetLow.y, 0f), Quaternion.Euler(Vector3.forward * angle));
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetLow.x * to.ShootDirection, transform.position.y + to.ColOffsetLow.y, 0f), Quaternion.Euler(Vector3.forward * (angle + 5)));
				Instantiate(to.Projectile, new Vector3(transform.position.x + to.ColOffsetLow.x * to.ShootDirection, transform.position.y + to.ColOffsetLow.y, 0f), Quaternion.Euler(Vector3.forward * (angle - 5)));
				break;
		}
	}

}
