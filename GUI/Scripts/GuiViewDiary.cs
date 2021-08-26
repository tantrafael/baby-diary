//using System;
using UnityEngine;

public class GuiViewDiary : GuiView
{
	private const int MARGIN = 10;

	private Rect m_backgroundRect;
	private GUIStyle m_backgroundStyle;
	private Texture m_backgroundImage;

	private Rect m_headerRect;
	private GUIStyle m_headerStyle;
	private string m_headerText;

	private Rect m_todayRect;
	private GUIStyle m_todayStyle;
	private string m_todayText;

	private Rect m_otherDateRect;
	private GUIStyle m_otherDateStyle;
	private string m_otherDateText;

	private Rect m_browseEntriesRect;
	private GUIStyle m_browseEntriesStyle;
	private string m_browseEntriesText;

	private Rect m_backRect;
	private GUIStyle m_backStyle;
	private string m_backText;

	private Rect m_closeRect;
	private GUIStyle m_closeStyle;
	private string m_closeText;

	public GuiViewDiary()
	{
		m_backgroundRect = new Rect( 0, 0, Screen.width, Screen.height );
		m_backgroundImage = ( Texture2D ) Resources.Load( "Textures/TextBackground" );

		m_headerRect = new Rect( 0, 5, Screen.width, 50 );
		m_headerStyle = new GUIStyle();
		m_headerStyle.font = ( Font ) Resources.Load( "Fonts/Arial Rounded Bold 40" );
	//	m_headerStyle.normal.background = ( Texture2D ) Resources.Load( "Textures/TextBackground" );
		m_headerStyle.normal.textColor = Color.white;
		m_headerStyle.alignment = TextAnchor.UpperCenter;
		m_headerText = LanguageManager.Instance.LookupString( "Diary" );

		m_todayRect = new Rect( MARGIN, 125, Screen.width - 2 * MARGIN, 50 );
		m_todayStyle = GUI.skin.button;
		m_todayText = LanguageManager.Instance.LookupString( "Today" );

		m_otherDateRect = new Rect( MARGIN, 185, Screen.width - 2 * MARGIN, 50 );
		m_otherDateStyle = GUI.skin.button;
		m_otherDateText = LanguageManager.Instance.LookupString( "OtherDate" );

		m_browseEntriesRect = new Rect( MARGIN, 245, Screen.width - 2 * MARGIN, 50 );
		m_browseEntriesStyle = GUI.skin.button;
		m_browseEntriesText = LanguageManager.Instance.LookupString( "BrowseEntries" );

		m_backRect = new Rect( MARGIN, 305, Screen.width - 2 * MARGIN, 50 );
		m_backStyle = GUI.skin.button;
		m_backText = LanguageManager.Instance.LookupString( "Back" );

		m_closeRect = new Rect( 280, 10, 30, 30 );
		m_closeStyle = GUI.skin.button;
		m_closeText = "X";
	}

	public override void Update()
	{
	}

	public override void OnGUI()
	{
		GUI.DrawTexture( m_backgroundRect, m_backgroundImage );

		GUI.Label( m_headerRect, m_headerText, m_headerStyle );

		if( GUI.Button( m_todayRect, m_todayText, m_todayStyle ) )
		{
			GuiManager.Instance.OpenViewDiaryEntry( System.DateTime.Now );
		}
		if( GUI.Button( m_otherDateRect, m_otherDateText, m_otherDateStyle ) )
		{
			GuiManager.Instance.OpenViewDiarySelectDate();
		}
		if( GUI.Button( m_browseEntriesRect, m_browseEntriesText, m_browseEntriesStyle ) )
		{
			GuiManager.Instance.OpenViewDiaryBrowseEntries();
		}
		if( GUI.Button( m_backRect, m_backText, m_backStyle ) )
		{
			GuiManager.Instance.OpenViewMenu();
		}

		/*if( GUI.Button( m_closeRect, m_closeText, m_closeStyle ) )
		{
			OnClose();
		}*/
	}

	public override void Destroy()
	{}
}
