using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public static LevelManager currentLevelManager;
    
    private Transform currentCheckpoint;

    [SerializeField] private string[] levelsToUnlock;

    void Awake()
    {
        currentLevelManager = this;
    }
    
	void Start () {

	}
	
	void Update () {
        
    }

    void OnDestroy()
    {
        currentLevelManager = null;
    }

    public void ReachCheckpoint(Transform newCheckpoint)
    {
        Player.SetSpawnLocation(newCheckpoint.position);

        currentCheckpoint = newCheckpoint;
    }

    public void ClearLevel()
    {
        ProgressionManager.CurrentProfile.SetLevelCleared(SceneManager.GetActiveScene().name);

        foreach (var levelToUnlock in levelsToUnlock)
        {
            ProgressionManager.CurrentProfile.SetLevelUnlocked(levelToUnlock);
        }

        Assert.HardAssert(levelsToUnlock.Length>0,"There should be at least one level to unlock");
        //TODO make samuel fix this
        FindObjectOfType<LoadScene>().Load(SceneManager.GetSceneByName(levelsToUnlock[0]).buildIndex);
    }

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.green;
        if (currentCheckpoint != null) Gizmos.DrawWireSphere(currentCheckpoint.position, 2);
    }
#endif

}
