using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WindZone : MonoBehaviour {

    public ParticleSystem windParticle;

    public float windForce;
    public float radius = 2.0f;

    private SphereCollider collider;

	// Use this for initialization
	void Start ()
    {

        ParticleSystem.ShapeModule windShape = windParticle.shape;
        windShape.radius = radius;
        ParticleSystem.EmissionModule windEmission = windParticle.emission;
        windEmission.rateOverTime = radius * 50.0f;

	    collider = GetComponent<SphereCollider>();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    collider.radius = radius;
	}

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerControllerA>().addForce(transform.forward * windForce);
        }
    }
}
