using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    private bool isTriggered;

    private static GameObject[] wispToSpawn; 

    private List<Vector3> fagBag = new List<Vector3>();
    private List<Vector3> fagBagBackup;

    //TODO Remove override for debug spawning
    //public GameObject objectToTestSpawn;

    void Start()
    {
        for (float i = -1; i < 1; i+= 0.2f)
        {
            fagBag.Add(new Vector3(i,0,0));
        }

        fagBagBackup = new List<Vector3>(fagBag);
    }

    Vector3 GetRandomPos()
    {
        if (fagBag.Count == 1)
        {
            Vector3 pos = fagBag[0];

            fagBag = new List<Vector3>(fagBagBackup);

            return pos;
        }

        int index = Random.Range(0, fagBag.Count);

        Vector3 position = fagBag[index];

        fagBag.RemoveAt(index);

        return position;
    }


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
        int count = Random.Range(4, 8);

        for (int i = 0; i < count; i++)
        {
            Vector3 insideUnitSphere = GetRandomPos();
            Vector3 position = insideUnitSphere * 1.5f + transform.position;
            position.z += Random.Range(-1f, 1f);
            RaycastHit hit;
            Physics.Raycast(new Ray(position, Vector3.down), out hit, 999, LayerMask.GetMask("Obstacles"));

            print(hit.collider.gameObject.name);

            position = hit.point;

            //TODO: set Height for the spawned wisp
            
            GameObject obj = Instantiate(wispToSpawn[Random.Range(0,wispToSpawn.Length)], position + Vector3.up * 0.4f, Quaternion.identity);

            if (insideUnitSphere.x > 0)
            {
                Vector3 localscale = obj.transform.GetChild(0).localScale;
                localscale.x *= -1;
                obj.transform.GetChild(0).localScale = localscale;
            }

            yield return new WaitForSeconds(0.8734f);
        }
    }
}
