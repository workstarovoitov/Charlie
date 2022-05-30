using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuManagement;
public class EndMenuRun : MonoBehaviour
{
    public void RunMenu()
    {
        StartCoroutine(RunEndMenu());
    }
    IEnumerator RunEndMenu()
    {
        yield return new WaitForSecondsRealtime(5);
        EndMenu.Open();
    }
}
