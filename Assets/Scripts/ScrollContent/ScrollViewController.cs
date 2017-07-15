using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum ItemLayoutType
{
	Left,
	Right,
	Center
}
public class ScrollViewController : MonoBehaviour 
{
	[Header("Tool")]
	public CellCalculator cellTool;

	[Header("Item Prefab")]
	public RectTransform leftItemPrefab;
	public RectTransform rightItemPrefab;
	public RectTransform centerItemPrefab;

	[Header("Contents")]
	public float top = 20f;
	public float bottom = 20f;
	public float interval = 20f;
	public RectTransform content;

	[Header("ScrollRect")]
	public ScrollRect scrollRect;
	public RectTransform viewPort;

    //
    public int itemNode = 1;
    //manager each item info
    public Dictionary<int, ItemInfo> itemDic = new Dictionary<int, ItemInfo>();
    //manager total prefab
    public LinkedList<RectTransform> avaliableLeftItem = new LinkedList<RectTransform>();
    public LinkedList<RectTransform> avaliableRightItem = new LinkedList<RectTransform>();
    public LinkedList<RectTransform> avaliableCenterItem = new LinkedList<RectTransform>();

    #region add info to itemdic
    public void AddToItemDic(ItemInfo item)
    {
        item.index = itemNode;
        itemDic.Add(itemNode, item);
        itemNode++;
    }
    #endregion

    private Coroutine _Co_RefreshItem;
    public Coroutine Co_RefreshItem
    { 
        get{ return _Co_RefreshItem;}
        set 
        {
            if (_Co_RefreshItem != null)
            {
                StopCoroutine(_Co_RefreshItem);
            }
            _Co_RefreshItem = value;
        }
    }
    public void Init(string tabID, List<string> message)
	{
		scrollRect.onValueChanged.AddListener(OnScrollValueChange);
        SetCotentData(message, delegate {
            Debug.Log("SetContentData Success");
        });
	}
    private void InitContentInfo()
    {
        RecycleItemDic();
        itemNode = 1;
        itemDic.Clear();
        scrollRect.content.sizeDelta = new Vector2(content.rect.width, 0);
    }
    #region SetData
    public void SetCotentData(List<string> messageList, Action OnCallback)
    {
        Co_RefreshItem = StartCoroutine(RefreshItem());
        InitContentInfo();
        scrollRect.content.sizeDelta = new Vector2(content.rect.width, 0);
        StartCoroutine(ReceiveMessage(messageList, OnCallback));
    }
    #endregion
    void RecycleItemDic()
    {
        foreach (var item in itemDic)
        {
            if (item.Value.cell != null)
            {
                RecycleRect(item.Value.cell, item.Value.type);
            }
        }
    }
//	void LateUpdate()
    IEnumerator RefreshItem()
	{
        while (true)
        {
            foreach (var item in itemDic)
            {
                RefreshLinklist(item.Value);
            }
            yield return null;
        }
	}


    void OnScrollValueChange(Vector2 value)
    {
    }

	#region refresh showlinklist
	private void RefreshLinklist(ItemInfo item)
	{
		if (IsInContent(item))
		{
			if (item.cell == null)
			{
				item.cell = GetFromItemPool(item.type);
				SetRectData(item.cell, item);
			}
		}
		else
		{
			if (item.cell != null)
			{
				RecycleRect(item.cell, item.type);
				item.cell = null;
			}
		}
	}

	private void SetRectData(RectTransform rect, ItemInfo item)
	{
		rect.anchoredPosition = item.anchorPos;
		rect.GetComponent<ItemUIInfo>().messagetext.text = item.contentTxt;
	}
	#endregion

	#region Check if a item is in the content
	private bool IsInContent(ItemInfo item)
	{
		return ((item.bottomDistance <= -content.anchoredPosition.y) 
			&& (item.topDistance > (-content.anchoredPosition.y - GetViewPortInfo().height)));
	}
	#endregion

	#region item pool
	private void RecycleRect(RectTransform rect, ItemLayoutType type)
	{
		rect.gameObject.SetActive(false);
		if (type == ItemLayoutType.Left)
		{
			avaliableLeftItem.AddFirst(rect);
		}
		else if(type == ItemLayoutType.Right)
		{
			avaliableRightItem.AddFirst(rect);
		}else
		{
			avaliableCenterItem.AddFirst(rect);
		}
	}
	private RectTransform GetFromItemPool(ItemLayoutType type)
	{
		RectTransform rect;
		if (type == ItemLayoutType.Left)
		{
			if (avaliableLeftItem.Count > 0)
			{
				rect = avaliableLeftItem.First.Value;
				avaliableLeftItem.RemoveFirst();
			}
			else
			{
				rect = Instantiate(leftItemPrefab);
				//set rect
				rect.transform.SetParent(content);
				rect.localScale = Vector3.one;
			}
		}
		else if (type == ItemLayoutType.Right)
		{
			if (avaliableRightItem.Count > 0)
			{
				rect = avaliableRightItem.First.Value;
				avaliableRightItem.RemoveFirst();
			}
			else
			{
				rect = Instantiate(rightItemPrefab);
				rect.transform.SetParent(content);
				rect.localScale = Vector3.one;
			}
		}
		else
		{
			if (avaliableCenterItem.Count > 0)
			{
				rect = avaliableCenterItem.First.Value;
				avaliableCenterItem.RemoveFirst();
			}
			else
			{
				rect = Instantiate(centerItemPrefab);
				rect.transform.SetParent(content);
				rect.localScale = Vector3.one;
			}
		}
		rect.gameObject.SetActive(true);
		return rect;
	}
	#endregion

	#region handle receive message
    IEnumerator ReceiveMessage(List<string> msgList, Action OnCallback, ItemLayoutType type = ItemLayoutType.Left)
    {
        for (int i = 0; i < msgList.Count; i++)
        {
            CalcItemlHeight(msgList[i]);
            yield return null;
            Internal_ReceiveMessage(msgList[i], type);
        }
        SetContentToBottom();
        if (OnCallback != null)
            OnCallback();
        yield return null;
    }
	public void ReceiveMessge(string str, ItemLayoutType type)
	{
		StartCoroutine(RceiveCoroutine(str, type));
	}
	IEnumerator RceiveCoroutine(string str , ItemLayoutType type = ItemLayoutType.Left)
	{
		CalcItemlHeight(str);
		yield return null;
		Internal_ReceiveMessage(str, type);
	}

	private void Internal_ReceiveMessage(string msg, ItemLayoutType type = ItemLayoutType.Left)
	{
		ItemInfo itemInfo = GenerateItemInfo(type, msg);
		SetContentSize(itemInfo);
	}
	#endregion

	#region Generate Item recttransform
	private ItemInfo GenerateItemInfo(ItemLayoutType type, string msg)
	{
		ItemInfo info = new ItemInfo();
		info.type = type;
		info.contentTxt = msg;
		info.height = CalcItemlHeight(info.contentTxt);
		info.anchorPos = -new Vector2(0, content.rect.height);
		info.topDistance = info.anchorPos.y;
		info.bottomDistance = info.topDistance - info.height;
		AddToItemDic(info);
		return info;
	}
	#endregion

	#region calculate cell height
	private float CalcItemlHeight(string txt)
	{
		return cellTool.CalculateHeight(txt);
	}
	#endregion

	#region calculate content size
	private void SetContentSize(ItemInfo info)
	{
		//test scroll auto
		if(Mathf.Abs(content.anchoredPosition.y - content.rect.height + viewPort.rect.height) < 10f)
		{
			SetContentToBottom(new Vector2(0, content.anchoredPosition.y + info.height));
		}
		else
		{
			
		}

		Vector2 delta = scrollRect.content.sizeDelta;
		scrollRect.content.sizeDelta += new Vector2(0, info.height);
	}
	#endregion

	#region setContent pos
	private void SetContentToBottom(Vector2 targetPos)
	{
		content.anchoredPosition = targetPos;
	}
    private void SetContentToBottom()
    {
        SetContentToBottom(new Vector2(0, content.anchoredPosition.y));
    }
	#endregion

	#region get viewport height and position
	private ViewPortInfo GetViewPortInfo()
	{
		ViewPortInfo info = new ViewPortInfo();
		info.height = viewPort.rect.height;
		info.top = viewPort.anchoredPosition.y;
		info.bottom = viewPort.anchoredPosition.y-viewPort.rect.height;
		info.anchorPos = viewPort.anchoredPosition;
		return info;
	}
	#endregion
	#region print dictionary value
	private void PrintDic()
	{
		foreach (var item in itemDic)
		{
			Debug.Log(item.Value.anchorPos+"   index:"+item.Value.index+"   uiinfo text:  "+item.Value.contentTxt);
		}
	}
	#endregion
	#region print dictionary avaliable item
	private void PrintAvaliableDic()
	{
		foreach (var item in avaliableLeftItem)
		{
			Debug.Log("[" + item.anchoredPosition + "]");
		}
	}
	#endregion
	#region test
	void OnGUI()
	{
        if (GUI.Button(new Rect(10, 0, 100, 50), "count"))
        {
            Debug.Log(itemDic.Count);
        }
		if (GUI.Button(new Rect(10, 100, 100, 50), "center"))
		{
			ReceiveMessge("centerxxxxxxxxxxxxxxxxxxxxxxx", ItemLayoutType.Center);
		}
		if (GUI.Button(new Rect(10, 200, 100, 50), "left"))
		{
			ReceiveMessge("leftxxxxxxxxxxxxxxxxxxxxxxx", ItemLayoutType.Left);
		}
		if (GUI.Button(new Rect(10, 300, 100, 50), "right"))
		{
			ReceiveMessge("rightxxxxxxxxxxxxxxxxxxxxxxx", ItemLayoutType.Right);
		}
		if (GUI.Button(new Rect(10, 400, 100, 50), "bottom"))
		{
			SetContentToBottom(new Vector2(0, content.rect.height-viewPort.rect.height));
		}
	}
	#endregion
}