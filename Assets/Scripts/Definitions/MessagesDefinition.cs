using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Messages
{
    public string messageText;
    public int messagePhase;
    public bool isPlayerMessage;
    public bool isSent;
    public bool isResponse;
    public string isResponseTo;
    public bool hasResponse;
    public string responseIs;
}

[CreateAssetMenu(fileName = "MessagesDefinition", menuName = "Scriptable Objects/MessagesDefinition")]
public class MessagesDefinition : ScriptableObject
{
    [SerializeField] public List<Messages> messages = new List<Messages>();

    public void MarkMessageAsSent(Messages _message)
    {
        foreach(Messages message in messages)
        {
            if (_message == message)
            {
                message.isSent = true;
            }
        }
    }
}