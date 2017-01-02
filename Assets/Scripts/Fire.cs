using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    private bool isTriggered;

    private static GameObject wispToSpawn; 

    //TODO Remove override for debug spawning
    public GameObject objectToTestSpawn;


    void OnTriggerEnter()
    {
        if (isTriggered) return;

        isTriggered = true;

        if (!wispToSpawn)
        {
            //TODO Remove
            if (objectToTestSpawn)
            {
                wispToSpawn = objectToTestSpawn;
            }
            else
            //END remove
            wispToSpawn = Resources.Load<GameObject>("Wisp");
        }

        transform.GetChild(0).gameObject.SetActive(true);

        StartCoroutine(WispSpawning());
    }

    IEnumerator WispSpawning()
    {
        int count = Random.Range(3, 5);

        for (int i = 0; i < count; i++)
        {
            Vector3 position = Random.insideUnitSphere * 1.5f + transform.position;
            RaycastHit hit;
            Physics.Raycast(new Ray(position, Vector3.down), out hit);

            position = hit.point;

            //TODO: set Height for the spawned wisp
            
            Instantiate(wispToSpawn, position + Vector3.up * Random.Range(0.5f,0.8f), Quaternion.identity);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
