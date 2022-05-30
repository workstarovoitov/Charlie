using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

class soundtrackBanditsSwitcher : MonoBehaviour
{
    [SerializeField] private soundtrackBandits soundtrack;
    [SerializeField] private LayerMask enemyLayer;

    public void Start()
    {
        gameObject.SetActive(false);
    }
    public void Update()
    {
        if (!IsEnemyNear())
        {
            soundtrack.SwitchMode(SOUNDMODE.EXPLORATION, 500f);
        }
    }

    private bool IsEnemyNear()
    {
        Collider2D hitCollider = Physics2D.OverlapBox(transform.position, GetComponent<BoxCollider2D>().size, 0f, enemyLayer);
        if (hitCollider != null)
        {
            return true;
        }
        return false;
    }
}