using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Tips : MonoBehaviour
{
    [NonSerialized] public Action onShow; 
    [NonSerialized] public Action onHide;
    public Image image;
	[HideInInspector]public int registerCount;
	public void Init()
	{
		image.enabled = false;
		registerCount = 0;
	}
    public void Show()
    {
		image.enabled = true;
		if (onShow != null)
			onShow();
    }

    public void Hide()
    {
		image.enabled = false;
		if (onHide != null)
			onHide();
    }
	public void AddEvent(Action onCallBack, TipsType type)
	{
		if (type == TipsType.Open)
		{
			onShow += onCallBack;
		}
		else
		{
			onHide += onCallBack;
		}
	}
	public void RemoveEvent(Action onCallBack, TipsType type)
	{
		if (type == TipsType.Open)
		{
			onShow -= onCallBack;
		}
		else
		{
			onHide -= onCallBack;
		}
	}
	public void AddRegisterCount()
	{
		registerCount++;
	}
	public void SubRegisterCount()
	{
		if(registerCount > 0)
			registerCount--;
	}
	public void Clear()
	{
		onShow = null;
		onHide = null;
		registerCount = 0;
	}
}
