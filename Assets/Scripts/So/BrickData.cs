using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace BrickBreaker.Resources
{
    [CreateAssetMenu(fileName = "BrickData", menuName = "BrickBreaker/BrickData")]
    public class BrickData : ScriptableObject
    {
        public Sprite sprite;
        public BrickType brickType = BrickType.Normal;
        public int scorePoint = 10;
        public int smashTime = 1;


    }
    public enum BrickType
    {
        Normal,
        Understory,
        Bomb
    }
}

