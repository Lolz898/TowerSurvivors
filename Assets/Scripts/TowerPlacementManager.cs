using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacementManager : MonoBehaviour
{
    private GridHighlight gridHighlight; // Reference to the GridHighlight script
    private TileOccupier tileOccupier; // Reference to the TileOccupier script
    private GameManager gameManager; // Reference to the GameManager script

    private Tower currentTower; // Current tower to place
    private int currentTowerCost; // Cost of the current tower

    private bool isPlacingTower = false; // Flag to track if player is placing a tower

    void Start()
    {
        // Get references to GameManager, GridHighlight, and TileOccupier
        gameManager = GameManager.instance;
        gridHighlight = gameManager.GetComponent<GridHighlight>();
        tileOccupier = gameManager.GetComponentInChildren<TileOccupier>();
    }
    private void Update()
    {
        if (isPlacingTower)
        {
            // Check for mouse click
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                // Perform raycast to detect where the player clicked
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null)
                {
                    // Check if the hit object is a valid tile or grid cell
                    Vector3 clickPosition = hit.point; // Get the world position where the ray hit

                    // Check if the tile is occupied
                    if (tileOccupier.CheckIfCellOccupied(clickPosition))
                    {
                        Debug.Log("Tile is already occupied.");
                        return;
                    }
                    else
                    {
                        // Calculate the center position of the cell
                        Vector3 centerPosition = tileOccupier.GetCellCentre(clickPosition);

                        // Place the tower: occupy tile, disable GridHighlight
                        tileOccupier.OccupyTiles(centerPosition); // Adjust this method based on your TileOccupier implementation
                        gridHighlight.ClearHighlights();
                        gridHighlight.enabled = false;
                        isPlacingTower = false;

                        // Instantiate the tower prefab or handle placement logic here
                        Instantiate(currentTower, centerPosition, Quaternion.identity);
                    }
                }
            }
        }
    }

    public void StartTowerPlacement(Tower tower)
    {
        if (isPlacingTower)
        {
            return;
        }

        currentTower = tower;
        currentTowerCost = tower.goldCost;

        // Check if player can afford the tower
        if (CanAffordTower(currentTowerCost))
        {
            // Enable GridHighlight to show valid placement positions
            gridHighlight.enabled = true;
            isPlacingTower = true;
            gameManager.ChangeGold(-currentTowerCost);
        }
        else
        {
            Debug.Log("Not enough gold to place the tower.");
        }
    }

    private bool CanAffordTower(int cost)
    {
        return GameManager.gold >= cost;
    }
}
