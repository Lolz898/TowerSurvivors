using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridHighlight : MonoBehaviour
{
    public Tilemap highlightTilemap;
    public TileBase highlightTile;

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = highlightTilemap.WorldToCell(mousePos);

        // Set the highlight tile position to the center of the cell
        highlightTilemap.SetTile(cellPos, highlightTile);

        // Clear the highlight from other cells
        ClearHighlightsExcept(cellPos);
    }

    void ClearHighlightsExcept(Vector3Int highlightedCell)
    {
        // Loop through all cells in the tilemap and clear the highlight except the current cell
        BoundsInt bounds = highlightTilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin)
        {
            if (pos != highlightedCell)
            {
                highlightTilemap.SetTile(pos, null);
            }
        }
    }
}
