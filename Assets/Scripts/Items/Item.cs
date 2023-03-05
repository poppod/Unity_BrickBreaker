using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrickBreaker.Manager;
namespace BrickBreaker.Core
{
    public class Item : MonoBehaviour
    {
        public int timeCount;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (transform.localPosition.y <= -5)
            {
                Destroy(gameObject);
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                GameManager.Instance.OnCollectedItem(this);
                return;
            }
            if (other.tag == "Destroy")
            {
                Destroy(gameObject);
            }
        }
    }







}

