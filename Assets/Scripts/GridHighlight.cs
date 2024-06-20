using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridHighlight : MonoBehaviour
{
    public Tilemap highlightTilemap;
    public TileBase highlightTile;
    public TileOccupier tileOccupier;

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = highlightTilemap.WorldToCell(mousePos);

        // Check if the cell is not occupied before setting the highlight tile
        if (!tileOccupier.CheckIfCellOccupied(cellPos))
        {
            // Set the highlight tile position to the center of the cell
            highlightTilemap.SetTile(cellPos, highlightTile);
        }

        // Clear the highlight from other cells
        ClearHighlightsExcept(cellPos);
    }

    void ClearHighlightsExcept(Vector3Int highlightedCell)
    {
        // Loop through all cells in the tilemap and clear the highlight except the current cell
        BoundsInt bounds = highlightTilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin)
        {
            // Check if the cell is not the highlighted cell and is not occupied
            if (pos != highlightedCell && !tileOccupier.CheckIfCellOccupied(pos))
            {
                highlightTilemap.SetTile(pos, null);
            }
        }
    }

    public void ClearHighlights()
    {
        // Loop through all cells in the tilemap and clear the highlight
        BoundsInt bounds = highlightTilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin)
        {
            // Check if the cell is not the highlighted cell and is not occupied
            if (!tileOccupier.CheckIfCellOccupied(pos))
            {
                highlightTilemap.SetTile(pos, null);
            }
        }
    }
}
