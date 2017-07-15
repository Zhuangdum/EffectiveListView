﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class TabsManager : MonoBehaviour 
{
    public Text title;
	[Header("TabSelectColor")]
	public Color normalColor;
	public Color selectedColor;
	[Header("Rect")]
	public RectTransform content;
	public RectTransform tabPrefab;
	private LinkedList<TabInfo> fixedLinklist = new LinkedList<TabInfo>();
	private LinkedList<TabInfo> flexibleLinklist = new LinkedList<TabInfo>();

    public ScrollViewController messageContent;
    public TabInfo currentTab { get; set;}
	private int index;
    public void Init(string tabID, List<string> message)
    {
        AddTab(tabID, message);
        SetToTab(tabID);
        messageContent.Init(currentTab.tabID, currentTab.messageLog);
    }
	#region Add Tabs
    private void AddTab(string tabID, List<string> message, TabType type = TabType.Flexible)
	{
		if (type == TabType.Fixed)
		{
			if (fixedLinklist.Count > 1)
			{
				Debug.Log("fixed tab overflow");
				return;
			}
            TabInfo info = CreateTab(tabID, message);
			info.rect.SetSiblingIndex(fixedLinklist.Count);
			fixedLinklist.AddFirst(info);
		}
		else
		{
			if (flexibleLinklist.Count > 4-fixedLinklist.Count)
			{
				AddToPool(flexibleLinklist.Last.Value.rect);
				flexibleLinklist.Last.Value.Clear();
				flexibleLinklist.RemoveLast();
			}
            TabInfo info = CreateTab(tabID, message);
			info.rect.SetSiblingIndex(fixedLinklist.Count);
			flexibleLinklist.AddFirst(info);
		}
	}
    TabInfo CreateTab(string tabID, List<string> message)
	{
		RectTransform tab = GetTabFromPool();
		tab.SetParent(content);
		tab.localScale = Vector3.one;
		TabInfo info = new TabInfo();
        info.messageLog = message;
		info.tabBehavior = tab.GetComponent<TabBehavior>();
        info.tabBehavior.txt.text = tabID;//temp set as data
		info.tabBehavior.SetTabManager(this);
		info.tabBehavior.SetState(TabState.Normal);
        info.tabBehavior.id = tabID;
        info.tabID = tabID;
		info.rect = tab;

		index++;
		return info;
	}
	#endregion

	#region Set tab state
	private TabBehavior selectTab{ get; set;}
	public void SwitchTabState(TabBehavior tab)
	{
		if (selectTab == null)
		{
			selectTab = tab;
			selectTab.SetState(TabState.Selected);
		}
		else if (selectTab != tab)
		{
			selectTab.SetState(TabState.Normal);
			selectTab = tab;
			selectTab.SetState(TabState.Selected);
		}
		else
		{
			//do nothing
		}
	}
    private void SetToTab(string tabID)
	{
        TabInfo info = flexibleLinklist.FirstOrDefault(e => e.tabID == tabID);
        if (info != null)
        {
            SetTitle(tabID);
            currentTab = info;
            SwitchTabState(info.tabBehavior);
        }
		else
			Debug.LogError("can not find convId");
	}
	private void ReSetSelectTab()
	{
		if (selectTab != null)
		{
			selectTab.SetState(TabState.Normal);
			selectTab = null;
		}
	}
	#endregion

	#region receive message
    public void ReceiveMessage(string id, string message)
	{
        TabInfo tabInfo = flexibleLinklist.FirstOrDefault(e => e.tabID == id);
        if (tabInfo != null && id == currentTab.tabID)
		{
			tabInfo.rect.SetSiblingIndex(fixedLinklist.Count);
//            tabInfo.ReceiveMessge(id, message);
            messageContent.ReceiveMessge(message, ItemLayoutType.Left);
		}
		else
		{
			Debug.LogError("id is not current or can not find id");
		}
	}
	#endregion

	#region Tab Pool
    private LinkedList<RectTransform> avaliableTabLinklist = new LinkedList<RectTransform>();
	private void AddToPool(RectTransform rect)
	{
		rect.gameObject.SetActive(false);
		avaliableTabLinklist.AddFirst(rect);
	}
	private RectTransform GetTabFromPool()
	{
		if (avaliableTabLinklist.Count > 0)
		{
			RectTransform rect = avaliableTabLinklist.First.Value;
			rect.gameObject.SetActive(true);
			avaliableTabLinklist.RemoveFirst();
			return rect;
		}
		else
		{
			RectTransform rect = Instantiate(tabPrefab);
			return rect;
		}
	}
	#endregion

    #region SetContent Info
    public void SetContentInfo(string id)
    {
        SetToTab(id);
        messageContent.SetCotentData(TestManager.instance.messageManager.messageData.Find(s=>s.id == id).list);
    }
    #endregion

    private void SetTitle(string id)
    {
        title.text = id;
    }
	#region at the end and first
	private void OnEnable()
	{
		
	}
	private void OnDisable()
	{
		if (selectTab != null)
			selectTab.SetState(TabState.Normal);
	}
	#endregion

	#region Test Code
	string id1 = "";
	string id2 = "";
	void OnGUI()
	{
	}
	#endregion
}