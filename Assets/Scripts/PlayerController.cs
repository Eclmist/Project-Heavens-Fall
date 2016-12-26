using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [ReadOnly] public float horizontal;
    [ReadOnly] public float vertical;

    public float gravity = 0.7f;
    public float horizontalDamp = 0.7f;

    public float jumpScale = 1;
    public float horizontalSpeed = 1;

    private Rigidbody rb;
    private CharacterController cc;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
    }
    
    // Update is called once per frame
    void Update () {

        horizontal *= horizontalDamp;
        vertical += -gravity * Time.deltaTime;

        if (Input.GetKey(KeyCode.D)) horizontal += horizontalSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) horizontal += -horizontalSpeed * Time.deltaTime;

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && cc.isGrounded) vertical = jumpScale;

        vertical = Mathf.Clamp(vertical, -20, 999);

        cc.Move((horizontal*Vector3.right + vertical*Vector3.up));
;

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
}
