using UnityEngine;
using System.Collections.Generic;

public class UnitCollisions : MonoBehaviour
{
	[SerializeField] UnitMain uMain;

	[Header("Settings")]
	[SerializeField] private LayerMask CollisionLayer;
	[SerializeField] private LayerMask LedgeLayer;
    [SerializeField] private LayerMask PlatformLayer;
    [SerializeField] private LayerMask StairsLayer;

    [SerializeField] [Range(15f, 60f)] private float slopeLimit = 60;

    private float normalAngle = 90f;
    public float NormalAngle
    {
        get => normalAngle;
        set => normalAngle = value;
    }
	
    private float bounce = 0.1f;
    private GameObject platform = null;

    public GameObject Platform
    {
        get => platform;
        set => platform = value;
    }

    private int direction;
    private int frontSideCheck;
     
    public bool IsGrounded()
    {
        SetDeriction();
        GetSlopeAngle();
        if (Mathf.Abs(90 - normalAngle) > slopeLimit/* && normalangle > 0.01f*/)
        {
            normalAngle = 90;
            return false;
        }
        RaycastHit2D[] hitColliders = Physics2D.RaycastAll(uMain.bc.bounds.center + Vector3.right * (uMain.bc.bounds.size.x / 2 + uMain.bc.edgeRadius) * direction * frontSideCheck - Vector3.up * (uMain.bc.bounds.size.y / 2), Vector3.down, bounce, CollisionLayer | PlatformLayer);
        foreach (RaycastHit2D hit in hitColliders)
        {
                if (hit && hit.distance >= Mathf.Tan(Mathf.Deg2Rad * ((90 - Mathf.Abs(90 - normalAngle)) / 2)) * uMain.bc.edgeRadius)
                {

                    platform = hit.transform.gameObject;
                    if (IsPlatformOverlap()) continue;
                    uMain.uState.OnPlatform = PlatformLayer == (PlatformLayer | (1 << platform.layer));
                    uMain.uState.SurfaceMaterial = platform.tag;
                    return true;
                }
        }
        return false;
    }
   
    public bool IsTouchingPlatform(GameObject platform)
    {
        RaycastHit2D[] hitColliders = Physics2D.RaycastAll(uMain.bc.bounds.center + Vector3.right * (uMain.bc.bounds.size.x / 2 + uMain.bc.edgeRadius) , Vector3.down, uMain.bc.bounds.size.y / 2 + uMain.bc.edgeRadius + bounce, PlatformLayer);
        foreach (RaycastHit2D hit in hitColliders)
        {
            if (hit.transform.gameObject == platform)
            {
                return true;
            }
        }       
                    hitColliders = Physics2D.RaycastAll(uMain.bc.bounds.center + Vector3.left * (uMain.bc.bounds.size.x / 2 + uMain.bc.edgeRadius) , Vector3.down, uMain.bc.bounds.size.y / 2 + uMain.bc.edgeRadius + bounce, PlatformLayer);
        foreach (RaycastHit2D hit in hitColliders)
        {
            if (hit.transform.gameObject == platform)
            {
                return true;
            }
        }   
        return false;
    }  
    public bool IsTouchingNewPlatform(GameObject platform)
    {
        if (platform != this.platform) return true;
        return false;
    }
    private void GetSlopeAngle()
    {
        float angleFront = 90f;
        float angleBack = 90f;
        float distanceFront = 100f;
        float distanceBack = 100f;

        //check front side
        RaycastHit2D[] hitColliders = Physics2D.RaycastAll(uMain.bc.bounds.center + Vector3.right * (uMain.bc.bounds.size.x / 2 + uMain.bc.edgeRadius) * direction - Vector3.up * (uMain.bc.bounds.size.y / 2), Vector3.down, uMain.bc.bounds.size.y, CollisionLayer | PlatformLayer);
        foreach (RaycastHit2D hit in hitColliders)
        {
            angleFront = 180 - Vector2.Angle(hit.normal, Vector2.right * direction);
            distanceFront = hit.distance;
            if (hit.distance >= Mathf.Tan(Mathf.Deg2Rad * ((90 - Mathf.Abs(90-angleFront)) / 2)) * uMain.bc.edgeRadius)
            {
                break;
            }
        }            

        //check back side
        hitColliders = Physics2D.RaycastAll(uMain.bc.bounds.center + Vector3.left * (uMain.bc.bounds.size.x / 2 + uMain.bc.edgeRadius) * direction - Vector3.up * (uMain.bc.bounds.size.y / 2 ), Vector3.down, uMain.bc.bounds.size.y, CollisionLayer | PlatformLayer);
        foreach (RaycastHit2D hit in hitColliders)
        {

            angleBack = 180 - Vector2.Angle(hit.normal, Vector2.right * direction);
            distanceBack = hit.distance;
            if (distanceBack >= Mathf.Tan(Mathf.Deg2Rad * ((90 - Mathf.Abs(90 - angleBack)) / 2)) * uMain.bc.edgeRadius)
            {
                break;
            }
        }


        if (distanceFront < distanceBack)
        {
            frontSideCheck = 1;
            normalAngle = angleFront;

        }
        else
        {
            frontSideCheck = -1;
            normalAngle = angleBack;
        }
        return;
    }
  
    public bool IsPlatformOverlap()
    {
        RaycastHit2D hit = Physics2D.Raycast(uMain.bc.bounds.center + Vector3.right * (uMain.bc.bounds.size.x / 2 + uMain.bc.edgeRadius) * direction - Vector3.up * (uMain.bc.bounds.size.y / 2), Vector3.left * direction, uMain.bc.bounds.size.x + 2 * uMain.bc.edgeRadius, PlatformLayer);
        if (hit && hit.transform.gameObject == platform)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void SetDeriction()
    {
        int dir = 0;
        if (Mathf.Abs(uMain.rb.velocity.x) > 0.1f)
        {
            dir = (int)Mathf.Sign(uMain.rb.velocity.x);
        }
        else
        {
            dir = (int)uMain.uMovement.CurrentDirection;
            //if (uMain.uMovement.inputDirection.x == 0)
            //{
            //    dir = (int)uMain.uMovement.CurrentDirection;
            //}
            //else
            //{
            //    dir = (int)Mathf.Sign(uMain.uMovement.inputDirection.x);
            //}
        }
        direction = dir;
    }
 
    //check wall in front a unit
    public bool WallInFrontX()
    {
        return Physics2D.BoxCast(uMain.bc.bounds.center - Vector3.up * (uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) / 2 + Vector3.up * ((uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) / 2 + bounce / 2 / Mathf.Tan(Mathf.Deg2Rad * (90 - slopeLimit))) + Vector3.right * (int)uMain.uMovement.CurrentDirection * (uMain.bc.bounds.size.x + 2 * uMain.bc.edgeRadius) / 2,
                                new Vector2(bounce * 2, (uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) - bounce /*/ 2*/ / Mathf.Tan(Mathf.Deg2Rad * (90 - slopeLimit))), 0f, Vector2.right * (int)uMain.uMovement.CurrentDirection, 0f, CollisionLayer);
    }
 
    //check wall in front a unit
    public bool CliffInFrontX()
    {
        return !Physics2D.Raycast(uMain.bc.bounds.center + Vector3.right * (uMain.bc.bounds.size.x*2 ) * (int)uMain.uMovement.CurrentDirection - Vector3.up * (uMain.bc.bounds.size.y / 2), Vector3.down, 3.5f, CollisionLayer | PlatformLayer);
    }

    //check wall behind a unit
    public bool WallInBackX()
    {
        return Physics2D.BoxCast(uMain.bc.bounds.center - Vector3.up * (uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) / 2 + Vector3.up * ((uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) / 2 + bounce / 2 / Mathf.Tan(Mathf.Deg2Rad * (90 - slopeLimit))) + Vector3.left * (int)uMain.uMovement.CurrentDirection * (uMain.bc.bounds.size.x + 2 * uMain.bc.edgeRadius) / 2,
                                new Vector2(bounce * 2, (uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) - bounce / 2 / Mathf.Tan(Mathf.Deg2Rad * (90 - slopeLimit))), 0f, Vector2.left * (int)uMain.uMovement.CurrentDirection, 0f, CollisionLayer);
    }
   
    public bool PlatformInBackX()
    {
        return Physics2D.BoxCast(uMain.bc.bounds.center + Vector3.up * ((uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) / 2) + Vector3.left * (int)uMain.uMovement.CurrentDirection * ((uMain.bc.bounds.size.x + 2 * uMain.bc.edgeRadius) / 2 + bounce / 2),
                        new Vector2(2 * bounce, 8 * bounce), 0f, Vector2.left * (int)uMain.uMovement.CurrentDirection, 0f, PlatformLayer | CollisionLayer);
    }

    //ledge check
    public RaycastHit2D LedgeInFrontX()
    {
        return Physics2D.BoxCast(uMain.bc.bounds.center + Vector3.up * ((uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) / 2 ) + Vector3.right * (int)uMain.uMovement.CurrentDirection * ((uMain.bc.bounds.size.x + 2 * uMain.bc.edgeRadius) / 2 + bounce / 2),
                                new Vector2( 3 * bounce, 4 * bounce), 0f, Vector2.right * (int)uMain.uMovement.CurrentDirection, 0f, LedgeLayer);
    }
    
    //stair check
    public RaycastHit2D StairOnTop()
    {
        return Physics2D.BoxCast(uMain.bc.bounds.center + Vector3.up * ((uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) / 2 ) ,
                                new Vector2(uMain.bc.bounds.size.x, 2 * bounce), 0f, Vector2.up, 0f, StairsLayer);
    }

    //additional wall check for WALLSLIDE
    public bool WallForSlideInFront()
    {
        return  Physics2D.BoxCast(uMain.bc.bounds.center + Vector3.up * ((uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) / 2 ) + Vector3.right * (int)uMain.uMovement.CurrentDirection * ((uMain.bc.bounds.size.x + 2 * uMain.bc.edgeRadius) / 2 + bounce / 2),
                                new Vector2( 2 * bounce, 2 * bounce), 0f, Vector2.right * (int)uMain.uMovement.CurrentDirection, 0f, CollisionLayer)    &&
                Physics2D.BoxCast(uMain.bc.bounds.center + Vector3.down * ((uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) / 2) + Vector3.right * (int)uMain.uMovement.CurrentDirection * ((uMain.bc.bounds.size.x + 2 * uMain.bc.edgeRadius) / 2 + bounce / 2),
                                new Vector2(2 * bounce, 2 * bounce), 0f, Vector2.right * (int)uMain.uMovement.CurrentDirection, 0f, CollisionLayer);
    }

    public bool IsWallBetweenTarget(GameObject target)
    {

        RaycastHit2D[] hitColliders = Physics2D.RaycastAll(transform.position, target.transform.position - transform.position, (target.transform.position - transform.position).magnitude, CollisionLayer);
        foreach (RaycastHit2D hit in hitColliders)
        {
            if (hit.transform.gameObject != target)
            {
                return true;
            }
        }
        return false;
    }
    //draw collision probes in the unity editor
#if UNITY_EDITOR
    void OnDrawGizmos()
	{
		if (!Application.isPlaying) return;
        // front wall check box
        
        Gizmos.color = Color.red;
        Vector3 wallXCheckBoxSize = new Vector3(bounce*2, (uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) - bounce /*/ 2*/ / Mathf.Tan(Mathf.Deg2Rad * (90 - slopeLimit)), bounce);
        Vector3 frontWallCheckBoxPosition = uMain.bc.bounds.center - Vector3.up * (uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) / 2 + Vector3.up * ((uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) / 2 + bounce / 2 / Mathf.Tan(Mathf.Deg2Rad * (90 - slopeLimit))) + Vector3.right * (int)uMain.uMovement.CurrentDirection * (uMain.bc.bounds.size.x + 2 * uMain.bc.edgeRadius) / 2;
        Gizmos.DrawWireCube(frontWallCheckBoxPosition, wallXCheckBoxSize);
       
        // back wall check box
        Gizmos.color = Color.blue;
        Vector3 backWallCheckBoxPosition = uMain.bc.bounds.center - Vector3.up * (uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) / 2 + Vector3.up * ((uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) / 2 + bounce / 2 / Mathf.Tan(Mathf.Deg2Rad * (90 - slopeLimit))) + Vector3.left * (int)uMain.uMovement.CurrentDirection * (uMain.bc.bounds.size.x + 2 * uMain.bc.edgeRadius) / 2;
        Gizmos.DrawWireCube(backWallCheckBoxPosition, wallXCheckBoxSize);
        
        // ledge check boxes
        Gizmos.color = Color.cyan;
        Vector3 ledgeCheckBoxSize = new Vector3(3 * bounce, 4 * bounce, bounce);
        Vector3 ledgeCheckBoxPosition = uMain.bc.bounds.center + Vector3.up * ((uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) / 2  )+ Vector3.right * (int)uMain.uMovement.CurrentDirection * ((uMain.bc.bounds.size.x + 2 * uMain.bc.edgeRadius) / 2 + bounce/2);
        Gizmos.DrawWireCube(ledgeCheckBoxPosition, ledgeCheckBoxSize);
        ledgeCheckBoxSize = new Vector3(2 * bounce, 8 * bounce, bounce);
        ledgeCheckBoxPosition = uMain.bc.bounds.center + Vector3.up * ((uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) / 2) + Vector3.left * (int)uMain.uMovement.CurrentDirection * ((uMain.bc.bounds.size.x + 2 * uMain.bc.edgeRadius) / 2 + bounce / 2);
        Gizmos.DrawWireCube(ledgeCheckBoxPosition, ledgeCheckBoxSize);    
        // stair check box 
        Gizmos.color = Color.cyan;
        Vector3 stairCheckBoxSize = new Vector3(uMain.bc.bounds.size.x, 2 * bounce, bounce);
        Vector3 stairCheckBoxPosition = uMain.bc.bounds.center + Vector3.up * ((uMain.bc.bounds.size.y + 2 * uMain.bc.edgeRadius) / 2);
        Gizmos.DrawWireCube(stairCheckBoxPosition, stairCheckBoxSize);

        //ground check rays 
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(uMain.bc.bounds.center + Vector3.right * (uMain.bc.bounds.size.x / 2 + uMain.bc.edgeRadius) * direction * frontSideCheck - Vector3.up * (uMain.bc.bounds.size.y / 2), Vector3.down);
        Gizmos.DrawRay(uMain.bc.bounds.center + Vector3.right * (uMain.bc.bounds.size.x  + uMain.bc.edgeRadius) * direction * frontSideCheck - Vector3.up * (uMain.bc.bounds.size.y / 2), Vector3.down);

        //cliff check rays 
        Gizmos.color = Color.gray;
        Gizmos.DrawRay(uMain.bc.bounds.center + Vector3.right * (uMain.bc.bounds.size.x * 2) * (int)uMain.uMovement.CurrentDirection - Vector3.up * (uMain.bc.bounds.size.y / 2), Vector3.down);
    
        //Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(uMain.bc.bounds.center + Vector3.right * (int)uMain.uMovement.CurrentDirection * 3 * (uMain.bc.edgeRadius), uMain.bc.size.x / 2f);

    }
#endif
}
