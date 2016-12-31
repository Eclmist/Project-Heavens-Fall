using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    public static CameraBehaviour Instance;

    public float CameraSpeed = 6;
    public float yUpperBounds = 18; 
    public float yLowerBounds = -5;

    public GameObject target;

    float cameraZoffset;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        cameraZoffset = transform.position.z;
    }


    // Update is called once per frame
    void LateUpdate () {

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            Vector3 targetPos = new Vector3(target.transform.position.x, Mathf.Clamp(target.transform.position.y, yLowerBounds, yUpperBounds), cameraZoffset);
            transform.position = Vector3.Lerp(transform.position, targetPos, CameraSpeed * Time.deltaTime);
        }
	}

    void setCameraMaxMin(float max, float min)
    {
        //Function to allow different height bounds for different levels
        yUpperBounds = max;
        yLowerBounds = min;
    }
}
