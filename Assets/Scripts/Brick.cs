using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrickBreaker.Resources;

namespace BrickBreaker.Core
{
    public class Brick : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField]
        BrickData brickData;
        [Header("UIContent")]
        [SerializeField]
        SpriteRenderer spriteRenderer;


        public BrickData BrickData
        {
            get
            {
                return brickData;
            }
            set
            {
                brickData = value;
            }
        }

        int smashTime;
        private void OnEnable()
        {
            spriteRenderer.sprite = brickData.sprite;
            smashTime = brickData.smashTime;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Ball")
            {
                if(brickData.brickType== BrickType.Understory) return;
                if(smashTime>0)
                {
                    smashTime--;
                }
                else{
                    //TODO Check Type and Destroy
                }
            }
        }
    }
}

