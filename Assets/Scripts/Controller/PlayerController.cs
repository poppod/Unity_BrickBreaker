using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using BrickBreaker.Manager;
using BrickBreaker.Core;
using System;

namespace BrickBreaker.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Mouse Mover")]
        [SerializeField]
        float clampX_min = -8.36f;
        [SerializeField]
        float clampX_max = 8.36f;
        [Header("Power Option")]
        [SerializeField]
        Vector3 startPosition = new Vector3(0, -3.20f, 0);
        [SerializeField]
        float ballForce = 30;
        [Header("Content")]
        [SerializeField]
        Transform magnetSpawnPos;



        //Option
        public bool IsStart = false;
        int fireBallItems = 0;
        int doubleBallItems = 0;
        int magnetItems = 0;
        float tempDoubleBallsTime = 0;
        float tempFireBallTime = 0;
        //
        Ball m_magnetBall;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void ResetOnStart()
        {
            transform.position = this.startPosition;
        }
        #region  Input
        public void OnMouseMover(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var _value = context.ReadValue<Vector2>();
                if (GameManager.Instance.IsPlaying)
                {
                    //TODO Run game play
                    var _pos = Camera.main.ScreenToWorldPoint(new Vector3(_value.x, _value.y, 0f));
                    if (_pos.x <= clampX_min || _pos.x >= clampX_max) return;
                    transform.position = new Vector3(_pos.x, transform.position.y, transform.position.z);
                    // Debug.Log(_pos.x);
                }
                else
                {

                }
            }


        }
        public void OnMouseClick(InputAction.CallbackContext context)
        {
            //Debug.Log("Click :" + magnetItems);
            if (magnetItems > 0)
            {
                if (m_magnetBall == null)
                {
                    m_magnetBall = magnetSpawnPos.GetComponentInChildren<Ball>();
                }
                var _rb = m_magnetBall.GetComponent<Rigidbody2D>();
                m_magnetBall.transform.SetParent(null);
                _rb.simulated = true;
                _rb.AddForce(new Vector2(0, ballForce), ForceMode2D.Impulse);
                magnetItems = 0;
            }
            else
            {
                if (m_magnetBall == null)
                {
                    var _temp = magnetSpawnPos.GetComponentsInChildren<Ball>();
                    foreach (var item in _temp)
                    {
                        GameManager.Instance.BallManager.ReleaseBall(item.gameObject);
                    }
                    magnetItems = 0;
                }
                // GameManager.Instance.BallManager.ReleaseBall(m_magnetBall.gameObject);
                // var _temp = magnetSpawnPos.GetComponentsInChildren<Ball>();
                // foreach (var item in _temp)
                // {
                //     GameManager.Instance.BallManager.ReleaseBall(item.gameObject);
                // }
                // magnetItems = 0;
            }
            //other.rigidbody.AddForce(new Vector2(0, ballForce), ForceMode2D.Impulse);
        }
        #endregion
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Ball")
            {
                GameManager.Instance.OnBallEnter();
                other.rigidbody.AddForce(new Vector2(0, ballForce), ForceMode2D.Impulse);
                if (fireBallItems > 0)
                {
                    //TODO switch ball to fire ball
                    GameManager.Instance.OnBallToFireball(other.gameObject.GetComponent<Ball>(), tempFireBallTime);
                    fireBallItems--;
                }
                if (doubleBallItems > 0)
                {
                    GameManager.Instance.BallManager.GetBall(other.gameObject.transform.position);
                    doubleBallItems--;
                }
                return;
            }

        }
        public void OnItemCollect(Item item)
        {
            if (item is Fireball)
            {
                tempFireBallTime = item.timeCount;
                fireBallItems++;
                //Destroy(item.gameObject);
                return;
            }
            if (item is PaddleResizerItems)
            {
                var _temp = item as PaddleResizerItems;
                PaddleResizer(_temp);
                //Destroy(item.gameObject);
                StartCoroutine(DelayToAction(_temp.timeCount, () =>
                {
                    transform.localScale = new Vector3(2, transform.localScale.y, transform.localScale.z);
                }));
                return;
            }
            if (item is DoubleBalls)
            {
                var _temp = item as DoubleBalls;
                this.tempDoubleBallsTime = _temp.timeCount;
                doubleBallItems++;
            }
            if (item is Magnet)
            {
                if(magnetItems>0) return;
                var _temp = item as Magnet;
                m_magnetBall = GameManager.Instance.BallManager.GetBallObject(magnetSpawnPos.position).GetComponent<Ball>();
                m_magnetBall.transform.SetParent(magnetSpawnPos);
                var _rb = m_magnetBall.GetComponent<Rigidbody2D>().simulated = false;
                magnetItems++;
                StartCoroutine(DelayToAction(_temp.timeCount, () =>
                {
                    if (magnetItems > 0)
                    {
                        GameManager.Instance.BallManager.ReleaseBall(m_magnetBall.gameObject);
                        var _temp = magnetSpawnPos.GetComponentsInChildren<Ball>();
                        foreach (var item in _temp)
                        {
                            GameManager.Instance.BallManager.ReleaseBall(item.gameObject);
                        }
                        magnetItems = 0;
                    }
                }));

            }

            Destroy(item.gameObject);


        }


        IEnumerator DelayToAction(float time, Action action = null)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }

        void PaddleResizer(PaddleResizerItems item)
        {
            var size = (item.resizeValue * transform.localScale.x) / 100;
            transform.localScale = new Vector3(transform.localScale.x + size, transform.localScale.y, transform.localScale.z);
        }

    }
}

