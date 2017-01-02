using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    private bool isTriggered;

    private static GameObject[] wispToSpawn; 

    //TODO Remove override for debug spawning
    //public GameObject objectToTestSpawn;


    void OnTriggerEnter()
    {
        if (isTriggered) return;

        isTriggered = true;

        if (wispToSpawn == null)
        {
            //TODO Remove
            //if (objectToTestSpawn != null)
            //{
            //    wispToSpawn = new []{objectToTestSpawn};
            //}
            //else
            //END remove
            wispToSpawn = Resources.LoadAll<GameObject>("Wisp");
            print(wispToSpawn.Length);
        }

        transform.GetChild(0).gameObject.SetActive(true);

        StartCoroutine(WispSpawning());
    }

    IEnumerator WispSpawning()
    {
        int count = Random.Range(3, 5);

        for (int i = 0; i < count; i++)
        {
            Vector3 insideUnitSphere = Random.insideUnitSphere;
            Vector3 position = insideUnitSphere * 1.5f + transform.position;
            RaycastHit hit;
            Physics.Raycast(new Ray(position, Vector3.down), out hit);

            position = hit.point;

            //TODO: set Height for the spawned wisp
            
            GameObject obj = Instantiate(wispToSpawn[Random.Range(0,wispToSpawn.Length)], position + Vector3.up * Random.Range(0.5f,0.8f), Quaternion.identity);

            if (insideUnitSphere.x> 0) obj.transform.localScale.Scale(new Vector3(-1,1,1));

            yield return new WaitForSeconds(0.8734f);
        }
    }
}
