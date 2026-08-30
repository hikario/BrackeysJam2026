using System.Collections;
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
    [SerializeField] private Button sendButton;
    [SerializeField] public Image newMessageAlertImage;
    [SerializeField] public TextMeshProUGUI newMessageCountText;
    [SerializeField] private RectTransform messageContainer;
    [SerializeField] private float timeToWaitForResponse = 1f;
    [SerializeField] private Messages queuedMessage;
    private bool isOpen = false;

    public void OpenWidget()
    {
        if (isOpen)
        {
            isOpen = false;
            return;
        }

        if (queuedMessage != null)
        {
            nextMessageText.text = queuedMessage.messageText;
        }
        else
        {
            nextMessageText.text = "";
        }

        newMessageCountText.text = "";
        newMessageAlertImage.enabled = false;
        foreach (Messages message in ComputerScreenManager.instance.messagesDefinitionReference.messages)
        {
            if (message.messagePhase == ComputerScreenManager.currentSequence && message.isPlayerMessage
                && !message.isSent && !messagesToDisplay.Contains(message) && !message.isResponse)
            {
                QueueNextPlayerMessage(message);
                break;
            }
        }

        Invoke("RebuildLayout", .1f);
    }

    public void QueueNextPlayerMessage(Messages message)
    {
        nextMessageText.GetComponentInChildren<TextMeshProUGUI>().text = message.messageText;

        queuedMessage = message;
    }

    public void SendNextRoommateMessages()
    {
        int newMessageCount = 0;
        foreach (Messages message in ComputerScreenManager.instance.messagesDefinitionReference.messages)
        {
            if (message.messagePhase == ComputerScreenManager.currentSequence && !message.isPlayerMessage
                && !messagesToDisplay.Contains(message) && !message.isResponse)
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
        if (queuedMessage == null)
        {
            return;
        }

        //foreach (Messages message in ComputerScreenManager.instance.messagesDefinitionReference.messages)
        //{
        //    if (message.messagePhase == ComputerScreenManager.currentSequence && message.isPlayerMessage
        //        && !message.isSent && !messagesToDisplay.Contains(message))
        //    {
        //        GameObject newMessage = Instantiate(playerMessagePrefab, messageContainer);
        //        newMessage.GetComponentInChildren<TextMeshProUGUI>().text = message.messageText;
        //        ComputerScreenManager.instance.messagesDefinitionReference.MarkMessageAsSent(message);
        //        messagesToDisplay.Add(message);
        //        if (message.hasResponse)
        //        {
        //            SendRoommateResponse(message.messageText);
        //        }
        //        nextMessageText.text = "";
        //        break;
        //    }
        //}

        GameObject newMessage = Instantiate(playerMessagePrefab, messageContainer);
        newMessage.GetComponentInChildren<TextMeshProUGUI>().text = queuedMessage.messageText;
        ComputerScreenManager.instance.messagesDefinitionReference.MarkMessageAsSent(queuedMessage);
        messagesToDisplay.Add(queuedMessage);

        if (queuedMessage.hasResponse)
        {
            SendRoommateResponse(queuedMessage.messageText);
        }
        queuedMessage = null;
        nextMessageText.text = "";

        foreach (Messages message in ComputerScreenManager.instance.messagesDefinitionReference.messages)
        {
            if (message.messagePhase == ComputerScreenManager.currentSequence && message.isPlayerMessage
                && !message.isSent && !messagesToDisplay.Contains(message) && !message.isResponse)
            {
                QueueNextPlayerMessage(message);
                break;
            }
        }

        Invoke("RebuildLayout", .1f);
    }
    private void RebuildLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(messageContainer);
    }

    private void SendRoommateResponse(string messageToRespondTo)
    {
        Messages roommateMessageToSend = null;
        foreach (Messages message in ComputerScreenManager.instance.messagesDefinitionReference.messages)
        {
            if (message.isResponseTo == messageToRespondTo)
            {
                roommateMessageToSend = message;
            }
        }

        StartCoroutine(CoSendRoommateResponse(roommateMessageToSend));
    }


    private IEnumerator CoSendRoommateResponse(Messages message)
    {
        Debug.Log($"CoSendRoommateResponse");
        yield return new WaitForSecondsRealtime(timeToWaitForResponse);

        Debug.Log($"CoSendRoommateResponse Time Elapsed");
        GameObject newMessage = Instantiate(responseMessagePrefab, messageContainer);
        newMessage.GetComponentInChildren<TextMeshProUGUI>().text = message.messageText;
        messagesToDisplay.Add(message);

        if (!containerObject.activeInHierarchy)
        {
            newMessageAlertImage.enabled = true;
            newMessageCountText.text = 1.ToString();
        }

        if (message.hasResponse)
        {
            foreach (Messages nextMessage in ComputerScreenManager.instance.messagesDefinitionReference.messages)
            {
                if (nextMessage.messageText == message.responseIs)
                {
                    QueueNextPlayerMessage(nextMessage);
                }
            }
        }

        Invoke("RebuildLayout", .1f);
    }
}
