using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScroll : MonoBehaviour 
{
    public MessageMananger messageManager;
    public float scrollSpeed = 1f;
    public RectTransform viewRect;
    public RectTransform textRect;
    public Text message;
    private float length{get{ return viewRect.rect.width + textRect.rect.width;}}
    private int count;
    private int messageCount;
    void Start()
    {
        count = 0;
        message.text = messageManager.messageData[messageCount].list[count];
        textRect.anchoredPosition = new Vector2(viewRect.rect.width, 0);
    }
    void Update()
    {
        if (textRect.anchoredPosition.x <  - textRect.rect.width)
        {
            textRect.anchoredPosition = new Vector2(viewRect.rect.width, 0);
            count++;
            if ( messageManager.messageData[messageCount].list.Count > count)
            {
                message.text =  messageManager.messageData[messageCount].list[count];
            }
            else
            {
                count = 0;
                messageCount++;
                if (messageManager.messageData.Count > messageCount)
                {
                    message.text = messageManager.messageData[messageCount].list[count];
                }
                else
                {
                    messageCount = 0;
                    message.text = messageManager.messageData[messageCount].list[count];
                }
            }
        }
        else
        {
            textRect.anchoredPosition -= new Vector2(scrollSpeed * Time.deltaTime, 0);
        }
    }
}
