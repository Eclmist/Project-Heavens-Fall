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

	    GameObject[] objs = SceneManager.GetActiveScene().GetRootGameObjects();


        foreach (var obj in objs)
	    {
            if (obj.name == "Player" || obj.name == "Main Camera" || obj.name == "LevelEndPoint" || obj.name == "Checkpoint")
	            obj.SetActive(true);
        }

        Destroy(gameObject);

        //Find("Player").SetActive(true);
        //Find("LevelEndPoint").SetActive(true);
        //Find("Main Camera").SetActive(true);
    }
}
