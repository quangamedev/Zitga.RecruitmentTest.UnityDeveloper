using System.Collections;
using System.Collections.Generic;
using DesignPatterns.Singleton;
using UnityEngine;

public class MazeGenerator : Singleton<MazeGenerator>
{
    [Header("Maze Settings")]
    [SerializeField] private int _rows = 13;
    [SerializeField] private int _cols = 10;

    [Header("References")]
    [SerializeField] private Cell _cell;

    private float _cellSize;
    private List<Cell> _cells = new List<Cell>();
    // Start is called before the first frame update
    void Start()
    {
        _cellSize = _cell.transform.localScale.x;
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void GenerateGrid()
    {
        float halfCellSize = _cellSize / 2f;
        var gridOrigin = new Vector3(-_cols * halfCellSize + halfCellSize, -_rows * halfCellSize + halfCellSize, 0);
        
        for (int row = 0; row < _rows; row++) {
            for (int col = 0; col < _cols; col++)
            {
                Cell cell = Instantiate(_cell, transform);
                _cells.Add(cell);
                float xPos = col * _cellSize;
                float yPos = row * _cellSize;
                cell.transform.position = new Vector3(xPos, yPos, 0) + gridOrigin;
                cell.name = $"({row}, {col})";
                cell.gridPos.x = row;
                cell.gridPos.y = col;
            }
        }
    }
}
