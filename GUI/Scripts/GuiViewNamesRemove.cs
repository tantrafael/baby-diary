//using System;
using UnityEngine;

public class GuiViewNamesRemove : GuiView
{
	private const int MARGIN = 10;

	private const int STATE_SELECT = 0;
	private const int STATE_CONFIRM = 1;


	private Rect m_backgroundRect;
	private GUIStyle m_backgroundStyle;
	private Texture m_backgroundImage;

	private Rect m_closeRect;
	private GUIStyle m_closeStyle;
	private string m_closeText;

	private Rect m_headerRect;
	private GUIStyle m_headerStyle;
	private string m_headerText;


	private Rect m_namesRect;
	private Vector2 m_namesScrollPos = Vector2.zero;

	private GUIStyle m_nameStyle;

	private Rect m_backRect;
	private GUIStyle m_backStyle;
	private string m_backText;


	private Rect m_confirmInfoRect;
	private GUIStyle m_confirmInfoStyle;
	private string m_confirmInfoText;

	private Rect m_confirmNameRect;
	private GUIStyle m_confirmNameStyle;
	private string m_confirmNameText;

	private Rect m_confirmNoRect;
	private GUIStyle m_confirmNoStyle;
	private string m_confirmNoText;

	private Rect m_confirmYesRect;
	private GUIStyle m_confirmYesStyle;
	private string m_confirmYesText;


	private int m_state = STATE_SELECT;

	private string m_selectedName = "";


	public GuiViewNamesRemove()
	{
		m_backgroundRect = new Rect( 0, 0, Screen.width, Screen.height );
		m_backgroundImage = ( Texture2D ) Resources.Load( "Textures/TextBackground" );

		m_closeRect = new Rect( 280, 10, 30, 30 );
		m_closeStyle = GUI.skin.button;
		m_closeText = "X";


		m_headerRect = new Rect( 0, 5, Screen.width, 50 );
		m_headerStyle = new GUIStyle();
		m_headerStyle.font = ( Font ) Resources.Load( "Fonts/Arial Rounded Bold 40" );
		m_headerStyle.normal.textColor = Color.white;
		m_headerStyle.alignment = TextAnchor.UpperCenter;
		m_headerText = LanguageManager.Instance.LookupString( "RemoveName" );

		m_namesRect = new Rect( MARGIN, 60, Screen.width - 2 * MARGIN, 340 );

		m_nameStyle = GUI.skin.button;

		m_backRect = new Rect( MARGIN, Screen.height - MARGIN - 50, Screen.width - 2 * MARGIN, 50 );
		m_backStyle = GUI.skin.button;
		m_backText = LanguageManager.Instance.LookupString( "Back" );


		m_confirmInfoRect = new Rect( 0, 150, Screen.width, 25 );
		m_confirmInfoStyle = new GUIStyle();
		m_confirmInfoStyle.font = ( Font ) Resources.Load( "Fonts/Arial Rounded Bold 15" );
		m_confirmInfoStyle.normal.textColor = Color.white;
		m_confirmInfoStyle.alignment = TextAnchor.UpperCenter;
		m_confirmInfoText = LanguageManager.Instance.LookupString( "RemoveTheFollowingName?" );

		m_confirmNameRect = new Rect( 0, 200, Screen.width, 50 );
		m_confirmNameStyle = new GUIStyle();
		m_confirmNameStyle.font = ( Font ) Resources.Load( "Fonts/Arial Rounded Bold 20" );
		m_confirmNameStyle.normal.textColor = Color.white;
		m_confirmNameStyle.alignment = TextAnchor.UpperCenter;
		m_confirmNameText = "";

		m_confirmNoRect = new Rect( MARGIN, 300, 100, 50 );
		m_confirmNoStyle = GUI.skin.button;
		m_confirmNoText = LanguageManager.Instance.LookupString( "No" );

		m_confirmYesRect = new Rect( Screen.width - MARGIN - 100, 300, 100, 50 );
		m_confirmYesStyle = GUI.skin.button;
		m_confirmYesText = LanguageManager.Instance.LookupString( "Yes" );

	}

	public override void OnGUI()
	{
		GUI.DrawTexture( m_backgroundRect, m_backgroundImage );

		/*if( GUI.Button( m_closeRect, m_closeText, m_closeStyle ) )
		{
			OnClose();
		}*/

		GUI.Label( m_headerRect, m_headerText, m_headerStyle );

		if( m_state == STATE_SELECT )
		{
			int numNames = NamesManager.Instance.GetNumNames();
	
			float scrollbarWidth = GUI.skin.verticalScrollbar.fixedWidth;
			float scrollbarHeight = GUI.skin.horizontalScrollbar.fixedHeight;

			Vector2 namesContentSize = new Vector2( m_namesRect.width - scrollbarWidth - 0.1f, numNames * 50.0f );
			if( namesContentSize.y < m_namesRect.height )
				namesContentSize.y = m_namesRect.height;
			Rect namesContentRect = new Rect( 0.0f, 0.0f, namesContentSize.x, namesContentSize.y );
	
			m_namesScrollPos = GUI.BeginScrollView( m_namesRect, m_namesScrollPos, namesContentRect );

			for( int i = 0; i < numNames; i++ )
			{
				string name = NamesManager.Instance.GetName( i );

				Rect nameRect = new Rect( 0.0f, i * 50.0f, namesContentSize.x - 10.0f, 50.0f );
				if( GUI.Button( nameRect, name, m_nameStyle ) )
				{
					m_selectedName = name;
					m_state = STATE_CONFIRM;
					UpdateConfirmNameText();
				}
			}
	
			GUI.EndScrollView();
	
			if( GUI.Button( m_backRect, m_backText, m_backStyle ) )
			{
				GuiManager.Instance.OpenViewNames();
			}
		}
		else if( m_state == STATE_CONFIRM )
		{
			GUI.Label( m_confirmInfoRect, m_confirmInfoText, m_confirmInfoStyle );
			GUI.Label( m_confirmNameRect, m_confirmNameText, m_confirmNameStyle );

			if( GUI.Button( m_confirmNoRect, m_confirmNoText, m_confirmNoStyle ) )
			{
				m_selectedName = "";
				m_state = STATE_SELECT;
			}
			if( GUI.Button( m_confirmYesRect, m_confirmYesText, m_confirmYesStyle ) )
			{
				NamesManager.Instance.RemoveName( m_selectedName );
				PrefsManager.Instance.Save();

				m_selectedName = "";
				m_state = STATE_SELECT;
			}
		}

	}

	public override void Destroy()
	{
	}

	private void UpdateConfirmNameText()
	{
		m_confirmNameText = m_selectedName;
	}

}