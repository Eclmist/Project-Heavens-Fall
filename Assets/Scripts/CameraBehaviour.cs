using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public float CameraSpeed = 6;
    public static float highestView = 18; 
    public static float lowestView = -5;

    GameObject playerObject;
    float cameraZoffset = -20;
    Vector3 endPos;
	
	// Update is called once per frame
	void Update () {
		
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            endPos = new Vector3(playerObject.transform.position.x, Mathf.Clamp(playerObject.transform.position.y, lowestView, highestView), cameraZoffset);
            transform.position = Vector3.Lerp(transform.position, endPos, CameraSpeed * Time.deltaTime);
        }
	}

    static void setCameraMaxMin(float max, float min)
    {
        //Function to allow different height bounds for different levels
        highestView = max;
        lowestView = min;
    }
}
