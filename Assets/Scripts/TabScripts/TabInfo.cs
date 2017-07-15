using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TabType
{
	Fixed,
	Flexible
}

public enum TabState
{
	Selected,
	Normal
}
public enum TipsType
{
	Open,
	Close
}

public class TabInfo
{
    public string tabID;
	public TabBehavior tabBehavior;
	public  RectTransform rect;
    public List<string> messageLog;
	public void Clear()
	{
		tabBehavior = null;
		rect = null;
	}
}

