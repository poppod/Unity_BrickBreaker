using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrickBreaker.Manager;
using System;
namespace BrickBreaker.Core
{
    public class Ball : MonoBehaviour
    {
        [SerializeField]
        SpriteRenderer spriteRenderer;
        [SerializeField]
        Sprite defaultBall;
        [SerializeField]
        Sprite fireBall;

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
        private void OnEnable() {
            SwitchToDefault();
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
                GameManager.Instance.OnBallDestroy(this);
                // GameManager.Instance.BallManager.ReleaseBall(this.gameObject);
                //gameObject.SetActive(false);
            }
        }
        public void SwitchToFireBall(float countTime)
        {
            spriteRenderer.sprite = fireBall;
            StartCoroutine(DelayToAction(countTime, () =>
            {
                SwitchToDefault();
            }));
        }
        void SwitchToDefault()
        {
            spriteRenderer.sprite = defaultBall;
        }
        IEnumerator DelayToAction(float time, Action action = null)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }

    }

}
