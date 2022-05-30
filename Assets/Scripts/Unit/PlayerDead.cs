using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDead : MonoBehaviour
{

    public static event Action OnPlayerDead;

    public void PlayerDeadSubmit()
    {
        OnPlayerDead?.Invoke();
    }
}
