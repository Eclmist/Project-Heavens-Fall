using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndPoint : MonoBehaviour {

    void OnTriggerEnter()
    {
        //Analytics Stuff
        AnalyticsManager.SaveData();

        LevelManager.currentLevelManager.ClearLevel();
    }
}
