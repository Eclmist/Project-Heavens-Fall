using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour {

    [Range(0,1)]
    public float targetBrightness = 1;

    float scriptStartTime;

    RenderImage brightnessControl;

    // Use this for initialization
    void Start () {
        scriptStartTime = Time.time;
        brightnessControl = gameObject.GetComponent<RenderImage>();
    }

    // Update is called once per frame
    void Update () {
        if (Time.time - scriptStartTime < 1)
        {
            brightnessControl.setBrightness(Mathf.Lerp(brightnessControl.getBrightness(),
                targetBrightness, Time.time - scriptStartTime));
        }
        else
        {
            brightnessControl.setBrightness(targetBrightness);
                
        }
    }
}
