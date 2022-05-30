using UnityEngine;

public abstract class SpecialMove : MonoBehaviour
{
    // Inheritors have to implement this (just like with an interface)
    public abstract void OnSpecialMove();
    public abstract bool IsSpecialMoveFaced();
}



