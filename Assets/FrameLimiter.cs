using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FrameLimiter : MonoBehaviour
{
	public int sleepMillis = 17;

	public static FrameLimiter instance;

	// Use this for initialization
	void Start ()
	{
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		//I know its disgusting but it has to be done
		Thread.Sleep(sleepMillis);
	}
}
