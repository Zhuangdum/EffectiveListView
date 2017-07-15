using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestManager : MonoBehaviour 
{
    public static TestManager instance;
    public MessageMananger messageManager;
    public ScrollViewManager uiManager;
    void Start()
    {
        instance = this;
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(200, 400, 100, 50), "SET000"))
        {
            uiManager.Init("000", messageManager.messageData.Find(s => s.id == "000").list);
        }
        if (GUI.Button(new Rect(300, 400, 100, 50), "SET111"))
        {
            uiManager.Init("111", messageManager.messageData.Find(s => s.id == "111").list);
        }
        if (GUI.Button(new Rect(400, 400, 100, 50), "SET222"))
        {
            uiManager.Init("222", messageManager.messageData.Find(s => s.id == "222").list);
        }
        if (GUI.Button(new Rect(500, 400, 100, 50), "SET333"))
        {
            uiManager.Init("333", messageManager.messageData.Find(s => s.id == "333").list);
        }
    }
}
