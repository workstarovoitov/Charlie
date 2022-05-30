using UnityEngine;

public abstract class ControllerDoor : MonoBehaviour
{
    // Inheritors have to implement this (just like with an interface)
    public abstract void SetState(DOORSTATE state);
    public abstract DOORSTATE GetState();
}

public enum DOORSTATE
{
    OPENED = -1,
    CLOSED = 1,
}