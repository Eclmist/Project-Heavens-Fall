using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour {

    public ParticleSystem windParticle;

    public Vector3 windForce = new Vector3(0.6f, 0, 0);
    public float windSize = 2.0f;
    public Vector3 windDir = new Vector3(1, 0, 0); 

	// Use this for initialization
	void Start () {


        //ParticleSystem.ShapeModule windShape = windParticle.shape;
        //windShape.radius = windSize;
        //ParticleSystem.EmissionModule windEmission = windParticle.emission;
        //windEmission.rateOverTime = windSize * 50.0f;
        
        transform.LookAt(windDir);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerControllerA>().addForce(windForce);
        }
    }
}
