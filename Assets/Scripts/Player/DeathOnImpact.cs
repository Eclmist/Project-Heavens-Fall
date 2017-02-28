using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnImpact : MonoBehaviour {

    public LayerMask hitWhatDieLayer;

#pragma warning disable 108, 114
    private BoxCollider collider;
#pragma warning restore 108, 114

    void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (Physics.CheckSphere(transform.position + collider.center, 0.77f, hitWhatDieLayer))
        {
            Player.KillPlayer();
        }
    }

}
