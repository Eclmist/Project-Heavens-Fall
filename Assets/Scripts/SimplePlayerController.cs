using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimplePlayerController : MonoBehaviour
{
    public float acceleration = 10;
    public float maxSpeed = 1;
    public float horizontalDrag = 1;
    public float jumpForce = 10;
    public float terminalVelocity = 2;
    [Range(0,90)] public float slopeLimitForJump = 70;

    public bool slightlyComplicatedGroundCheck = false;

    private Rigidbody rb;
    private float timeSinceLastJump = 0;
    private float jumpCooldown = 0.2f;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	void FixedUpdate ()
	{
	    if (Input.GetAxis("Jump") > 0.01F && timeSinceLastJump > jumpCooldown && IsGrounded())
	    {
	        Jump();
	        timeSinceLastJump = 0;
	    }
	    else
	    {
	        timeSinceLastJump += Time.fixedDeltaTime;
	    }

	    Move();

	    ClampVelocity();
	}

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontal) >= 0.01F)
            rb.AddForce(new Vector3(horizontal*acceleration, 0, 0));
        else
        {
            if (IsGrounded())
                DecelerateHorizontalVelocity();
        }
    }

    void Jump()
    {
        Vector3 velocity = rb.velocity;
        velocity.y = jumpForce;
        rb.velocity = velocity;
    }

    void ClampVelocity()
    {
        Vector3 velocity = rb.velocity;
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        velocity.y = Mathf.Clamp(velocity.y, -terminalVelocity, terminalVelocity);
        rb.velocity = velocity;
    }

    void DecelerateHorizontalVelocity()
    {
        Vector3 velocity = rb.velocity;
        float clampMagnitude = Mathf.Abs(velocity.x / horizontalDrag);
        velocity.x = Mathf.Clamp(velocity.x, -clampMagnitude, clampMagnitude);
        rb.velocity = velocity;
    }

    bool IsGrounded()
    {
        // Raycast Method
        if (!slightlyComplicatedGroundCheck)
        {
            Debug.DrawLine(transform.position, transform.position - transform.up*0.5f);

            RaycastHit hit;

            if (Physics.Linecast(transform.position, transform.position - transform.up*0.5f, out hit))
            {

                if (Vector3.Dot(hit.normal, Vector3.up) > slopeLimitForJump / 90)
                {
                    return true;
                }

                return false;
            }

            return false;

        }
        else
        {
            return false;
        }
    }
}
