using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerMenuController : MonoBehaviour
{
    public GameObject towerButtonPrefab; // Prefab of the TowerButton UI element
    public Transform buttonContainer; // Parent transform where tower buttons will be instantiated

    public Tower towerPrefab; // Temporary storage for testing buttons until other gameplay has been added, will eventually have towers sent from levelling up

    private RectTransform menuContainer;
    private Vector3 onScreenPosition;
    private Vector3 offScreenPosition;

    public float transitionSpeed = 8f;

    void Start()
    {
        menuContainer = GetComponent<RectTransform>();

        // Initialize on and off-screen positions
        offScreenPosition = menuContainer.anchoredPosition;
        onScreenPosition = new Vector3(offScreenPosition.x, offScreenPosition.y + 54f, offScreenPosition.z);

        AddTowerButton(towerPrefab);
    }

    void Update()
    {
        // Check if the mouse is over the container
        if (RectTransformUtility.RectangleContainsScreenPoint(menuContainer, Input.mousePosition))
        {
            // If mouse is over, move towards on-screen position
            menuContainer.anchoredPosition = Vector3.Lerp(menuContainer.anchoredPosition, onScreenPosition, Time.deltaTime * transitionSpeed);
        }
        else
        {
            // If mouse is not over, move towards off-screen position
            menuContainer.anchoredPosition = Vector3.Lerp(menuContainer.anchoredPosition, offScreenPosition, Time.deltaTime * transitionSpeed);
        }
    }

    void AddTowerButton(Tower tower)
    {
        GameObject buttonGO = Instantiate(towerButtonPrefab, buttonContainer);
        TowerButton button = buttonGO.GetComponent<TowerButton>();

        if (button != null)
        {
            button.SetTowerData(tower);
        }
        else
        {
            Debug.LogWarning("TowerButton component not found on TowerButton prefab.");
        }
    }
}
