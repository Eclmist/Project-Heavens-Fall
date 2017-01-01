using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isActivated = false;
    
    void OnTriggerEnter()
    {
        if (isActivated) return;
        LevelManager.currentLevelManager.ReachCheckpoint(transform);
        isActivated = true;
    }
}
