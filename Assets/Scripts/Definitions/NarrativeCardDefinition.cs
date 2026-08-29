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
}

[CreateAssetMenu(fileName = "NarrativeCardDefinition", menuName = "Scriptable Objects/NarrativeCardDefinition")]
public class NarrativeCardDefinition : ScriptableObject
{
    [SerializeField] public List<NarrativeCardData> narrativeCards = new List<NarrativeCardData>();

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

