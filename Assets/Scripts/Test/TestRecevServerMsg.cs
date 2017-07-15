using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestRecevServerMsg : MonoBehaviour 
{
    public InputField inputMsg;
    public InputField inputId;
    public void ClickSendToRoom()
    {
        TestManager.instance.messageManager.messageData.Find(s => s.id == inputId.text).list.Add(inputMsg.text);
        TestManager.instance.uiManager.ScrollMgrReceiveMessage(inputId.text, inputMsg.text);
    }
}
