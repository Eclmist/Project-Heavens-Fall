using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UsefulStuff
{
    public class HitSomethingDie : MonoBehaviour
    {
        private SphereCollider sphCollider;
        private BoxCollider boxCollider;

        private bool hasRigidbody;

        // Use this for initialization
        void Start()
        {
            hasRigidbody = GetComponent<Rigidbody>();

            sphCollider = GetComponent<SphereCollider>();
            boxCollider = GetComponent<BoxCollider>();
        }

        //If theres no rigidbody
        void Update()
        {
            if (hasRigidbody) return;

            if (sphCollider != null && Physics.CheckSphere(transform.position + sphCollider.center, sphCollider.radius))
                Destroy(gameObject);
            if (boxCollider != null && Physics.CheckBox(boxCollider.center, boxCollider.size/2, transform.rotation))
                Destroy(gameObject);
        }


        //If Have Rigidbody
        void OnTriggerEnter()
        {
            Destroy(gameObject);
        }

        void OnCollisionEnter()
        {
            Destroy(gameObject);
        }
    }
}
