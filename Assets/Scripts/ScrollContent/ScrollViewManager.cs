using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewManager : MonoBehaviour 
{
	[Header("ScrollController")]
	public ScrollViewController scrollViewController;
    [Header("TabManager")]
    public TabsManager tabManager;
	public InputField inputField;

    public void Init(string id, List<string> list)
    {
        tabManager.Init(id, list);
    }

	#region Refresh Channel
	public void OnRefreshClicked()
	{
		Debug.Log("Refresh success");
	}
	#endregion

	#region onclickReceive Test 
    public void ScrollMgrReceiveMessage(string id,string msg, ItemLayoutType type)
	{
		//TODO
		if (type == ItemLayoutType.Right)
		{
			type = ItemLayoutType.Right;
		}
		else
		{
			type = ItemLayoutType.Left;
		}
        tabManager.ReceiveMessage(id, msg);
	}
	#endregion

	#region on click send button
	public void OnClickSendMsg()
	{
		if (!string.IsNullOrEmpty(inputField.text))
		{
			Debug.Log("input field:  "+inputField.text);
            //send message to server
            tabManager.ReceiveMessage("000", inputField.text);
			inputField.text = null;
		}
		else
		{
			Debug.LogWarning("input can not be null");
		}
	}
	#endregion

    #region clear content
    private void ClearContent()
    {
    }
    #endregion

}
