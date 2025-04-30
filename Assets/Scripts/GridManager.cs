using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClassicTetrisGame
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField]
        private int width = 10;
        [SerializeField]
        private int height = 20;
        [SerializeField]
        private int depth = 1;
        [SerializeField]
        private Transform VisualCubes;

        private Transform[,,] gridArray;

        public int Width { get => width;}
        public int Height { get => height;}
        public int Depth { get => depth;}

        // Start is called before the first frame update
        void Start()
        {
            gridArray = new Transform[width, height, depth];

            //VisualizeGrid();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private bool IsRowFull(int y)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (gridArray[x, y, z] == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void ShiftRowsDown(int startY)
        {
            for(int y = startY + 1 ; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        Transform block = gridArray[x, y, z];
                        if(block != null)
                        {
                            gridArray[x, y - 1, z] = block;
                            gridArray[x, y, z] = null;
                            block.position += Vector3.down;
                        }
                    }
                }
            }
        }

        private void ClearRow(int y)
        {
            AudioManager.Instance.PlaySFX(SFXType.ClearSound);
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    Destroy(gridArray[x, y, z].gameObject);
                    gridArray[x, y, z] = null;
                }
            }
        }

        public void VisualizeGrid()
        {
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    for(int z = 0; z < depth; z++)
                    {
                        GameObject placeHolder = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        placeHolder.transform.position = new Vector3(x, y, z);
                        placeHolder.transform.localScale = Vector3.one * 0.1f;
                        placeHolder.GetComponent<Renderer>().material.color = Color.gray;
                        placeHolder.transform.SetParent(VisualCubes);
                    }
                }
            }
        }

        public bool IsOccupied(int x, int y, int z)
        {
            return gridArray[x, y, z] != null;
        }

        public bool SetCell(int x, int y, int z, Transform block)
        {
            if(x >= 0 && x < gridArray.GetLength(0) &&
               y >= 0 && y < gridArray.GetLength(1) &&
               z >= 0 && z < gridArray.GetLength(2))
            {
                gridArray[x, y, z] = block;
                return true;
            }
            return false;
        }

        public int ClearFullRows()
        {
            int clearedRowCount = 0;

            for(int y = 0; y < height; y++)
            {
                if (IsRowFull(y))
                {
                    ClearRow(y);
                    ShiftRowsDown(y);
                    y--;
                    clearedRowCount++;
                }
            }
            return clearedRowCount;
        }
    }
}
