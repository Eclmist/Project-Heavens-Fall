using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(Light))]
[RequireComponent (typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RTLights : MonoBehaviour
{

    [Range(0, 360)] public float angle;
    [Range(0, 5)] public float resolution;
    [Range(0, 100)] public int edgeResolveResolution;
    [Range(0, 10)] public float distanceBias = 1;

    public LayerMask includeLayers;

    /* Inherited Properties */
    private Light light;
    [ReadOnly] public float radius;
    /* -------------------- */

    private MeshFilter lightMeshFilter;
    private Mesh lightMesh;

    private List<Vector3> vertexList = new List<Vector3>();

    void Start ()
    {
        lightMesh = new Mesh();
        lightMesh.name = "Light Mesh";
        lightMeshFilter = GetComponent<MeshFilter>();
        lightMeshFilter.mesh = lightMesh;
        light = GetComponent<Light>();
    }
	
	void Update ()
	{
        radius = light.range;
	}

    void LateUpdate()
    {
        DrawLightMesh();
    }

    void DrawLightMesh()
    {
        int rayCount = Mathf.RoundToInt(angle*resolution);
        float stepAngleSize = angle/rayCount;
        List<Vector3> lightPoints = new List<Vector3>();

        for (int i = 0; i <= rayCount; i++)
        {
            float currentStepAngle = transform.eulerAngles.y - angle/2 + stepAngleSize*i;
            RTInfo currentRtInfo = RayTrace(- angle/2 +stepAngleSize * i);
            lightPoints.Add(currentRtInfo.point);
        }

        int vertexCount = lightPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        Vector2[] uv = new Vector2[vertexCount];

        int[] triangles = new int[(vertexCount - 2)*3];

        vertices[0] = Vector3.zero;
        uv[0] = new Vector2(0.5F, 0.5F);

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i+1] = transform.InverseTransformPoint(lightPoints[i]);
            uv[i + 1] = new Vector2(vertices[i + 1].x / radius, vertices[i + 1].y / radius) + new Vector2(0.5F, 0.5F);

            if (i < vertexCount - 2)
            {
                triangles[i*3] = 0;
                triangles[i*3 + 1] = i + 2;
                triangles[i*3 + 2] = i + 1;
            }
        }

        lightMesh.Clear();
        lightMesh.vertices = vertices;
        lightMesh.uv = uv;
        lightMesh.triangles = triangles;
        lightMesh.RecalculateNormals();
    }

    public Vector3 DirFromAngle(float angle, bool global)
    {
        if (!global)
            angle += transform.eulerAngles.y;

        return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
    }

    RTInfo RayTrace(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, false);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, radius, includeLayers))
        {
            return new RTInfo(true, hit.point, hit.distance, globalAngle);
        }

        return new RTInfo(false, transform.position + dir * radius, radius, globalAngle);
    }

    public struct RTInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public RTInfo(bool h, Vector3 p, float d, float a)
        {
            hit = h;
            point = p;
            distance = d;
            angle = a;
        }
    }
}
