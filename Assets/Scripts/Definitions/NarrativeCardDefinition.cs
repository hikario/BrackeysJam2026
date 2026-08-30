using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class NarrativeCardData
{
    public string messageContent;
    public bool wasShown;

    public int narrativePhase;
    public int narrativeOrder;
    public float cardDuration;

    public Sprite narrativeBeatSprite;

    public NarrativeCardDisplay prefab;
    public Ending ending;
}

[CreateAssetMenu(fileName = "NarrativeCardDefinition", menuName = "Scriptable Objects/NarrativeCardDefinition")]
public class NarrativeCardDefinition : ScriptableObject
{
    [SerializeField] public List<NarrativeCardData> narrativeCards = new List<NarrativeCardData>();
    public NarrativeCardDisplay defaultNarrativeCardPrefab;

    public void FillEmptyPrefabReferences()
    {
        foreach (NarrativeCardData card in narrativeCards)
        {
            if (card.prefab == null)
            {
                card.prefab = defaultNarrativeCardPrefab;
            }
        }
    }

    public void SetNarrativeCardViewedState(NarrativeCardData _card, bool _wasViewed = true)
    {
        foreach (NarrativeCardData card in narrativeCards)
        {
            if (_card == card)
            {
                card.wasShown = _wasViewed;
            }
        }
    }
}

