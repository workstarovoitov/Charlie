using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MenuManagement
{
    public class LevelUnlockPopup : MonoBehaviour
    {
        [SerializeField] private Text levelNum;
        [SerializeField] private Text levelPrice;
        [SerializeField] private Text coinsAmount;


        public int LevelName
        {
            set
            {
                levelNum.text = value.ToString();
            }
        }
        public int LevelCost
        {
            set
            {
                levelPrice.text = value.ToString();
            }
        }
        public int CoinsAmount
        {
            set
            {
                coinsAmount.text = value.ToString();
            }
        }
    }
}