using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(Light))]
public class RTLights : MonoBehaviour
{

    [Range(0, 360)] public float angle;
    [Range(0, 100)] public int resolution;
    [Range(0, 100)] public int edgeResolveResolution;
    [Range(0, 10)] public float distanceBias = 1;

    public LayerMask shadowLayer;

    /* Inherited Properties */
    private Light light;
    public float radius;
    /* -------------------- */

    private MeshFilter lightMeshFilter;
    private Mesh lightMesh;

    private List<Vector3> vertexList = new List<Vector3>();

    void Start ()
    {
        light = GetComponent<Light>();
    }
	
	void Update ()
	{
        radius = light.range;
	}

    public Vector3 DirFromAngle(float angle)
    {
        return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
    }
}
