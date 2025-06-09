using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    private TextMeshProUGUI goldText;
    private GameManager gameManager;

    private void Start()
    {
        // Get the TextMeshPro component
        goldText = GetComponent<TextMeshProUGUI>();

        // Get the GameManager reference
        gameManager = GameManager.instance;

        // Update the initial gold value
        UpdateGoldText();

        // Subscribe to the gold change event
        gameManager.OnGoldChanged += UpdateGoldText;
    }

    private void UpdateGoldText()
    {
        // Update the text with the current gold amount
        goldText.text = GameManager.gold.ToString();
    }
}
