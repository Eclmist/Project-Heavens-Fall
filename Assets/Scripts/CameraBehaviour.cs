using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    public static CameraBehaviour Instance;

    public float cameraSpeed = 6;
    public float yUpperBounds = 18; 
    public float yLowerBounds = -3;

    public GameObject target;

    float cameraZoffset;
    private Vector3 centerPos;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        centerPos = transform.position;
        cameraZoffset = transform.position.z;
    }


    // Update is called once per frame
    void LateUpdate ()
    {


        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            centerPos = new Vector3(target.transform.position.x, Mathf.Clamp(target.transform.position.y, yLowerBounds, yUpperBounds), cameraZoffset);
        }

        Vector3 targetPos = new Vector3(centerPos.x + (Input.mousePosition.x - centerPos.x) * 0.001F,
                                          centerPos.y + (Input.mousePosition.y - centerPos.y) * 0.001F,
                                          transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime*cameraSpeed);

    }

    void setCameraMaxMin(float max, float min)
    {
        //Function to allow different height bounds for different levels
        yUpperBounds = max;
        yLowerBounds = min;
    }
}
