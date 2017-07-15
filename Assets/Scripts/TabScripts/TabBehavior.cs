using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabBehavior : MonoBehaviour 
{
	public Text txt;
	public Image image;
	public Tips tips;
    public string id;
    private ScrollViewManager manager;
	private Button button;
	#region enable and disable this scripts
	private void OnEnable()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(OnClickButton);
		tips.Init();
	}
	private void OnDisable()
	{
		button.onClick.RemoveListener(OnClickButton);
	}
	#endregion

	#region click events
	public void OnClickButton()
	{
        manager.SwitchToTab(id);
        manager.tabManager.SwitchTabState(this);
		Debug.Log("send message named:  " + txt.text);
	}
	#endregion

	#region set components
    public void SetMainManager(ScrollViewManager manager)
	{
		this.manager = manager;
	}
	#endregion

	#region set click or normal state
	private TabState currentState = TabState.Normal;
	public void SetState(TabState state)
	{
		currentState = state;
		if (state == TabState.Selected)
		{
            image.color = manager.tabManager.selectedColor;
			SetAttention(false);
		}
		else
		{
            image.color = manager.tabManager.normalColor;
		}
	}
	#endregion
	#region receive message
	public void ReceiveMessage(string id, string message)
	{
		Debug.Log("id:  "+id+"  receive message"+message);
		if(currentState == TabState.Normal)
		{
			SetAttention(true);
		}
	}
	private void SetAttention(bool canShow)
	{
		if (canShow)
		{
			//hide reminder
			tips.Show();
		}
		else
		{
			//show reminder
			tips.Hide();
		}
	}
	#endregion
}
