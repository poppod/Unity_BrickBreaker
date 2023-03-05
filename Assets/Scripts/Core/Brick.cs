using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrickBreaker.Resources;
using BrickBreaker.Manager;
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

        public int[,] IndexPos = new int[1, 1];


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
               
                if (brickData.brickType == BrickType.Understory)
                {
                    GameManager.Instance.OnBrickDestroy(this);
                    return;
                }
                smashTime--;
                if (smashTime <= 0)
                {
                    //TODO Check Type and Destroy Boom Efx
                    if (brickData.brickType == BrickType.Bomb)
                    {
                        GameManager.Instance.BrickGenerator.BombBrickEffect(this);
                        GameManager.Instance.OnBrickDestroy(this);
                        //Debug.Log("Bomb");
                        return;
                    }
                    //gameObject.SetActive(false);
                    GameManager.Instance.OnBrickDestroy(this);
                    //DestroyImmediate(gameObject);//TODO add to pool
                }
                else
                {

                }
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Ball")
            {
                // if (brickData.brickType == BrickType.Understory) return;
                smashTime = 0;
                if (brickData.brickType == BrickType.Bomb)
                {
                    GameManager.Instance.BrickGenerator.BombBrickEffect(this);
                    GameManager.Instance.OnBrickDestroy(this);
                    //Debug.Log("Bomb");
                    return;
                }
                //gameObject.SetActive(false);
                GameManager.Instance.OnBrickDestroy(this);
                //DestroyImmediate(gameObject);//TODO add to pool
            }
        }
    }
}

