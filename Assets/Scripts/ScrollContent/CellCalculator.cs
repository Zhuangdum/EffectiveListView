using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellCalculator : MonoBehaviour 
{
	public Text text;
	private RectTransform toolRect;
	void Start()
	{
		toolRect = GetComponent<RectTransform>();
	}

	public float CalculateHeight(string str)
	{
		text.text = str;
//		Debug.Log("text height: "+GetComponent<RectTransform>().rect.height);
		return toolRect.rect.height;
	}
}