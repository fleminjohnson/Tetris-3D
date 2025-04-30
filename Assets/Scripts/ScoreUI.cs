using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace ClassicTetrisGame
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text scoreText;

        public void SetScore(float score)
        {
            scoreText.text = "Score \n" + score.ToString();
        }
    }
}
