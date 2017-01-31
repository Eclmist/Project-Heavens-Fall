using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WindZone : MonoBehaviour {

    public ParticleSystem windParticle;

    public float windForce;
    [Header("If Sphere collider")]
    public float radius = 2.0f;

    [Header("If box collider")]
    public float width;
    public float height;

#pragma warning disable 108,114
    private Collider collider;
#pragma warning restore 108,114

	// Use this for initialization
	void Start ()
    {

        ParticleSystem.ShapeModule windShape = windParticle.shape;
        windShape.radius = radius;
        ParticleSystem.EmissionModule windEmission = windParticle.emission;
        windEmission.rateOverTime = radius * 50.0f;

	    collider = GetComponent<Collider>();
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (collider is SphereCollider) (collider as SphereCollider).radius = radius;
        else if (collider is BoxCollider) (collider as BoxCollider).size = new Vector3(width, 2, height);
        else Assert.HardAssert(false, "Wind zone collider is not sphere or box collider ");
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
            collider = GetComponent<Collider>();
        }

        Gizmos.color = Color.green;
        if (collider is SphereCollider) Gizmos.DrawWireSphere(transform.position, (collider as SphereCollider).radius*transform.localScale.x);
        else if (collider is BoxCollider)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube((collider as BoxCollider).center, (collider as BoxCollider).size);
            Gizmos.matrix = Matrix4x4.identity;
            
        }
        else
        {
            Helper.DrawDebugArrow(transform.position, transform.position + transform.forward * 6, 1, Color.red);
        }
    }
}
