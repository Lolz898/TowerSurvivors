using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image fillImage; // Reference to the fill image
    private Coroutine fillCoroutine;

    public void SetHealth(float healthPercentage)
    {
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }
        fillCoroutine = StartCoroutine(SmoothFill(healthPercentage));
    }

    private IEnumerator SmoothFill(float targetFillAmount)
    {
        float startFillAmount = fillImage.fillAmount;
        float duration = 0.1f; // Duration of the fill animation
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
