using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerV2 : MonoBehaviour
{

    [ReadOnly] public float horizontal;
    [ReadOnly] public float vertical;

    public float gravity = 0.5f;
    public float airHorizontalDamping = 0.995f;
    public float groundHorizontalDamping = 0.95f;

    public float jumpScale = 0.25f;
    public float horizontalSpeed = 0.8f;
    public float maxSpeed = 0.2f;

    private CharacterController cc;

	// Use this for initialization
	void Start ()
	{
	    cc = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Clamp Z
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        //Update Values
        horizontal = cc.velocity.x / 80;
        vertical = cc.velocity.y / 80;

        //Grounded Check
        bool isGrounded = Physics.CheckSphere(transform.position - Vector3.up * 0.1f, cc.radius * 0.98f, LayerMask.GetMask("Default"));

        //if (isGrounded) vertical = -99.0f;

        //Inputs
        bool movement = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A);

        //Horizontal Damps
        if (movement) horizontal = Mathf.Clamp(horizontal, -maxSpeed, maxSpeed);
        else if (isGrounded) horizontal *= groundHorizontalDamping;
        else horizontal *= airHorizontalDamping;

        //Horizontal Movement
        if (Input.GetKey(KeyCode.D)) horizontal += horizontalSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) horizontal += -horizontalSpeed * Time.deltaTime;
        
        //Vertical Movement
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded) vertical = jumpScale;

        //Vertical Checks
        vertical += -gravity * Time.deltaTime;

        cc.Move((horizontal*Vector3.right + vertical*Vector3.up)*80*Time.deltaTime);
    }
}
