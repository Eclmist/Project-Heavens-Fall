using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnImpact : MonoBehaviour {

    public LayerMask hitWhatDie;

    private new CapsuleCollider collider;

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
