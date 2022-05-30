using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private int idleNum = 4;
    [SerializeField] private int idleDelay = 4;
    private int idleCurrent = 0;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void AE_IdleCheck()
    {
        if (idleDelay > 0)
        {
            idleDelay--;
            return;
        }
        idleCurrent++;
        if (idleCurrent >= idleNum)
        {
            idleCurrent = 0;
            GetComponent<Animator>().SetTrigger("Shoot");
        }
    }
    public void AE_LaserOn()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
    public void AE_LaserOff()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void AE_OffShoot()
    {
        GetComponent<Animator>().SetTrigger("Idle");
    }
}
