using UnityEngine;
using System.Collections;

public class PlayerControllerA : RaycastHelper
{
    public static PlayerControllerA Instance;

    public LayerMask collidedLayer;

    public float maxJump = 8;
    public float timeToReachMaxJump = .7f;
    float timeToReachMaxSpeed = .15f;
    float maxSpeed = 18;

    public GameObject flipObject;

    float gravity;
    float jumpVelocity;
    Vector3 vDirection;
    float sVelocity;
    Vector2 input;

    [HideInInspector]
    public bool jumping = false;

    float maxClimbAngle = 60;
    float maxDescendAngle = 80;

    public CollisionInfo myCollision;

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

    public override void Start()
    {
        base.Start();
        gravity = -(2 * maxJump) / Mathf.Pow(timeToReachMaxJump, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToReachMaxJump;
        CalculateRaySpacing();
    }

    void Update()
    {

        if (myCollision.above || myCollision.below)
        {
            vDirection.y = 0;
        }

        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && myCollision.below)
        {
            jumping = true;
        }

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


    }

    void FixedUpdate()
    {
        if (jumping)
        {
            vDirection.y = jumpVelocity;
            jumping = false;
        }

        float xVelocity = input.x * maxSpeed;
        vDirection.x = Mathf.SmoothDamp(vDirection.x, xVelocity, ref sVelocity, timeToReachMaxSpeed);
        vDirection.y += gravity * Time.deltaTime;
        vDirection.z = 0;
        Move(vDirection * Time.deltaTime);
    }

    public void addForce(Vector3 f)
    {
        myCollision.below = false;
        vDirection += f;
    }

    public void Move(Vector3 velocity, bool standingOnPlatform = false)
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

        if (standingOnPlatform)
        {
            myCollision.below = true;
        }
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + rayOffset;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector3 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector3.up * (hRaySpread * i);
            RaycastHit hit;

            Debug.DrawRay(rayOrigin, Vector3.right * directionX * rayLength, Color.red);

            if (Physics.Raycast(rayOrigin, Vector3.right * directionX, out hit, rayLength, collidedLayer))
            {

                if (hit.distance == 0)
                {
                    continue;
                }

                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    if (myCollision.descendingSlope)
                    {
                        myCollision.descendingSlope = false;
                        velocity = myCollision.velocityOld;
                    }
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != myCollision.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - rayOffset;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if (!myCollision.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    velocity.x = (hit.distance - rayOffset) * directionX;
                    rayLength = hit.distance;

                    if (myCollision.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(myCollision.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    myCollision.left = directionX == -1;
                    myCollision.right = directionX == 1;
                }
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + rayOffset;

        for (int i = 0; i < verticalRayCount; i++)
        {

            Vector3 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector3.right * (vRaySpread * i + velocity.x);
            RaycastHit hit;

            Debug.DrawRay(rayOrigin, Vector3.up * directionY * rayLength, Color.red);

            if (Physics.Raycast(rayOrigin, Vector3.up * directionY, out hit, rayLength, collidedLayer))
            {

                velocity.y = (hit.distance - rayOffset) * directionY;
                rayLength = hit.distance;

                if (myCollision.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(myCollision.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                myCollision.below = directionY == -1;
                myCollision.above = directionY == 1;
            }
        }

        if (myCollision.climbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + rayOffset;
            Vector3 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector3.up * velocity.y;
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, Vector3.right * directionX, out hit, rayLength, collidedLayer))
            {
                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                if (slopeAngle != myCollision.slopeAngle)
                {
                    velocity.x = (hit.distance - rayOffset) * directionX;
                    myCollision.slopeAngle = slopeAngle;
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
            myCollision.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector3 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, -Vector3.up, out hit, Mathf.Infinity, collidedLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - rayOffset <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        myCollision.slopeAngle = slopeAngle;
                        myCollision.descendingSlope = true;
                        myCollision.below = true;
                    }
                }
            }
        }
    }

}
