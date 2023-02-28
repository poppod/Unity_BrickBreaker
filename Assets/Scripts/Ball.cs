using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrickBreaker.Core
{
    public class Ball : MonoBehaviour
    {
        [SerializeField]
        float force = 200;
        [SerializeField]
        float maxVelocity = 20f;


        Rigidbody2D rb;
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(rb.velocity.magnitude);

        }
        private void FixedUpdate()
        {
            if (rb.velocity.magnitude > maxVelocity)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Destroy")
            {
                gameObject.SetActive(false);
            }
        }
    }

}
