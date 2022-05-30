using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class UnitMain : MonoBehaviour
{
    //Store a reference to all the sub player scripts
    [SerializeField] internal UnitState uState;

    [SerializeField] internal UnitAnimator uAnimator;
    [SerializeField] internal UnitSFX uSFX;
   
    [SerializeField] internal UnitCollisions uCollisions;
   
    [SerializeField] internal UnitHealthManager uHealth;
    [SerializeField] internal UnitStaminaManager uStamina;
    [SerializeField] internal UnitMovement uMovement;
    [SerializeField] internal UnitMovementAttack uMovementAttack;
    [SerializeField] internal UnitMovementHit uMovementHit;
    [SerializeField] private LayerMask layer;
      
    //component references
    internal Rigidbody2D rb = null;
    internal BoxCollider2D bc = null;

    public bool isStarted {get; private set;} = false;

    private void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;     
       
        //if (!unitState) unitState = GetComponent<UnitState>();
        if (!bc) bc = GetComponent<BoxCollider2D>();

        //error messages for missing components
        if (!rb) Debug.LogError("No Rigidbody component found on " + gameObject.name);
        //if (!unitState) Debug.LogError("No UnitState component found on " + gameObject.name);
        if (!bc) Debug.LogError("No Capsule Collider found on " + gameObject.name);

    }
    // Start is called before the first frame update
    private void Start()
    {
        isStarted = true;
        gameObject.layer = (int)Mathf.Log(layer, 2);
        uState.DefaultLayer = gameObject.layer;
    }


    public void SpawnVFX(GameObject vfx, int dir = 0, bool child = false, float xOffset = 0, float yOffset = 0)
    {
        if (vfx != null)
        {
            // Set dust spawn position
            Vector3 vfxSpawnPosition = transform.position + new Vector3(xOffset * (int)uMovement.CurrentDirection, yOffset, 0.0f);
            GameObject newVFX = Instantiate(vfx, vfxSpawnPosition, Quaternion.identity) as GameObject;
            // Turn dust in correct X direction
            if (dir == 0)
            {
                dir = (int)uMovement.CurrentDirection;
            }
            newVFX.transform.localScale = newVFX.transform.localScale.x * new Vector3(dir, 1, 1);
            if (child)
            {
                newVFX.transform.parent = transform;
            }
        }
    }


}
