using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestManager : MonoBehaviour 
{
    public static TestManager instance;
    public MessageMananger messageManager;
    public ScrollViewManager uiManager;
    Coroutine testCo;
    void Start()
    {
        instance = this;

//        testCo = StartCoroutine(TestCoroutine());

    }
    IEnumerator TestCoroutine()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1);
            Debug.Log(Time.time);
        }
        yield return null;
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(200, 400, 100, 50), "SET000"))
        {
            uiManager.Init("000", TrimList(messageManager.messageData.Find(s => s.id == "000").list));
        }
        if (GUI.Button(new Rect(300, 400, 100, 50), "SET111"))
        {
            uiManager.Init("111", TrimList(messageManager.messageData.Find(s => s.id == "111").list));
        }
        if (GUI.Button(new Rect(400, 400, 100, 50), "SET222"))
        {
            uiManager.Init("222", TrimList(messageManager.messageData.Find(s => s.id == "222").list));
        }
        if (GUI.Button(new Rect(500, 400, 100, 50), "SET333"))
        {
            uiManager.Init("333", TrimList(messageManager.messageData.Find(s => s.id == "333").list));
        }
    }
    public static List<string> TrimList(List<string> list)
    {
        List<string> newList = new List<string>();
        for (int i = 0; i < list.Count; i++)
        {
            if (!string.IsNullOrEmpty(list[i]))
            {
                newList.Add(list[i]);
            }
        }
        return newList;
    }
}
