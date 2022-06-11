using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardsGrid : MonoBehaviour
{
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private int rowCount;
    [SerializeField] private int columnCount;
    
    private Cell[] cells;
    private List<Cell> emptyCells = new List<Cell>();

    private void Awake()
    {
        SpawnCells();
    }

    private void SpawnCells()
    {
        List<Cell> cellsList = new List<Cell>();

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                Vector3 currentPosition = new Vector3(j, i, 0);
                var newCell = Instantiate(cellPrefab, currentPosition, Quaternion.identity, transform);
                cellsList.Add(newCell);
            }
        }

        emptyCells = cellsList;
        cells = cellsList.ToArray();
    }

    public void ResetCells()
    {
        foreach (var cell in cells)
            cell.IsEmpty = true;

        emptyCells = null;
        emptyCells = cells.ToList();
    }

    public bool HasEmptyCells()
    {
        if (emptyCells.Count > 0) 
            return true;

        return false;
    }

    public Cell GetRandomEmptyCell()
    {
        if (!HasEmptyCells())
            return null;
        
        int randomIndex = Random.Range(0, emptyCells.Count);
        Cell randomCell = emptyCells[randomIndex];
        randomCell.IsEmpty = false;
        emptyCells.Remove(randomCell);
        
        return randomCell;
    }

    public bool IsCellEmpty(Vector2 cellPosition)
    {
        foreach (var cell in cells)
            if ((Vector2)cell.Position == cellPosition && cell.IsEmpty)
                return true;
        
        return false;
    }

    public bool IsBorder(Vector2 position)
    {
        int xPos = (int)position.x;
        int yPos = (int)position.y;
        bool isHorizontalBorder = xPos == columnCount || xPos == -1;
        bool isVerticalBorder = yPos == rowCount || yPos == -1;

        if (isHorizontalBorder || isVerticalBorder)
            return true;

        return false;
    }
}
