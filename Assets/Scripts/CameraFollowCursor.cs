using UnityEngine;
using System.Collections;

public class CameraFollowCursor : MonoBehaviour
{
    [Range(0, 2)] public float cameraShakeMagnitude = 1;
    [Range(0, 2)] public float cameraShakeFrequency = 1;
    [Range(0, 2)] public float cameraShakeSpeed = 1;

    Vector2 startingPosition;
    Vector2 centerPoint;
    Vector2 targetPoint;

    float tParam = 2;
    float scriptStartTime;

    void Start()
    {
        startingPosition = transform.position;
        centerPoint = startingPosition;

        gameObject.GetComponent<RenderImage>().setBrightness(0);
        scriptStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - scriptStartTime < 1)
        {
            gameObject.GetComponent<RenderImage>().setBrightness(Mathf.Lerp(0, 0.88F, Time.time - scriptStartTime));
        }

        transform.position = new Vector3(centerPoint.x + (Input.mousePosition.x - centerPoint.x) * 0.001F,
                                          centerPoint.y + (Input.mousePosition.y - centerPoint.y) * 0.001F,
                                          transform.position.z);

        if (tParam < 1 / cameraShakeFrequency)
        {
            tParam += Time.deltaTime;
            centerPoint = Vector2.Lerp(centerPoint, targetPoint, Time.deltaTime * cameraShakeSpeed);
        }
        else
        {
            targetPoint = startingPosition + Random.insideUnitCircle.normalized * cameraShakeMagnitude;
            tParam = 0;
        }
    }
}