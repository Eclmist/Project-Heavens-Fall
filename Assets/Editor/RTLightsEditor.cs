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

        Vector3 viewAngleA = light.DirFromAngle(-light.angle/ 2, false);
        Vector3 viewAngleB = light.DirFromAngle(light.angle / 2, false);

        Handles.DrawLine(light.transform.position,
            light.transform.position + viewAngleA * light.radius);
        Handles.DrawLine(light.transform.position,
            light.transform.position + viewAngleB * light.radius);

        Handles.color = Color.white;
        Handles.DrawWireArc(light.transform.position, Vector3.forward,
            viewAngleA, light.angle, light.radius);

    }
}
