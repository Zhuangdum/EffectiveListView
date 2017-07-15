using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Tips : MonoBehaviour
{
    [NonSerialized] public Action onShow; 
    [NonSerialized] public Action onHide;
	public RawImage rawImage;
	[HideInInspector]public int registerCount;
	public void Init()
	{
		rawImage.enabled = false;
		registerCount = 0;
	}
    public void Show()
    {
		rawImage.enabled = true;
		if (onShow != null)
			onShow();
    }

    public void Hide()
    {
		rawImage.enabled = false;
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
