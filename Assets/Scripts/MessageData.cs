using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Message", menuName = "Data")]
public class MessageData : ScriptableObject 
{
//    public LinkedList<string> messageLinklist = new LinkedList<string>();
    public string id;
    public List<string> list;
}
