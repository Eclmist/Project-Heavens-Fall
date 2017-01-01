using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsefulStuff
{
    public class Move : MonoBehaviour
    {
        public bool localSpace;
        public Vector3 velocity;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (localSpace) transform.Translate(velocity*Time.deltaTime);
            else transform.position += velocity*Time.deltaTime;
        }
    }
}
