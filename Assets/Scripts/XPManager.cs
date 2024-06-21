using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;

        // Subscribe to the XP change event
        gameManager.OnXPChanged += CheckForLevelUp;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the XP change event to prevent memory leaks
        if (gameManager != null)
        {
            gameManager.OnXPChanged -= CheckForLevelUp;
        }
    }

    private void CheckForLevelUp()
    {
        while (gameManager.GetXP() >= gameManager.nextLevel)
        {
            // Level up the player
            gameManager.playerLevel++;
            gameManager.ChangeXP(-(gameManager.nextLevel));

            // Increase the XP required for the next level (example logic)
            gameManager.nextLevel += 10;

            Debug.Log("Player leveled up! New level: " + gameManager.playerLevel);
        }
    }
}
