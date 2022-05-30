using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobinDead : MonoBehaviour
{
    public void SaveRobinDeath()
    {
        GameObject progress = GameObject.FindGameObjectWithTag("UserInterface");
        progress.GetComponent<GameProgressManager>().Progress.ViciousRobinDead = true;
    }
}
