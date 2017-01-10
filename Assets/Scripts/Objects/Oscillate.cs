using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour
{

    public Vector3 oscillationAmount;
    public float oscillationTime;

    //TODO: SAMUEL! FIX unused variable 
    //private Vector3 startPos;
    private bool atTarget;

	void Start ()
	{
	    //startPos = transform.position;
	    gameObject.MoveTo(transform.position + oscillationAmount, oscillationTime, 0, EaseType.easeInOutSine, LoopType.pingPong);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
