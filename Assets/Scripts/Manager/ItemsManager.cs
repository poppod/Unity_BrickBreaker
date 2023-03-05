using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrickBreaker.Core;

namespace BrickBreaker.Manager
{
    public class ItemsManager : MonoBehaviour
    {
        [Header("Prefabs Items")]
        [SerializeField]
        List<GameObject> items = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void OnBrickDestroy(Brick brick)
        {
            var _probRan = Random.Range(1,101); 
            if(_probRan>10) return;
            
            var _ranIdx = Random.Range(0,items.Count);
            
            var _obj = Instantiate(items[_ranIdx], brick.transform.position,Quaternion.identity );

        }
    }
}

