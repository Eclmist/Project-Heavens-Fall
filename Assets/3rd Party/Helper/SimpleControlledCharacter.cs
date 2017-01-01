using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UsefulStuff
{
    public class SimpleControlledCharacter : MonoBehaviour
    {
        public float speed;

        [Header("Custom controls")] private bool useCustomKeycodes;

        public KeyCode left = KeyCode.A;
        public KeyCode right = KeyCode.D;
        public KeyCode up = KeyCode.W;
        public KeyCode down = KeyCode.S;

        private PlayerControllerA controller;
        private DeathOnImpact death;

        private bool ghosting;

        // Use this for initialization
        void Start()
        {
            controller = GetComponent<PlayerControllerA>();
            death = GetComponent<DeathOnImpact>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                ghosting = !ghosting;
                controller.enabled = !ghosting;
                death.enabled = !ghosting;
            }

            if (!ghosting) return;

            float tempSpeed = speed;
            if (Input.GetKey(KeyCode.LeftShift)) tempSpeed *= 3;


            if (!useCustomKeycodes)
                transform.position += (Input.GetAxis("Horizontal")*Vector3.right +
                                       Input.GetAxis("Vertical")*Vector3.up)*
                                      Time.deltaTime* tempSpeed;
            else
            {
                Vector3 axis = Vector3.zero;
                if (Input.GetKey(left)) axis += Vector3.left;
                if (Input.GetKey(right)) axis += Vector3.right;
                if (Input.GetKey(up)) axis += Vector3.up;
                if (Input.GetKey(down)) axis += Vector3.down;
                transform.position += axis*Time.deltaTime* tempSpeed;
            }
        }
    }
}
