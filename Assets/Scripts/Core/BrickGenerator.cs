using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrickBreaker.Resources;
using BrickBreaker.Manager;
using System;
namespace BrickBreaker.Core
{
    public class BrickGenerator : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField]
        GameObject greenBrickPrefab; //Example
        [SerializeField]
        GameObject blueBrickPrefab;
        [SerializeField]
        GameObject yellowBrickPrefab;
        [SerializeField]
        GameObject purpleHardBrickPrefab;
        [SerializeField]
        GameObject undestroyableBrickPrefab;
        [SerializeField]
        GameObject bombBrickPrefab;
        [SerializeField]
        Transform starBrickTransform;

        [Header("Option")]
        [SerializeField]
        int hardBrickRandom = 5;//On last 5 line
        [SerializeField]
        int undestroyableRandom = 3; // Any lines
        [SerializeField]
        int fireballRandom = 7;//When brick destroy
        [SerializeField]
        int bombBrickRandom = 4; //Any line


        float padX = 0.98f;
        float padY = -0.47f;
        int row = 7;
        int column = 10;
        Brick[,] brickGrids = new Brick[7, 10];
        Vector3 currentPosition = new Vector3();
        // Start is called before the first frame update
        void Start()
        {
            //For test
            // GenerateBrick();
            // RandomHardBrick();
        }

        // Update is called once per frame
        void Update()
        {

        }
        //[ContextMenu("Generate Brick")]
        public void GenerateBrick()
        {
            DestroyAllBricks();

            currentPosition = starBrickTransform.position;
            //var hardBrick = hardBrickRandom;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    GameObject _prefab = null;
                    //  lastline

                    if (i <= 1) _prefab = yellowBrickPrefab;

                    if (i > 1 && i <= 4) _prefab = greenBrickPrefab;
                    if (i > 4 && i <= 7) _prefab = blueBrickPrefab;

                    var _temp = Instantiate(_prefab, currentPosition, Quaternion.identity);
                    var _tempBrick = _temp.GetComponent<Brick>();
                    brickGrids[i, j] = _tempBrick;
                    _temp.transform.SetParent(starBrickTransform);
                    currentPosition.x += padX;
                    _tempBrick.IndexPos = new int[,] { { i, j } };
                }
                currentPosition.x = starBrickTransform.position.x;
                currentPosition.y += padY;
            }
            //Random Other Brick
            RandomOtherBrick(purpleHardBrickPrefab, 1, 5, hardBrickRandom);//Random Hard Brick
            RandomOtherBrick(bombBrickPrefab, 5, 7, bombBrickRandom); //Random Bomb Brick
            RandomOtherBrick(undestroyableBrickPrefab, 1, 7, undestroyableRandom);//Random Undestroy Brick

        }
        void DestroyAllBricks()
        {

            var _temp = starBrickTransform.GetComponentsInChildren<Brick>();
            foreach (var item in _temp)
            {
                Destroy(item.gameObject);
            }
            brickGrids = new Brick[7, 10];
        }
        public bool CheckBrickDestroyAll()
        {
            var _temp = starBrickTransform.GetComponentsInChildren<Brick>();
            Debug.Log(_temp.Length);
            if (_temp.Length <= 0 || _temp == null) return true;
            foreach (var item in _temp)
            {
                if(item.BrickData.brickType != BrickType.Understory)
                {
                    Debug.Log(item.BrickData.brickType.ToString());
                    return false;
                    
                }
                else{

                }
            }
            return true;
        }
        public void DestroyBrick(Brick brick)
        {
            var _idxRow = brick.IndexPos[0, 0];
            var _idxColumn = brick.IndexPos[0, 1];
            Destroy(brick.gameObject);
            brickGrids[_idxRow, _idxColumn] = null;
            Debug.Log("All Brick :"+CheckBrickDestroyAll());
            if (CheckBrickDestroyAll())
            {
                GenerateBrick();
            }
        }

        void RandomOtherBrick(GameObject brickPrefab, int startLine, int lastLine, int randomCount)
        {
            //var hardBrick = hardBrickRandom;
            for (int count = 0; count < randomCount; count++)
            {
                var _ranRow = UnityEngine.Random.Range(startLine - 1, lastLine);
                var _ranColmn = UnityEngine.Random.Range(0, column);
                //Debug.Log(_ranRow + "," + _ranColmn);
                var _brickType = brickGrids[_ranRow, _ranColmn].BrickData.brickType;
                if (_brickType == BrickType.Bomb || _brickType == BrickType.Understory || _brickType == BrickType.Hard)
                {

                    count--;
                    return;
                }
                if (_brickType == BrickType.Normal)
                {
                    var _temp = Instantiate(brickPrefab, currentPosition, Quaternion.identity);
                    _temp.transform.position = brickGrids[_ranRow, _ranColmn].transform.position;
                    _temp.transform.SetParent(starBrickTransform);
                    var _tempBrick = _temp.GetComponent<Brick>();
                    Destroy(brickGrids[_ranRow, _ranColmn].gameObject);//TODO add to pool
                    //brickGrids[_ranRow, _ranColmn] = null;
                    brickGrids[_ranRow, _ranColmn] = _tempBrick;
                    _tempBrick.IndexPos = new int[,] { { _ranRow, _ranColmn } };

                }
                // Debug.Log(" :" + count);

            }

        }



        public void BombBrickEffect(Brick brick)
        {
            if (brick.BrickData.brickType == BrickType.Bomb)
            {
                var _idxRow = brick.IndexPos[0, 0];
                var _idxColumn = brick.IndexPos[0, 1];
                var _ran = UnityEngine.Random.Range(0, 2);
                if (_idxColumn <= 4)
                {
                    //Left Boom
                    for (int i = _idxColumn; i >= 0; i--)
                    {

                        if (brickGrids[_idxRow, i] == null) return;
                        GameManager.Instance.OnBrickDestroy(brickGrids[_idxRow--, i]);
                        //Debug.Log("Pos :" + _idxRow + ", " + i);
                    }
                }
                else
                {
                    //Right Boom
                    for (int i = _idxColumn; i < column; i++)
                    {
                        if (brickGrids[_idxRow, i] == null) return;
                        GameManager.Instance.OnBrickDestroy(brickGrids[_idxRow--, i]);
                        //Debug.Log("Pos :" + _idxRow + ", " + i);
                    }
                }
            }
        }

        public void FireballEffect(float countTime)
        {
            var _temp = starBrickTransform.GetComponentsInChildren<Brick>();
            foreach (var item in _temp)
            {
                item.GetComponent<BoxCollider2D>().isTrigger = true;
            }
            StartCoroutine(DelayToAction(countTime, () =>
            {
                var _temp = starBrickTransform.GetComponentsInChildren<Brick>();
                foreach (var item in _temp)
                {
                    item.GetComponent<BoxCollider2D>().isTrigger = false;
                }
            }));
        }
        IEnumerator DelayToAction(float time, Action action = null)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }

    }
}

