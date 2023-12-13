using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager
    public TileOccupier tileOccupier; // Reference to the TileOccupier script

    public int baseHealth = 10;

    void Start()
    {
        // Get the references to GameManager and TileOccupier
        gameManager = GameManager.instance; // Assuming GameManager is a singleton
        tileOccupier = gameManager.GetComponentInChildren<TileOccupier>();

        if (gameManager != null && tileOccupier != null)
        {
            // Occupy a 2x2 area of tiles underneath the player base
            OccupyBaseTiles();
        }
        else
        {
            Debug.LogError("GameManager or TileOccupier script not found.");
        }
    }

    void OccupyBaseTiles()
    {
        // Assuming the base is positioned at the center of the 2x2 area
        Vector3 basePosition = transform.position;

        // Occupy a 2x2 area of tiles
        tileOccupier.OccupyTiles(basePosition); // Top right tile by default
        tileOccupier.OccupyTiles(basePosition + new Vector3(-1, 0, 0)); // Offset left
        tileOccupier.OccupyTiles(basePosition + new Vector3(0, -1, 0)); // Offset down
        tileOccupier.OccupyTiles(basePosition + new Vector3(-1, -1, 0)); // Offset left and down
    }

    public void TakeDamage(int damage)
    {
        if (gameManager != null)
        {
            // Damage the player's HP in the GameManager
            gameManager.ChangeHealth(-damage);

            // Check if the player's HP has reached zero
            if (GameManager.playerHP <= 0)
            {
                // Game over logic can be implemented here
                Debug.Log("Game Over");
            }
        }
        else
        {
            Debug.LogError("GameManager reference not found on PlayerBase GameObject.");
        }
    }
}
