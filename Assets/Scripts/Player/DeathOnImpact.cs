using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnImpact : MonoBehaviour {

    public LayerMask hitWhatDie;

#pragma warning disable 108,114
    private CapsuleCollider collider;
#pragma warning restore 108,114

    void Start()
    {
        collider = GetComponent<CapsuleCollider>();
    }

	void Update ()
    {
        if (Physics.CheckSphere(transform.position + collider.center, collider.radius, hitWhatDie))
        {
            Player.KillPlayer();
        }
    }
}
