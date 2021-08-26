using System;
using UnityEngine;

public class MainStateEnter : MainState
{
	private GameObject m_enterCameraGameObject = null;

	public MainStateEnter()
	{
		m_enterCameraGameObject = GameObject.Instantiate( Resources.Load( "Enter/EnterCamera" ) ) as GameObject;
		GuiManager.Instance.OpenViewSelectLanguage();
	}

	public override void ReceiveMessage( string message )
	{
		if( message == "LanguageSelected" )
		{
			bool conceptionDateSet = PrefsManager.Instance.IsConceptionDateSet();

			if( conceptionDateSet )
				MainStateManager.Instance.EnterState( MainStateManager.BABY );
			else
				GuiManager.Instance.OpenViewConceptionDate();
		}
		else if( message == "ConceptionDateSelected" )
		{
			MainStateManager.Instance.EnterState( MainStateManager.BABY );
		}
	}

	public override void Destroy()
	{
		if (m_enterCameraGameObject != null)
		{
			GameObject.Destroy(m_enterCameraGameObject);
			m_enterCameraGameObject = null;
		}
	}

}
