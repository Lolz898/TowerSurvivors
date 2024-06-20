using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerButton : MonoBehaviour
{
    public TMP_Text costText;
    public Image towerIcon;
    private Tower tower;
    private GameManager gameManager;
    private TowerPlacementManager placementManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        placementManager = gameManager.GetComponent<TowerPlacementManager>();
    }

    public void SetTowerData(Tower towerData)
    {
        tower = towerData;

        SpriteRenderer towerSpriteRenderer = towerData.GetComponent<SpriteRenderer>();
        if (towerSpriteRenderer != null)
        {
            towerIcon.sprite = towerSpriteRenderer.sprite; // Set the sprite to display on the button
        }
        else
        {
            Debug.LogWarning("Tower does not have a SpriteRenderer attached.");
        }

        costText.text = "$" + towerData.goldCost.ToString(); // Use goldCost from Tower class

        // Set other visuals as needed
    }

    public void OnButtonClick()
    {
        Debug.Log("Buying tower");
        placementManager.StartTowerPlacement(tower);
    }
}
