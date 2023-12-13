using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    public static int gold = 0;
    public static int playerHP = 10;
    public bool isGamePaused = false;

    public GameObject playerBase;

    public delegate void HealthChanged();
    public HealthChanged OnHealthChanged;

    public delegate void GoldChanged();
    public GoldChanged OnGoldChanged;

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

    private void Start()
    {
        Instantiate(playerBase, Vector3.zero, Quaternion.identity);
    }

    void Update()
    {
        // Example: Add 10 gold with the "G" key
        if (Input.GetKeyDown(KeyCode.G))
        {
            ChangeGold(10);
        }

        // Example: Toggle pause with the "P" key or Escape key
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }
    }

    public void ChangeGold(int amount)
    {
        gold += amount;
        // Update UI or perform other actions related to gold
        OnGoldChanged?.Invoke();
    }

    public void SetGold(int amount)
    {
        gold = amount;
        // Update UI or perform other actions related to gold
        OnGoldChanged?.Invoke();
    }

    public void ChangeHealth(int amount)
    {
        playerHP += amount;
        // Update UI or perform other actions related to gold
        OnHealthChanged?.Invoke();
    }

    public void SetHealth(int amount)
    {
        playerHP = amount;
        // Update UI or perform other actions related to gold
        OnHealthChanged?.Invoke();
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
