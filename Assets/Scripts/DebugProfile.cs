using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugProfile : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    if (ProgressionManager.CurrentProfile == null)
	    {
	        ProgressionManager.CreateNewProfile();
	    }
        Destroy(gameObject);
	}
}
