using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClassicTetrisGame
{
    public class TetrominoController : MonoBehaviour
    {
        [SerializeField] private float moveDelay = 0.2f;
        [SerializeField] private float dropDelay = 1.0f;
        [SerializeField] private float speed = 5.0f;
        [SerializeField] TetrominoType tetrominoType;

        private float moveTimer;
        private float dropTimer;
        private GridManager gridManager;

        public TetrominoType TetrominoType { get => tetrominoType;}

        private void Start()
        {
            gridManager = GameManager.Instance.GridManager;
        }

        // Update is called once per frame
        void Update()
        {
            HandleHorizontalMovement();
            HandleRotation();
            HandleDropping();
        }

        private void HandleDropping()
        { 

            dropTimer += Time.deltaTime;
            dropDelay = Mathf.Max(0.5f, 1 - GameManager.Instance.Score / GameManager.Instance.SpeedUpFactor);

            if (dropTimer >= dropDelay)
            {
                dropTimer = 0f;
                Debug.Log("Attempt movedown");
                AttemptMoveDown();
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                Debug.Log("Attempt hard movedown");
                AttemptMoveDown();
            }
        }

        private void HandleRotation()
        {
            if (tetrominoType == TetrominoType.O) return;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                AudioManager.Instance.PlaySFX(SFXType.RotateSound);
                Quaternion oldRotation = transform.rotation;

                transform.Rotate(0f,0f,90f);

                if (!IsValidMove())
                {
                    transform.rotation = oldRotation;
                }
            }
        }

        private void HandleHorizontalMovement()
        {
            moveTimer += Time.deltaTime;

            if(Input.GetKey(KeyCode.LeftArrow) && moveTimer >= moveDelay)
            {
                Vector3 oldPos = transform.position;

                transform.position += Vector3.left;
                moveTimer = 0f;

                AudioManager.Instance.PlaySFX(SFXType.MoveSound);

                if (!IsValidMove())
                {
                    transform.position = oldPos;
                }
            }

            if (Input.GetKey(KeyCode.RightArrow) && moveTimer >= moveDelay)
            {
                Vector3 oldPos = transform.position;

                AudioManager.Instance.PlaySFX(SFXType.MoveSound);

                transform.position += Vector3.right;
                moveTimer = 0f;

                if (!IsValidMove())
                {
                    transform.position = oldPos;
                }
            }
        }

        private bool IsValidMove()
        {
            foreach(Transform block in transform)
            {
                Vector3 pos = block.position;

                int x = Mathf.RoundToInt(pos.x);
                int y = Mathf.RoundToInt(pos.y);
                int z = Mathf.RoundToInt(pos.z);


                //Check bounds
                if (x < 0 || x >= gridManager.Width ||
                   y < 0 || y >= gridManager.Height ||
                   z < 0 || z >= gridManager.Depth)
                {
                    return false;
                }

                if (gridManager.IsOccupied(x,y,z))
                {
                    return false;
                }
            }

            return true;
        }

        private void AttemptMoveDown()
        {
            Vector3 oldPos = transform.position;
            transform.position += Vector3.down;

            if (!IsValidMove())
            {
                transform.position = oldPos;
                LockPiece();
            }
        }

        private void LockPiece()
        {
            AudioManager.Instance.PlaySFX(SFXType.LandSound);

            foreach (Transform block in transform)
            {
                Vector3 pos = block.position;

                int x = Mathf.RoundToInt(pos.x);
                int y = Mathf.RoundToInt(pos.y);
                int z = Mathf.RoundToInt(pos.z);

                if (!gridManager.SetCell(x, y, z, block))
                {
                    GameManager.Instance.TriggerGameOver(true);
                }
            }

            enabled = false;

            if (!GameManager.Instance.IsGameOver)
            {
                Debug.Log("From Tetromino controller 167");
                GameManager.Instance.SpawnTetrominoPrefab();
                GameManager.Instance.AddScore(gridManager.ClearFullRows());
            }
        }
    }

    public enum TetrominoType
    {
        I,
        J,
        O,
        S,
        T
    }

}