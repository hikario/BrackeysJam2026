using System;
using UnityEngine;
using FMODUnity;

[Serializable]
public enum EvidenceType
{
    Photo = 0,
    Audio = 1
}

[Serializable]
public class Evidence
{
    [SerializeField] public string evidenceName;
    [SerializeField] public EvidenceType evidenceType;
    [SerializeField] public bool catEvidence = false;
    [SerializeField] public bool evidenceIsCollected = false;
    [SerializeField] public bool flaggedAsRelevant = false;
    [SerializeField] public int evidencePhase;
    [SerializeField] public Sprite evidenceSprite;
    [SerializeField] public EventReference evidenceAudioEvent;
    [SerializeField] public float audioLength;
    [SerializeField] private string requiredPurchasableID;

    public bool CheckForRequiredPurchasable()
    {
        foreach (PurchasableObject purchasableObject in ComputerScreenManager.instance.purchasableObjectDefinitionReference.purchasableObjects)
        {
            if (requiredPurchasableID == purchasableObject.objectName && purchasableObject.isPlaced)
            {
                return true;
            }
        }

        return false;
    }
}
