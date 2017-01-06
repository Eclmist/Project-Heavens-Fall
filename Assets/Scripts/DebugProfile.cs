using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugProfile : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    if (ProgressionManager.CurrentProfile == null)
	    {
	        ProgressionManager.CreateNewProfile();
            ProgressionManager.CurrentProfile.SetLevelUnlocked(SceneManager.GetActiveScene().name);
	    }
        Destroy(gameObject);
	}
}
