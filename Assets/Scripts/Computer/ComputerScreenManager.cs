using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[Serializable]
public class ComputerScreen : MonoBehaviour
{
    [SerializeField] private Animator screenAnimator;

    public void CloseScreen()
    {
        screenAnimator.SetBool("expand", false);
        Invoke("DisableScreen", .5f);
    }

    private void DisableScreen()
    {
        this.gameObject.SetActive(false);
    }
}

public class ComputerScreenManager : MonoBehaviour
{
    public static ComputerScreenManager instance;
    [SerializeField] private ComputerScreen[] screens;
    [SerializeField] public PurchasableObjectDefinition purchasableObjectDefinitionReference;
    [SerializeField] public EvidenceDefinition evidenceDefinitionReference;
    [SerializeField] public static int currentSequence = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    public void ProgressToNextSequence()
    {
        currentSequence++;
        Debug.Log($"Progress to sequence {currentSequence}");
    }
}
