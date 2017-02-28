using UnityEngine;
using System.Collections;

public class RaycastHelper : MonoBehaviour
{
    public const float rayOffset = .015f;
    public int horizontalRayCount = 6;
    public int verticalRayCount = 6;

    [HideInInspector]
    public float hRaySpread;
    [HideInInspector]
    public float vRaySpread;

    BoxCollider myCollider;

    public RaycastOrigins raycastOrigins;

    public virtual void Start()
    {
        myCollider = GetComponent<BoxCollider>();
        CalculateRaySpacing();
    }

    public void UpdateRaycastOrigins()
    {
        Bounds bounds = myCollider.bounds;
        bounds.Expand(rayOffset * -2);

        raycastOrigins.bottomLeft = new Vector3(bounds.min.x, bounds.min.y, 0);
        raycastOrigins.bottomRight = new Vector3(bounds.max.x, bounds.min.y, 0);
        raycastOrigins.topLeft = new Vector3(bounds.min.x, bounds.max.y, 0);
        raycastOrigins.topRight = new Vector3(bounds.max.x, bounds.max.y, 0);
    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = myCollider.bounds;
        bounds.Expand(rayOffset * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        hRaySpread = bounds.size.y / (horizontalRayCount - 1);
        vRaySpread = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins
    {
        public Vector3 topLeft, topRight;
        public Vector3 bottomLeft, bottomRight;
    }
}
