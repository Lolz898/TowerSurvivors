using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    private TextMeshProUGUI healthText;
    private GameManager gameManager;

    private void Start()
    {
        // Get the TextMeshPro component
        healthText = GetComponent<TextMeshProUGUI>();

        // Get the GameManager reference
        gameManager = GameManager.instance;

        // Update the initial health value
        UpdateHealthText();

        // Subscribe to the health change event
        gameManager.OnHealthChanged += UpdateHealthText;
    }

    private void UpdateHealthText()
    {
        // Update the text with the current player health
        healthText.text = GameManager.playerHP.ToString();
    }
}
