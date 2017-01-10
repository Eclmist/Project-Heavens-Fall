using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDesignHelper : MonoBehaviour
{
    private CameraBehaviour cam;
    private PlayerControllerA player;

	// Use this for initialization
	void Start () {
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnDrawGizmos()
    {
        if (Debug.isDebugBuild)
        {
            if (!cam || !player)
            {
                GameObject[] roots = SceneManager.GetActiveScene().GetRootGameObjects();
                foreach (var root in roots)
                {
                    if (cam == null) cam = root.GetComponentInChildren<CameraBehaviour>();
                    if (player == null) player = root.GetComponentInChildren<PlayerControllerA>();
                }
            }
            Debug.DrawLine(new Vector3(100, cam.yLowerBounds, 0), new Vector3(-100, cam.yLowerBounds, 0), Color.red);
            Debug.DrawLine(new Vector3(100, cam.yUpperBounds, 0), new Vector3(-100, cam.yUpperBounds, 0), Color.blue);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(player.transform.position, 1);
        }
    }
}
