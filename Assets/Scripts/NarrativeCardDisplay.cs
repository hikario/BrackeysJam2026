using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NarrativeCardDisplay : MonoBehaviour
{
    [SerializeField] public NarrativeCardData narrativeCardData;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private Image narrativeImage;

    public void InitNarrativeCard(NarrativeCardData narrative)
    {
        narrativeCardData = narrative;
        displayText.text = narrative.messageContent;
        canvasGroup.alpha = 0;

        if (narrative.narrativeBeatSprite != null)
        {
            narrativeImage.sprite = narrative.narrativeBeatSprite;
            narrativeImage.color = Color.white;
        }
        else
        {
            narrativeImage.color = Color.clear;
        }
    }

    Coroutine coFadeCanvasGroup = null;
    public void FadeCanvasGroup(bool fadeIn, float duration)
    {
        if (coFadeCanvasGroup != null)
        {
            StopCoroutine(coFadeCanvasGroup);
            coFadeCanvasGroup = null;
        }
            
        coFadeCanvasGroup = StartCoroutine(CoFade(fadeIn, duration));        
    }

    private IEnumerator CoFade(bool fadeIn, float fadeDuration)
    {
        float currentDuration = 0;

        if (fadeIn)
        {
            NarrativeCardManager.instance.narrativeCardDefinitionReference.SetNarrativeCardViewedState(narrativeCardData);

            //Debug.Log($"Fade in over {fadeDuration}");
            while (currentDuration < fadeDuration)
            {
                currentDuration += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0, 1, currentDuration / fadeDuration);
                yield return null;
            }
        }
        else
        {
            //Debug.Log($"Fade out over {fadeDuration}");
            while (currentDuration < fadeDuration)
            {
                currentDuration += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1, 0, currentDuration / fadeDuration);
                yield return null;
            }
        }
    }
}