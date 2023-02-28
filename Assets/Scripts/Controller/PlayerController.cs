using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        float ballForce = 30;



        //Option
        public bool IsStart = false;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        #region  Input
        public void OnMouseMover(InputAction.CallbackContext context)
        {
            var _value = context.ReadValue<Vector2>();
            if (IsStart)
            {
                //TODO Run game play
                var _pos = Camera.main.ScreenToWorldPoint(new Vector3(_value.x, _value.y, 0f));
                if (_pos.x <= clampX_min || _pos.x >= clampX_max) return;
                transform.position = new Vector3(_pos.x, transform.position.y, transform.position.z);
                // Debug.Log(_pos.x);
            }

        }
        #endregion
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Ball")
            {
                other.rigidbody.AddForce(new Vector2(0, ballForce), ForceMode2D.Impulse);
            }
        }
    }
}

