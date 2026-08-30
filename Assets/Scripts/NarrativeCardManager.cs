using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class NarrativeCardManager : MonoBehaviour
{
    public static NarrativeCardManager instance;
    [SerializeField] public NarrativeCardDefinition narrativeCardDefinitionReference;
    [SerializeField] private List<NarrativeCardData> currentNarrativeCardsData = new List<NarrativeCardData>();
    //[SerializeField] private NarrativeCardDisplay narrativeCardPrefab;
    [SerializeField] private NarrativeCardDisplay currentCard;

    [SerializeField] private RectTransform cardParentRectTransform;
    [SerializeField] private bool useBlurEffects = true;
    [SerializeField] private BlurScene blurScreen;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image narrativeBeatImage;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private float quickFadeDuraton = .25f;
    [SerializeField] private float longFadeDuraton = 2f;

    [SerializeField] private InputActionAsset playerActions;
    private InputAction enterAction;

    [SerializeField] private bool resetForDebug = true;
    [SerializeField] private bool testSequence = true;
    [SerializeField] private bool sequenceIsActive = false;

    public void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;

        enterAction = playerActions.FindAction("DialogueNext");
        narrativeCardDefinitionReference.FillEmptyPrefabReferences();
        narrativeBeatImage.color = Color.clear;

        if (resetForDebug)
        {
            foreach (NarrativeCardData narrativeCard in narrativeCardDefinitionReference.narrativeCards)
            {
                narrativeCardDefinitionReference.SetNarrativeCardViewedState(narrativeCard, false);
            }
        }

        if (testSequence)
        {
            InitNewNarrativeCardsForPhaseIndex(ComputerScreenManager.currentSequence);
            if (currentNarrativeCardsData.Count > 0)
            {
                StartDisplaySequence();
            }
        }
    }

    public void Update()
    {
        if (enterAction.WasPressedThisFrame() && sequenceIsActive)
        {
            InitNewNarrativeCardsForPhaseIndex(ComputerScreenManager.currentSequence);
            if (currentNarrativeCardsData.Count > 0)
            {
                StartDisplaySequence();
            }
            else if (currentNarrativeCardsData.Count == 0 && sequenceIsActive)
            {
                cardParentRectTransform.GetComponentInChildren<NarrativeCardDisplay>().FadeCanvasGroup(false, quickFadeDuraton);
                ForceEndSequence();
            }
        }
    }

    public void StartNewSequence(Ending ending = Ending.NONE)
    {
        InitNewNarrativeCardsForPhaseIndex(ComputerScreenManager.currentSequence, ending);
        if (currentNarrativeCardsData.Count > 0)
        {
            StartDisplaySequence();
        }
    }

    public void InitNewNarrativeCardsForPhaseIndex(int phaseIndex, Ending ending = Ending.NONE)
    {
        if (currentNarrativeCardsData != null)
        {
            currentNarrativeCardsData = new List<NarrativeCardData>();
        }
        if (currentCard != null)
        {
            narrativeCardDefinitionReference.SetNarrativeCardViewedState(currentCard.narrativeCardData);
        }

        foreach (NarrativeCardData card in narrativeCardDefinitionReference.narrativeCards)
        {
            if (card.narrativePhase == phaseIndex && !card.wasShown && card.ending == ending)
            {
                currentNarrativeCardsData.Add(card);
            }
        }
    }

    Coroutine coDisplaySequence = null;
    private void StartDisplaySequence()
    {
        if (coDisplaySequence != null)
        {
            StopCoroutine(coDisplaySequence);
            coDisplaySequence = null;
        }

        coDisplaySequence = StartCoroutine(CoDisplaySequence());
    }

    private bool blurEffectsActive = false;
    private bool canvasGroupActive = false;
    bool isFadingCurrentCard = false;
    private IEnumerator CoDisplaySequence()
    {
        if (!sequenceIsActive)
        {
            if (useBlurEffects && !blurEffectsActive)
            {
                blurScreen.EnableBlur();
                blurScreen.Desaturate();
                blurEffectsActive = true;
            }

            //Debug.Log($"CanvasGroupActive is {canvasGroupActive} & canvasgroup.alpha is {canvasGroup.alpha}");
            if (!canvasGroupActive || canvasGroup.alpha < 1)
            {
                float canvasGroupStartAlpha = canvasGroup.alpha;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                float duration = longFadeDuraton * (1 - canvasGroupStartAlpha);
                float fadeInCanvasDuration = 0;
                while (fadeInCanvasDuration < duration)
                {
                    fadeInCanvasDuration += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Lerp(canvasGroupStartAlpha, 1, fadeInCanvasDuration / duration);
                    yield return null;
                }
                canvasGroupActive = true;
            }

            sequenceIsActive = true;
        }

        if (currentCard != null)
        {
            if (!isFadingCurrentCard)
            {
                isFadingCurrentCard = true;
                currentCard.FadeCanvasGroup(false, quickFadeDuraton);
                yield return new WaitForSecondsRealtime(quickFadeDuraton);
            }
            Destroy(currentCard.gameObject);
            currentCard = null;
            isFadingCurrentCard = false;
        }

        for (int i = 0; i < currentNarrativeCardsData.Count; i++)
        {
            //Debug.Log($"Display new card for '{currentNarrativeCardsData[i].messageContent}' at {Time.frameCount}");
            currentCard = Instantiate(currentNarrativeCardsData[i].prefab, cardParentRectTransform);
            currentCard.InitNarrativeCard(currentNarrativeCardsData[i]);


            ///// IMAGE HANDLING LOGIC
            //Debug.Log($"Current narrative beat sprite is {currentNarrativeCardsData[i].narrativeBeatSprite}, " +
            //    $"image alpha is {narrativeBeatImage.color.a}, " +
            //    $"next image sprite is the same as current is {narrativeBeatImage.sprite == currentNarrativeCardsData[i].narrativeBeatSprite}");
            if (currentNarrativeCardsData[i].narrativeBeatSprite != null && narrativeBeatImage.sprite != currentNarrativeCardsData[i].narrativeBeatSprite)
            {
                float narrativeBeatImageAlpha = narrativeBeatImage.color.a;
                float duration = quickFadeDuraton * narrativeBeatImage.color.a;
                float fadeInCanvasDuration = 0;
                if (narrativeBeatImage.color.a > 0)
                {
                    while (fadeInCanvasDuration < duration)
                    {
                        fadeInCanvasDuration += Time.deltaTime;
                        narrativeBeatImage.color = new Color(1,1,1, Mathf.Lerp(narrativeBeatImageAlpha, 0, fadeInCanvasDuration / duration));
                        yield return null;
                    }
                }

                narrativeBeatImage.sprite = currentNarrativeCardsData[i].narrativeBeatSprite;

                narrativeBeatImageAlpha = narrativeBeatImage.color.a;
                duration = longFadeDuraton * (1 - narrativeBeatImage.color.a);
                fadeInCanvasDuration = 0;
                while (fadeInCanvasDuration < duration)
                {
                    fadeInCanvasDuration += Time.deltaTime;
                    narrativeBeatImage.color = new Color(1, 1, 1, Mathf.Lerp(narrativeBeatImageAlpha, 1, fadeInCanvasDuration / duration));
                    yield return null;
                }
            }
            else if (currentNarrativeCardsData[i].narrativeBeatSprite == null && narrativeBeatImage.color.a > 0)
            {
                //Debug.Log($"Clear current image");
                float narrativeBeatImageAlpha = narrativeBeatImage.color.a;
                float duration = quickFadeDuraton * narrativeBeatImage.color.a;
                float fadeInCanvasDuration = 0;
                while (fadeInCanvasDuration < duration)
                {
                    fadeInCanvasDuration += Time.deltaTime;
                    narrativeBeatImage.color = new Color(1, 1, 1, Mathf.Lerp(narrativeBeatImageAlpha, 0, fadeInCanvasDuration / duration));
                    yield return null;
                }
            }
            /////

            currentCard.FadeCanvasGroup(true, fadeInDuration);

            yield return new WaitForSecondsRealtime(currentCard.narrativeCardData.cardDuration + fadeInDuration);

            //Debug.Log($"Fade out card {currentNarrativeCardsData[i].messageContent}");
            currentCard.FadeCanvasGroup(false, fadeOutDuration);

            yield return new WaitForSecondsRealtime(fadeOutDuration);
            //Debug.Log($"Destroy card {currentNarrativeCardsData[i].messageContent}");
            Destroy(currentCard.gameObject);
            currentCard = null;
        }

        if (useBlurEffects && blurEffectsActive)
        {
            blurScreen.DisableBlur();
            blurScreen.Saturate();
            blurEffectsActive = false;
        }

        if (canvasGroupActive)
        {
            float fadeOutCanvasDuration = 0;
            while (fadeOutCanvasDuration < longFadeDuraton)
            {
                fadeOutCanvasDuration += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1, 0, fadeOutCanvasDuration / longFadeDuraton);
                yield return null;
            }
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroupActive = false;
        }

        currentNarrativeCardsData = new List<NarrativeCardData>();
        sequenceIsActive = false;
        coDisplaySequence = null;
    }

    private void ForceEndSequence()
    {
        if (coDisplaySequence != null)
        {
            StopCoroutine(coDisplaySequence);
            coDisplaySequence = null;
        }

        coDisplaySequence = StartCoroutine(CoForceEndSequence());
    }

    private IEnumerator CoForceEndSequence()
    {
        if (useBlurEffects && blurEffectsActive)
        {
            blurScreen.DisableBlur();
            blurScreen.Saturate();
            blurEffectsActive = false;
        }

        if (canvasGroupActive)
        {
            float fadeOutCanvasDuration = 0;
            while (fadeOutCanvasDuration < quickFadeDuraton)
            {
                fadeOutCanvasDuration += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, fadeOutCanvasDuration / quickFadeDuraton);
                yield return null;
            }
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroupActive = false;
        }

        currentNarrativeCardsData = new List<NarrativeCardData>();
        sequenceIsActive = false;
        coDisplaySequence = null;
    }
}