using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UsefulStuff
{
    public class HitSomethingDie : MonoBehaviour
    {

        public LayerMask hitWhatDie;

        private SphereCollider sphCollider;
        private BoxCollider boxCollider;
        private CapsuleCollider capsuleCollider;

        private bool hasRigidbody;

        // Use this for initialization
        void Start()
        {
            hasRigidbody = GetComponent<Rigidbody>();

            sphCollider = GetComponent<SphereCollider>();
            boxCollider = GetComponent<BoxCollider>();
            capsuleCollider = GetComponent<CapsuleCollider>();

        }

        //If theres no rigidbody
        void Update()
        {
            if (hasRigidbody) return;

            if (sphCollider != null && Physics.CheckSphere(transform.position + sphCollider.center, sphCollider.radius, hitWhatDie))
                Destroy(gameObject);
            if (boxCollider != null && Physics.CheckBox(boxCollider.center, boxCollider.size/2, transform.rotation, hitWhatDie))
                Destroy(gameObject);

            if (capsuleCollider != null && capsuleCollider.height <= 0.1F && Physics.CheckSphere(transform.position + capsuleCollider.center, capsuleCollider.radius, hitWhatDie))
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
