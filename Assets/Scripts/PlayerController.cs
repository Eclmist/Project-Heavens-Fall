using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [ReadOnly] public float horizontal;
    [ReadOnly] public float vertical;

    [ReadOnly] public bool wasGrounded = false;

    public float gravity = 0.5f;
    public float horizontalDamp = 0.95f;
    public float groundedHorizontalDamp = 0.7f;

    public float jumpScale = 0.25f;
    public float horizontalSpeed = 0.8f;


    private Rigidbody rb;
    private CharacterController cc;
    private SphereCollider collider;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
    }
    
    // Update is called once per frame
    void Update ()
    {
        //Grounded value
        bool isGrounded = Physics.CheckSphere(transform.position - Vector3.up*0.1f, cc.radius, LayerMask.GetMask("Default"));
        bool isRight = Physics.CheckSphere(transform.position - Vector3.left*0.1f, cc.radius, LayerMask.GetMask("Default"));
        bool isLeft = Physics.CheckSphere(transform.position - Vector3.right*0.1f, cc.radius, LayerMask.GetMask("Default"));

        //Inputs
        bool movement = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A);

        //Horizontal Damps
        if (movement) horizontal = Mathf.Clamp(horizontal,-0.3f, 0.3f);
        else if (isGrounded) horizontal *= groundedHorizontalDamp;
        else horizontal *= horizontalDamp;

        //Vertical Checks
        if (!isGrounded) vertical += -gravity*Time.deltaTime;
        
        if (isGrounded && !wasGrounded)
        {
            vertical = 0;
        }

        //Horizontal Movement
        if (Input.GetKey(KeyCode.D)) horizontal += (!isGrounded ? horizontalSpeed : horizontalSpeed /groundedHorizontalDamp * horizontalDamp) * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) horizontal += (!isGrounded ? -horizontalSpeed : -horizontalSpeed/groundedHorizontalDamp * horizontalDamp) * Time.deltaTime;
        
        //Vertical Movement
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded) vertical = jumpScale;
        
        //Horizontal Checks
        if (isRight) horizontal = Mathf.Clamp(horizontal, -999, 0);
        if (isLeft) horizontal = Mathf.Clamp(horizontal, 0, 999);

        vertical = Mathf.Clamp(vertical, -20, 999);

        cc.Move((horizontal*Vector3.right + vertical*Vector3.up));

        wasGrounded = isGrounded;

        return;

        //Blend to zero
        horizontal *= horizontalDamp;
        vertical += -9.8f * Time.deltaTime;

        //Jump
        if (Input.GetKeyDown(KeyCode.W)) vertical = jumpScale;

        if (Input.GetKey(KeyCode.D)) horizontal += 1*Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) horizontal += -1*Time.deltaTime;

        rb.velocity += (horizontal*Vector3.right + vertical*Vector3.up) * Time.deltaTime;
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position - Vector3.up * 0.1f, cc.radius);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position - Vector3.right * 0.1f, cc.radius);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position - Vector3.left * 0.1f, cc.radius);
    }
}
