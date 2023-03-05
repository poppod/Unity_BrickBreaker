using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
namespace BrickBreaker.Manager
{
    public class BallManager : MonoBehaviour
    {
        [Header("Resource")]
        [SerializeField]
        Sprite defaultBall;
        [SerializeField]
        Sprite fireBall;
        [Header("Content")]
        [SerializeField]
        GameObject ballPrefab;
        [Header("StartPosition")]
        [SerializeField]
        Vector3 ballSpawnPosition = new Vector3(0, -1.25f, 0);


        ObjectPool<GameObject> ballsPool;
        // Start is called before the first frame update
        void Start()
        {
            ballsPool = new ObjectPool<GameObject>(
                () => { return Instantiate(ballPrefab, ballSpawnPosition, Quaternion.identity); },
                (_obj) =>
                {
                    _obj.transform.position = ballSpawnPosition;
                    _obj.SetActive(true);
                },
                (_obj) =>
                {
                    _obj.SetActive(false);
                },
                (_obj) =>
                {
                    Destroy(_obj);
                },
                true,
                2,
                1000
                );
        }

        // Update is called once per frame
        void Update()
        {

        }
        public bool CheckActiveBall()
        {
            if (ballsPool.CountActive > 0) return true;
            return false;
        }
        public void GetBall()
        {
            ballsPool.Get();
        }
        public void GetStartBall()
        {
            GetBall(ballSpawnPosition);
        }
        public void GetBall(Vector3 spawnPosition)
        {
            var _temp = ballsPool.Get();
            _temp.transform.position = spawnPosition;
        }
        public GameObject GetBallObject(Vector3 spawnPosition)
        {
            var _temp = ballsPool.Get();
            _temp.transform.position = spawnPosition;
            return _temp;
        }
        public void ReleaseBall(GameObject ball)
        {
            ballsPool.Release(ball);
        }
        #region 
        public void SwitchToFireBall()
        {
            
        }
        #endregion

    }
}

