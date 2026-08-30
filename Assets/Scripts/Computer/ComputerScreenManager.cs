using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using FMODUnity;

[Serializable]
public class ComputerScreen : MonoBehaviour
{
    [SerializeField] private Animator screenAnimator;
    [SerializeField] public GameObject containerObject;
    [SerializeField] public EventReference openWindowAudioEvent;
    [SerializeField] public EventReference closeWindowAudioEvent;

    public void ToggleScreenVisibility()
    {
        if (containerObject.activeInHierarchy)
        {
            CloseScreen();
        }
        else
        {
            openWindowAudioEvent.PlayOneShot();
            screenAnimator.SetBool("expand", true);
            containerObject.SetActive(true);
        }
    }

    public void CloseScreen()
    {
        closeWindowAudioEvent.PlayOneShot();
        screenAnimator.SetBool("expand", false);
        Invoke("DisableScreen", .5f);
    }

    private void DisableScreen()
    {
        containerObject.SetActive(false);
    }
}


// Really more of a game controller at this point my b guys
public class ComputerScreenManager : MonoBehaviour
{
    public static ComputerScreenManager instance;

    [SerializeField] public static int currentSequence = 0;
    [SerializeField] private float startingMoney = 436f;
    [SerializeField] private float currentMoney;
    [SerializeField] private bool resetOnPlay = true;

    [Header("Screens")]
    [SerializeField] public PurchaseScreenController purchaseScreenController;
    [SerializeField] public EvidenceScreenController evidenceScreenController;
    [SerializeField] public MessengerScreenController messengerScreenController;
    [SerializeField] public BankingWidgetController bankingScreenController;

    [Header("Definition Refs")]
    [SerializeField] public PurchasableObjectDefinition purchasableObjectDefinitionReference;
    [SerializeField] public EvidenceDefinition evidenceDefinitionReference;
    [SerializeField] public MessagesDefinition messagesDefinitionReference;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;

        if (resetOnPlay)
        {
            currentMoney = startingMoney;
            foreach (PurchasableObject purchasableObject in purchasableObjectDefinitionReference.purchasableObjects)
            {
                purchasableObject.isPurchased = false;
                purchasableObject.isPlaced = false;
            }
            foreach (Evidence evidence in evidenceDefinitionReference.evidence)
            {
                //evidence.evidenceIsCollected = false;
                evidence.flaggedAsRelevant = false;
            }
            foreach (Messages message in messagesDefinitionReference.messages)
            {
                message.isSent = false;
            }
        }

        ProgressToNextSequence(0);
    }

    public void ProgressToNextSequence(int sequenceNumber)
    {
        currentSequence = sequenceNumber;
        messengerScreenController.SendNextRoommateMessages();
        evidenceScreenController.UpdateCollectedEvidence();
        purchaseScreenController.UpdatePurchasableObjects();
        bankingScreenController.UpdateCurrentMoneyTextDisplay();
    }

    public void UpdateCurrentMoney(float amountToAdd)
    {
        currentMoney += amountToAdd;
    }

    public float GetCurrentMoney()
    {
        return currentMoney;
    }
}
