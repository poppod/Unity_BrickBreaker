using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BrickBreaker.Core;
using UnityEngine.InputSystem;
using BrickBreaker.Controller;
using System;
namespace BrickBreaker.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; set; }
        [Header("Initial Controller")]
        [SerializeField]
        PlayerController playerController;
        public PlayerController PlayerController
        {
            get
            {
                return this.playerController;
            }
        }
        [SerializeField]
        BrickGenerator brickGenerator;
        public BrickGenerator BrickGenerator
        {
            get
            {
                return this.brickGenerator;
            }
        }
        [SerializeField]
        BallManager ballManager;
        public BallManager BallManager
        {
            get
            {
                return this.ballManager;
            }
        }
        [SerializeField]
        ItemsManager itemsManager;
        [SerializeField]
        int playerLive = 3;
        [SerializeField]
        int scorePoint = 0;
        [SerializeField]
        Vector3 ballSpawnPosition = new Vector3(0, -1.25f, 0);

        [Header("UI")]
        [SerializeField]
        TMP_Text scoreUI;
        [SerializeField]
        TMP_Text liveUI;
        [SerializeField]
        TMP_Text messageUI;

        [Header("Items Effect")]
        [SerializeField]
        BoxCollider2D bottomWall;



        public bool IsPlaying { get; set; }


        private void Awake()
        {

        }
        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            IsPlaying = false;
            OnStart();
            GenerateBrick();

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnBallDestroy(Ball ball)
        {
            //TODO check  all ball If use Items

            ballManager.ReleaseBall(ball.gameObject);
            if (ballManager.CheckActiveBall())
            {
                return;
            }
            if (--playerLive <= 0)
            {
                Restart();
                return;
            }

            UIUpdate();
            Replay();
            //Restart


        }
        public void OnBallEnter()
        {
            if (brickGenerator.CheckBrickDestroyAll())
            {
                GenerateBrick();
            }
        }
        public void OnBallToFireball(Ball ball, float countTime)
        {
            ball.SwitchToFireBall(countTime);
            brickGenerator.FireballEffect(countTime);
        }
        public void OnCollectedItem(Item item)
        {
            playerController.OnItemCollect(item);
            if (item is FloorHelper)
            {
                var _timeCount = item.timeCount;
                bottomWall.isTrigger = false;
                StartCoroutine(DelayToAction(_timeCount, () =>
                {
                    bottomWall.isTrigger = true;
                }));
            }
        }

        public void OnBrickDestroy(Brick brick)
        {
            if (brick.BrickData.brickType == Resources.BrickType.Understory)
            {
                if (brickGenerator.CheckBrickDestroyAll())
                {
                    GenerateBrick();
                }
                return;
            }
            scorePoint += brick.BrickData.scorePoint;
            itemsManager.OnBrickDestroy(brick);
            brickGenerator.DestroyBrick(brick);
            UIUpdate();
            if (brickGenerator.CheckBrickDestroyAll())
            {
                GenerateBrick();
            }
        }


        void OnPlayerDeath()
        {

            OnPauseGame();
        }
        void GenerateBrick()
        {
            brickGenerator.GenerateBrick();
        }
        public void OnPressSpaceBar(InputAction.CallbackContext context)
        {
            //Debug.Log("Spacebar");
            if (context.performed)
            {
                OnPauseGame();
            }


        }
        void OnStart()
        {
            UIUpdate();
            Time.timeScale = 0;
            messageUI.text = "Press Space to play";
            messageUI.gameObject.SetActive(true);
            ballManager.GetStartBall();
        }
        void Replay()
        {
            ballManager.GetStartBall();
            IsPlaying = false;
            Time.timeScale = 0;
            messageUI.text = "Press Space to play";
            messageUI.gameObject.SetActive(true);
            playerController.ResetOnStart();
        }
        void Restart()
        {
            playerLive = 3;
            scorePoint = 0;
            GenerateBrick();
            UIUpdate();
            Replay();
        }
        void OnPauseGame()
        {
            IsPlaying = !IsPlaying;
            //Debug.Log(IsPlaying);
            if (!IsPlaying)
            {
                Time.timeScale = 0;
                messageUI.text = "Press Space to play";
                messageUI.gameObject.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                messageUI.gameObject.SetActive(false);
            }
        }
        void UIUpdate()
        {
            scoreUI.text = "Score : " + scorePoint;
            liveUI.text = "Live : " + playerLive;
        }
        IEnumerator DelayToAction(float time, Action action = null)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }


    }
}

