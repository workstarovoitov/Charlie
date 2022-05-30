using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class StickToObject : MonoBehaviour
{
    [SerializeField] private LayerMask CollisionLayer;

    void OnTriggerEnter2D(Collider2D col)
    {
      if ((CollisionLayer.value & (1 << col.transform.gameObject.layer)) > 0)
        {
            col.gameObject.transform.SetParent(transform);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if ((CollisionLayer.value & (1 << col.transform.gameObject.layer)) > 0)
        {
            col.gameObject.transform.SetParent(null);
        }
    }
}
