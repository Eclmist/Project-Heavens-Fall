using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    private AsyncOperation ao;
    private bool coroutineStarted = false;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Load(int index)
    {
        // Load the loading screen scene
        SceneManager.LoadSceneAsync(1);

        if (!coroutineStarted)
            StartCoroutine(LoadLevelAsync(index));
    }

    IEnumerator LoadLevelAsync(int index)
    {
        coroutineStarted = true;

        ao = SceneManager.LoadSceneAsync(index);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            LoadingScreen.progress = ao.progress;

            if (ao.progress >= 0.9F)
            {
                if (Input.anyKeyDown)
                {
                    ao.allowSceneActivation = true;
                }
            }

            yield return null;
        }

        coroutineStarted = false;
    }

}
