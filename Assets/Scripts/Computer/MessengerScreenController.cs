using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessengerScreenController : ComputerScreen
{
    private List<Messages> messagesToDisplay = new List<Messages>();
    [SerializeField] private GameObject playerMessagePrefab;
    [SerializeField] private GameObject responseMessagePrefab;
    [SerializeField] private RectTransform messageContainer;

    private void OnEnable()
    {
        foreach (Messages message in ComputerScreenManager.instance.messagesDefinitionReference.messages)
        {
            if (message.messagePhase == ComputerScreenManager.currentSequence && !message.isPlayerMessage && !messagesToDisplay.Contains(message))
            {
                GameObject newMessage = Instantiate(responseMessagePrefab, messageContainer);
                newMessage.GetComponentInChildren<TextMeshProUGUI>().text = message.messageText;
                messagesToDisplay.Add(message);
            }
        }
        Invoke("RebuildLayout", .1f);
    }

    public void SendNextMessage()
    {
        foreach (Messages message in ComputerScreenManager.instance.messagesDefinitionReference.messages)
        {
            if (message.messagePhase == ComputerScreenManager.currentSequence && message.isPlayerMessage
                && !message.isSent && !messagesToDisplay.Contains(message))
            {
                GameObject newMessage = Instantiate(playerMessagePrefab, messageContainer);
                newMessage.GetComponentInChildren<TextMeshProUGUI>().text = message.messageText;
                ComputerScreenManager.instance.messagesDefinitionReference.MarkMessageAsSent(message);
                messagesToDisplay.Add(message);
                break;
            }
        }
        Invoke("RebuildLayout", .1f);
    }
    private void RebuildLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(messageContainer);
    }
}
