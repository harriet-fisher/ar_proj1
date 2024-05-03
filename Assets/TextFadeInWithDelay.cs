using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFadeInWithDelay : MonoBehaviour
{
    public float fadeDuration = 1.0f;
    public float waitTime = 1.0f;

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        StartCoroutine(DelayedFadeIn());
    }

    IEnumerator DelayedFadeIn()
    {
        yield return new WaitForSeconds(waitTime);

        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            canvasGroup.alpha = timeElapsed / fadeDuration;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}