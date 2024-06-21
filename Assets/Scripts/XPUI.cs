using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPUI : MonoBehaviour
{
    public Image fillImage; // Reference to the fill image
    private GameManager gameManager;
    private Coroutine fillCoroutine;

    private void Start()
    {
        // Get the GameManager reference
        gameManager = GameManager.instance;

        // Update the initial XP value
        UpdateXPBarInstant();

        // Subscribe to the XP change event
        gameManager.OnXPChanged += UpdateXPBarSmooth;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the XP change event to prevent memory leaks
        if (gameManager != null)
        {
            gameManager.OnXPChanged -= UpdateXPBarSmooth;
        }
    }

    private void UpdateXPBarInstant()
    {
        fillImage.fillAmount = (float)gameManager.GetXP() / gameManager.nextLevel;
    }

    private void UpdateXPBarSmooth()
    {
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }
        fillCoroutine = StartCoroutine(SmoothFill((float)gameManager.GetXP() / gameManager.nextLevel));
    }

    private IEnumerator SmoothFill(float targetFillAmount)
    {
        float startFillAmount = fillImage.fillAmount;
        float duration = 0.2f; // Duration of the fill animation
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fillImage.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsed / duration);
            yield return null;
        }

        fillImage.fillAmount = targetFillAmount;
    }
}
