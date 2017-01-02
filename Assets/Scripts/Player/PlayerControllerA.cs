 using UnityEngine;
using System.Collections;

public class PlayerControllerA : MonoBehaviour
{

    public static PlayerControllerA Instance;

    //Player
    public float maxJump = 8;
    public float timeToReachMaxJump = .7f;
    public float timeToReachMaxSpeed = .15f;
    public float maxSpeed = 18;

    public LayerMask collidedLayer;

    public GameObject flipObject;

    float fAccel;
    float jumpVelocity;
    Vector3 vDirection;
    float sVelocity;

    float rayOffset = 0.1f;
    int horizontalRayCount = 6;
    int verticalRayCount = 6;

    public float maxClimbAngle = 60;
    public float maxDescendAngle = 80;

    float hRaySpread;
    float vRaySpread;

    struct RaycastOrigins
    {
        public Vector3 topLeft, topRight;
        public Vector3 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngleNew, slopeAngleOld;
        public Vector3 velocityOld;

        public void Reset()
        {
            above = false;
            below = false;
            left = false;
            right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngleNew;
            slopeAngleNew = 0;
        }
    }

    CapsuleCollider myCollider;
    RaycastOrigins raycastOrigins;
    public CollisionInfo myCollision;

    void Start()
    {
        
        fAccel = -(2 * maxJump) / Mathf.Pow(timeToReachMaxJump, 2);
        jumpVelocity = Mathf.Abs(fAccel) * timeToReachMaxJump;
        
        myCollider = GetComponent<CapsuleCollider>();
        CalculateRaySpacing();
    }

    // Update is called once per frame
    void Update()
    {        
        if (myCollision.above || myCollision.below)
        {
            vDirection.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && myCollision.below)
        {
            vDirection.y = jumpVelocity;
        }

        float xVelocity = input.x * maxSpeed;
        vDirection.x = Mathf.SmoothDamp(vDirection.x, xVelocity, ref sVelocity, timeToReachMaxSpeed);
        vDirection.y += fAccel * Time.deltaTime;

        if (vDirection.x > 0)
        {
            if (flipObject.transform.localScale.x < 0)
            {
                flipObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else if (vDirection.x < 0)
        {
            if (flipObject.transform.localScale.x > 0)
            {
                flipObject.transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        vDirection.z = 0;
        Move(vDirection * Time.deltaTime);
    }

    public void addForce(Vector3 f)
    {
    
        myCollision.below = false;

        vDirection += f;        
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        myCollision.Reset();
        myCollision.velocityOld = velocity;

        if (velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float xDir = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + rayOffset;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector3 rayOrigin = (xDir == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector3.up * (hRaySpread * i);
            RaycastHit hit;

            Debug.DrawRay(rayOrigin, Vector3.right * xDir * rayLength, Color.red);

            if (Physics.Raycast(rayOrigin, Vector3.right * xDir, out hit, rayLength, collidedLayer))
            {

                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    if (myCollision.descendingSlope)
                    {
                        myCollision.descendingSlope = false;
                        velocity = myCollision.velocityOld;
                    }
                    float distanceToSlope = 0;
                    if (slopeAngle != myCollision.slopeAngleOld)
                    {
                        distanceToSlope = hit.distance - rayOffset;
                        velocity.x -= distanceToSlope * xDir;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlope * xDir;
                }

                if (!myCollision.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    velocity.x = (hit.distance - rayOffset) * xDir;
                    rayLength = hit.distance;

                    if (myCollision.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(myCollision.slopeAngleNew * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    myCollision.left = xDir == -1;
                    myCollision.right = xDir == 1;
                }
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float yDir = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + rayOffset + 0.05F;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector3 rayOrigin = (yDir == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector3.right * (vRaySpread * i + velocity.x);
            RaycastHit hit;

            Debug.DrawRay(rayOrigin, Vector3.up * yDir * rayLength, Color.red);

            if (Physics.Raycast(rayOrigin, Vector3.up * yDir, out hit, rayLength, collidedLayer))
            {
                velocity.y = (hit.distance - rayOffset) * yDir;
                rayLength = hit.distance;

                if (myCollision.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(myCollision.slopeAngleNew * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                myCollision.below = yDir == -1;
                myCollision.above = yDir == 1;
            }
        }

        if (myCollision.climbingSlope)
        {
            float xDir = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + rayOffset;
            Vector3 rayOrigin = ((xDir == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector3.up * velocity.y;
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, Vector3.right * xDir, out hit, rayLength, collidedLayer))
            {
                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                if (slopeAngle != myCollision.slopeAngleNew)
                {
                    velocity.x = (hit.distance - rayOffset) * xDir;
                    myCollision.slopeAngleNew = slopeAngle;
                }
            }
        }
    }

    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            myCollision.below = true;
            myCollision.climbingSlope = true;
            myCollision.slopeAngleNew = slopeAngle;
        }
    }

    void DescendSlope(ref Vector3 velocity)
    {
        float xDir = Mathf.Sign(velocity.x);
        Vector3 rayOrigin = (xDir == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, -Vector3.up, out hit, Mathf.Infinity, collidedLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == xDir)
                {
                    if (hit.distance - rayOffset <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        myCollision.slopeAngleNew = slopeAngle;
                        myCollision.descendingSlope = true;
                        myCollision.below = true;
                    }
                }
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = myCollider.bounds;
        bounds.Expand(rayOffset * -2);

        raycastOrigins.bottomLeft = new Vector3(bounds.min.x, bounds.min.y, 0);
        raycastOrigins.bottomRight = new Vector3(bounds.max.x, bounds.min.y, 0);
        raycastOrigins.topLeft = new Vector3(bounds.min.x, bounds.max.y, 0);
        raycastOrigins.topRight = new Vector3(bounds.max.x, bounds.max.y, 0);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = myCollider.bounds;
        bounds.Expand(rayOffset * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        hRaySpread = bounds.size.y / (horizontalRayCount - 1);
        vRaySpread = bounds.size.x / (verticalRayCount - 1);
    }

}
