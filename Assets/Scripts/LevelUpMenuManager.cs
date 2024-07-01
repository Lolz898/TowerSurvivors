using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LevelUpMenuManager : MonoBehaviour
{
    public TowerMenuController towerMenuController;
    public GameObject levelUpPanel; // Panel to show tower choices
    public Button[] towerButtons; // Buttons for tower choices
    public TMP_Text towerNameText; // Text to display tower name
    public TMP_Text towerDamageText;
    public TMP_Text towerFireRateText;
    public TMP_Text towerHealthText;
    public TMP_Text towerRangeText;
    public TMP_Text towerCostText;

    private List<TowerData> availableTowers;

    private void Start()
    {
        PopulateTowerChoices();
    }

    // Method to show the level-up panel with tower choices
    public void ShowLevelUpOptions()
    {
        levelUpPanel.SetActive(true);
        PopulateTowerChoices();
    }

    // Method to populate the tower choice buttons
    private void PopulateTowerChoices()
    {
        availableTowers = TowerManager.instance.GetAvailableTowers();

        // Shuffle the available towers to get random choices
        for (int i = 0; i < availableTowers.Count; i++)
        {
            TowerData temp = availableTowers[i];
            int randomIndex = Random.Range(i, availableTowers.Count);
            availableTowers[i] = availableTowers[randomIndex];
            availableTowers[randomIndex] = temp;
        }

        // Assign towers to buttons
        for (int i = 0; i < towerButtons.Length; i++)
        {
            if (i < availableTowers.Count)
            {
                towerButtons[i].gameObject.SetActive(true);
                int index = i;
                towerButtons[i].onClick.RemoveAllListeners();
                towerButtons[i].onClick.AddListener(() => OnTowerSelected(availableTowers[index]));
                towerButtons[i].GetComponentInChildren<Image>().sprite = availableTowers[index].icon;

                // Add PointerEnter and PointerExit events dynamically
                AddEventTrigger(towerButtons[i], EventTriggerType.PointerEnter, () => ShowTowerStats(availableTowers[index]));
                // AddEventTrigger(towerButtons[i], EventTriggerType.PointerExit, () => ClearTowerStats());
            }
            else
            {
                towerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // Method to handle tower selection
    private void OnTowerSelected(TowerData selectedTower)
    {
        TowerManager.instance.PickTower(selectedTower.id);
        UpdateShopWithTower(selectedTower);
        ClearTowerStats();
        towerMenuController.AddTowerButton(selectedTower.towerPrefab.GetComponent<Tower>());
        levelUpPanel.SetActive(false);
    }

    // Method to update the shop with the selected tower
    private void UpdateShopWithTower(TowerData towerData)
    {
        // Implement logic to add the tower to the shop interface
        Debug.Log("Tower " + towerData.name + " added to shop.");
    }

    // Method to display tower stats when hovering over a tower button
    public void ShowTowerStats(TowerData towerData)
    {
        towerNameText.text = towerData.name;
        towerHealthText.text = towerData.maxhp.ToString();
        towerDamageText.text = towerData.damage.ToString();
        towerFireRateText.text = towerData.fireRate.ToString();
        towerRangeText.text = towerData.range.ToString();
        towerCostText.text = towerData.goldCost.ToString();
    }

    // Method to clear tower stats when not hovering over a tower button
    public void ClearTowerStats()
    {
        towerNameText.text = "??";
        towerHealthText.text = "??";
        towerDamageText.text = "??";
        towerFireRateText.text = "??";
        towerRangeText.text = "??";
        towerCostText.text = "??";
    }

    // Helper method to add EventTrigger events to a button
    private void AddEventTrigger(Button button, EventTriggerType eventType, UnityEngine.Events.UnityAction callback)
    {
        EventTrigger eventTrigger = button.gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = button.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener((data) => { callback.Invoke(); });

        eventTrigger.triggers.Add(entry);
    }
}
