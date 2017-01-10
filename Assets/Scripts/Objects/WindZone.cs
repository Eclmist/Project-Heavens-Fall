using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WindZone : MonoBehaviour {

    public ParticleSystem windParticle;

    public float windForce;
    public float radius = 2.0f;

#pragma warning disable 108,114
    private SphereCollider collider;
#pragma warning restore 108,114

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

    void OnDrawGizmos()
    {
        //Debug.DrawLine(transform.position-transform.up, transform.position+transform.up, Color.white);
        //Debug.DrawLine(transform.position-transform.up + transform.forward * 3, transform.position-transform.up* 2 + transform.forward * 3, Color.white);
        //Debug.DrawLine(transform.position+transform.up + transform.forward * 3, transform.position+transform.up* 2 + transform.forward * 3, Color.white);
        //Debug.DrawLine(transform.position+transform.forward*6, transform.position+transform.up* 2 + transform.forward * 3, Color.white);
        //Debug.DrawLine(transform.position+transform.forward*6, transform.position-transform.up* 2 + transform.forward * 3, Color.white);
        //Debug.DrawLine(transform.position+transform.up, transform.position+transform.up + transform.forward * 3, Color.white);
        ////Debug.DrawLine(transform.position-transform.up + transform.forward * 3, transform.position+transform.up + transform.forward * 3, Color.white);
        //Debug.DrawLine(transform.position-transform.up, transform.position-transform.up + transform.forward * 3, Color.white);

        //Debug.DrawLine(transform.position - transform.right, transform.position + transform.right, Color.white);
        //Debug.DrawLine(transform.position - transform.right + transform.forward * 3, transform.position - transform.right * 2 + transform.forward * 3, Color.white);
        //Debug.DrawLine(transform.position + transform.right + transform.forward * 3, transform.position + transform.right * 2 + transform.forward * 3, Color.white);
        //Debug.DrawLine(transform.position + transform.forward * 6, transform.position + transform.right * 2 + transform.forward * 3, Color.white);
        //Debug.DrawLine(transform.position + transform.forward * 6, transform.position - transform.right * 2 + transform.forward * 3, Color.white);
        //Debug.DrawLine(transform.position + transform.right, transform.position + transform.right + transform.forward * 3, Color.white);
        ////Debug.DrawLine(transform.position-transform.right + transform.forward * 3, transform.position+transform.right + transform.forward * 3, Color.white);
        //Debug.DrawLine(transform.position - transform.right, transform.position - transform.right + transform.forward * 3, Color.white);

        Helper.DrawDebugArrow(transform.position, transform.position+transform.forward*6,1);

        if (collider == null)
        {
            collider = GetComponent<SphereCollider>();
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, collider.radius*transform.localScale.x);
    }
}
