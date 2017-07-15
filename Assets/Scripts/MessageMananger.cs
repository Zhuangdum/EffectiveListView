using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MessageMananger", menuName = "DataManager")]
public class MessageMananger : ScriptableObject 
{
    public List<MessageData> messageData;
}
