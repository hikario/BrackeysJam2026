using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessengerScreenController : ComputerScreen
{
    private List<Messages> messagesToDisplay = new List<Messages>();
    [SerializeField] private GameObject playerMessagePrefab;
    [SerializeField] private GameObject responseMessagePrefab;
    [SerializeField] private TextMeshProUGUI nextMessageText;
    [SerializeField] public Image newMessageAlertImage;
    [SerializeField] public TextMeshProUGUI newMessageCountText;
    [SerializeField] private RectTransform messageContainer;

    private void OnEnable()
    {
        nextMessageText.text = "";
        newMessageCountText.text = "";
        newMessageAlertImage.enabled = false;
        foreach (Messages message in ComputerScreenManager.instance.messagesDefinitionReference.messages)
        {
            if (message.messagePhase == ComputerScreenManager.currentSequence && message.isPlayerMessage
                && !message.isSent && !messagesToDisplay.Contains(message))
            {
                nextMessageText.GetComponentInChildren<TextMeshProUGUI>().text = message.messageText;
                break;
            }
        }

        Invoke("RebuildLayout", .1f);
    }

    public void SendNextRoommateMessages()
    {
        int newMessageCount = 0;
        foreach (Messages message in ComputerScreenManager.instance.messagesDefinitionReference.messages)
        {
            if (message.messagePhase == ComputerScreenManager.currentSequence && !message.isPlayerMessage
                && !messagesToDisplay.Contains(message))
            {
                GameObject newMessage = Instantiate(responseMessagePrefab, messageContainer);
                newMessage.GetComponentInChildren<TextMeshProUGUI>().text = message.messageText;
                messagesToDisplay.Add(message);
                newMessageCount++;
            }
        }

        if (newMessageCount > 0)
        {
            newMessageAlertImage.enabled = true;
            newMessageCountText.text = newMessageCount.ToString();
        }
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
                nextMessageText.text = "";
                break;
            }
        }

        foreach (Messages message in ComputerScreenManager.instance.messagesDefinitionReference.messages)
        {
            if (message.messagePhase == ComputerScreenManager.currentSequence && message.isPlayerMessage
                && !message.isSent && !messagesToDisplay.Contains(message))
            {
                nextMessageText.GetComponentInChildren<TextMeshProUGUI>().text = message.messageText;
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
