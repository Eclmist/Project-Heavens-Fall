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

    public bool debug = false;

    /* Inherited Properties */
    private Light light;
    public float radius;
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
        RTInfo oldRTInfo = new RTInfo();

        for (int i = 0; i <= rayCount; i++)
        {
            float currentStepAngle = transform.eulerAngles.z - angle/2 + stepAngleSize*i;
            RTInfo currentRtInfo = RayTrace(- angle/2 +stepAngleSize * i);

            if (i > 0)
            {
                bool edgeThresholdExceeded =
                    Mathf.Abs(oldRTInfo.distance - currentRtInfo.distance) > distanceBias;

                if (oldRTInfo.hit != currentRtInfo.hit 
                    || (oldRTInfo.hit && edgeThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldRTInfo, currentRtInfo);

                    if (edge.pointA != Vector3.zero)
                    {
                        lightPoints.Add(edge.pointA);
                    }

                    if (edge.pointB != Vector3.zero)
                    {
                        lightPoints.Add(edge.pointB);
                    }
                }
            }

            lightPoints.Add(currentRtInfo.point);
            oldRTInfo = currentRtInfo;
        }

        int vertexCount = lightPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        Vector2[] uv = new Vector2[vertexCount];

        int[] triangles = new int[(vertexCount - 2)*3];

        vertices[0] = Vector3.zero;
        uv[0] = new Vector2(0.5F, 0.5F);

        for (int i = 0; i < vertexCount - 1; i++)
        {
            if (debug)
            Debug.DrawLine(transform.position, lightPoints[i]);

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

    EdgeInfo FindEdge(RTInfo minRTInfo,RTInfo maxRTInfo)
    {
        float minAngle = minRTInfo.angle;
        float maxAngle = maxRTInfo.angle;

        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveResolution; i++)
        {
            float angle = (minAngle + maxAngle)/2;
            RTInfo newRTInfo = RayTrace(angle);

            bool edgeThresholdExceeded =
                Mathf.Abs(minRTInfo.distance - newRTInfo.distance) > distanceBias;

            if (newRTInfo.hit == minRTInfo.hit && !edgeThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newRTInfo.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newRTInfo.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    public Vector3 DirFromAngle(float angle, bool global)
    {
        if (!global)
            angle += transform.eulerAngles.z;

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

    public struct EdgeInfo
    {

        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 a, Vector3 b)
        {
            pointA = a;
            pointB = b;
        }
    }
}
