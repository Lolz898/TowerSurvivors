using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    public static int gold = 0;
    public bool isGamePaused = false;

    // Other game-related variables and references can be added here

    private void Awake()
    {
        // Ensure only one instance of the GameManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Example: Add 10 gold with the "G" key
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddGold(10);
        }

        // Example: Toggle pause with the "P" key or Escape key
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }
    }

    public void AddGold(int amount)
    {
        gold += amount;
        // Update UI or perform other actions related to gold
    }

    void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Debug.Log("Game paused.");
        }
        else
        {
            Debug.Log("Game unpaused.");
        }

        // Pause or resume the game based on the isGamePaused flag
        Time.timeScale = isGamePaused ? 0f : 1f;
    }

    // Other game-related methods can be added here
}
