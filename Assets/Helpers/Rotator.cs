using UnityEngine;
using System.Collections;

namespace UsefulStuff
{
    public class Rotator : MonoBehaviour
    {

        public float x, y, z;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            transform.Rotate(x, y, z);
        }
    }
}