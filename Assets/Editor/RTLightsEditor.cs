using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RTLights))]
public class RTLightsEditor : Editor
{
    void OnSceneGUI()
    {
        RTLights light = (RTLights)target;
        Vector3 cameraPos = Camera.main.transform.position;

        Handles.color = Color.white;
        Handles.DrawWireArc(light.transform.position, Vector3.forward,
            Vector3.right, light.angle, light.radius);


    }
}
