using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RougeDead : MonoBehaviour
{
    public void SaveRougeDeath()
    {
        GameObject progress = GameObject.FindGameObjectWithTag("UserInterface");
        progress.GetComponent<GameProgressManager>().Progress.RedRougeDead = true;
    }
}
