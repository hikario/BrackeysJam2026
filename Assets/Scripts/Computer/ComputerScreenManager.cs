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
    [SerializeField] private ComputerScreen[] screens;
    [SerializeField] public static PurchasableObjectDefinition purchasableObjectDefinitionReference;
    [SerializeField] public static int currentSequence = 0;

    public void ProgressToNextSequence()
    {
        currentSequence++;
        Debug.Log($"Progress to sequence {currentSequence}");
    }
}
