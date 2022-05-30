using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game progress")]
public class GameProgress : ScriptableObject
{
    [SerializeField] private bool viciousRobinDead = false;
    public bool ViciousRobinDead
    {
        get => viciousRobinDead;
        set => viciousRobinDead = value;
    }
   
    [SerializeField] private bool redRougeDead = false;
    public bool RedRougeDead
    {
        get => redRougeDead;
        set => redRougeDead = value;
    }
}
