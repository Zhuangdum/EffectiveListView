using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ViewPortInfo
{
	public float height;
	public float top;
	public float bottom;
	public Vector2 anchorPos;
}

public class ItemInfo
{
	public int index;
	public RectTransform cell;
	public string contentTxt;
	public ItemLayoutType type;
	public float topDistance;
	public float bottomDistance;
	public float height;
	public Vector2 anchorPos;
}