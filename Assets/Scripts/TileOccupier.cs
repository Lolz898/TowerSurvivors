using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileOccupier : MonoBehaviour
{
    public int width = 1;
    public int height = 1;
    public int gridSizeX = 99;
    public int gridSizeY = 99;

    private Tilemap tilemap;
    public Tile occupiedTile;

    private void Start()
    {
        // Assuming the Grid component is on the same GameObject as this script
        tilemap = GetComponentInChildren<Tilemap>();

        if (tilemap == null)
        {
            Debug.LogError("Tilemap component not found. Make sure it's a child of the GameObject with TileOccupier script.");
        }
    }

    public void OccupyTiles(Vector3 position)
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap component not assigned. Make sure to call Start or assign the Tilemap manually.");
            return;
        }

        Vector3Int cellPosition = tilemap.WorldToCell(position);

        // Mark the cells as occupied
        for (int x = cellPosition.x; x < cellPosition.x + width; x++)
        {
            for (int y = cellPosition.y; y < cellPosition.y + height; y++)
            {
                if (GridIsValid(x, y))
                {
                    tilemap.SetTile(new Vector3Int(x, y, cellPosition.z), occupiedTile);
                    Debug.Log("Occupying at: (" + x + "," + y + ")");
                }
            }
        }
    }

    public bool CheckIfCellOccupied(Vector3 position)
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap component not assigned. Make sure to call Start or assign the Tilemap manually.");
            return false;
        }

        Vector3Int cellPosition = tilemap.WorldToCell(position);
        return CheckIfCellOccupied(cellPosition);
    }

    private bool CheckIfCellOccupied(Vector3Int cellPosition)
    {
        TileBase tile = tilemap.GetTile(cellPosition);
        return tile == occupiedTile;
    }

    private bool GridIsValid(int x, int y)
    {
        // Assuming gridSizeX and gridSizeY are variables representing the size of your grid
        return x >= -gridSizeX && x < gridSizeX && y >= -gridSizeY && y < gridSizeY;
    }
}
