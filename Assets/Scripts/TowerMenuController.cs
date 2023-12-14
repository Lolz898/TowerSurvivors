using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerMenuController : MonoBehaviour
{
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
}
