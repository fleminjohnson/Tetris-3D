using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClassicTetrisGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GridManager gridManager;
        [SerializeField]
        private GameObject[] tetrominoPrefabs;
        [SerializeField]
        private ScoreUI scoreUI;
        [SerializeField]
        private GameObject gameOverUI;
        [SerializeField]
        private Transform previewSpawnPoint;
        [SerializeField]
        private float scaleFactor = 0.7f;
        [SerializeField]
        private float speedUpFactor = 2000f;


        [SerializeField]
        private Vector3 spawnPosition = new Vector3(5, 19, 0);
        //[SerializeField]
        //private float spawnDelay = 1.0f;

        private float timer;
        private static GameManager instance = null;
        private int score = 0;
        private bool isGameOver = false;
        private GameObject nextTetromino;
        private GameObject previewTetromino;

        public GridManager GridManager { get => gridManager;}
        public static GameManager Instance { get => instance;}
        public int Score { get => score;}
        public bool IsGameOver { get => isGameOver;}
        public float SpeedUpFactor { get => speedUpFactor;}

        private void Awake()
        {
            if(Instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            PrepareNextTetromino();
            Debug.Log("From Game manager 60");
            SpawnTetrominoPrefab();
            TriggerGameOver(false);
        }

        private void PrepareNextTetromino()
        {
            if (previewTetromino != null)
            {
                Destroy(previewTetromino);
            }

            int randomIndex = UnityEngine.Random.Range(0, tetrominoPrefabs.Length);
            nextTetromino = tetrominoPrefabs[randomIndex];

            previewTetromino = Instantiate(nextTetromino, previewSpawnPoint.position, Quaternion.identity);
            previewTetromino.GetComponent<TetrominoController>().enabled = false;
            previewTetromino.transform.localScale *= scaleFactor;
        }

        public void SpawnTetrominoPrefab()
        {
            AudioManager.Instance.PlaySFX(SFXType.NewBlockSound);
            Debug.Log("Spawn Tetromino prefab");
            if (tetrominoPrefabs == null || tetrominoPrefabs.Length == 0) return;
            Instantiate(nextTetromino, spawnPosition, Quaternion.identity);
            PrepareNextTetromino();
        }

        public void AddScore(int linesCleared)
        {
            int points = 0;

            switch (linesCleared)
            {
                case 1: points = 100; break;
                case 2: points = 300; break;
                case 3: points = 500; break;
                case 4: points = 800; break;
                default: points = linesCleared * 200; break;
            }

            score += points;
            scoreUI.SetScore(score);
        }

        public void TriggerGameOver(bool status)
        {
            gameOverUI.SetActive(status);
            isGameOver = status;
            AudioManager.Instance.PlayMusic(!status);
        }
    }

}