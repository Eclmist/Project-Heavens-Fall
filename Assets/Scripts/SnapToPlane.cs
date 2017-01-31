using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapToPlane : MonoBehaviour
{
    public bool unlockObject;
    public bool setLockValue;

    [ReadOnly] public float lockedValue;
        

#if UNITY_EDITOR
    // Update is called once per frame
    void Update ()
    {
        if (setLockValue)
        {
            lockedValue = transform.position.z;
            setLockValue = false;
        }

        if (unlockObject) { return; }

        transform.position= new Vector3(transform.position.x, transform.position.y, lockedValue);

    }
#endif
}
