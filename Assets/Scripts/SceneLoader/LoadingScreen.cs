using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

    public GameObject spinner;
    public Text PressKeyText;

    public static Vector3 rotation;
    public static float progress;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (progress < 0.9F)
        {
            if (!spinner.activeSelf)
                spinner.SetActive(true);

            transform.eulerAngles = rotation;


            //Instant set text to cannot see
            PressKeyText.CrossFadeAlpha(0, 0, true);

        }
        else
        {
            spinner.SetActive(false);
            PressKeyText.CrossFadeAlpha(1, 0.5F, true);
        }
	}
}
