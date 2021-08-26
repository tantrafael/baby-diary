//using System;
using UnityEngine;

public class GuiViewDiaryEntry : GuiView
{
	private const int MARGIN = 10;

	private System.DateTime m_dateTime;

	private Rect m_backgroundRect;
	private GUIStyle m_backgroundStyle;
	private Texture m_backgroundImage;

	private Rect m_headerRect;
	private GUIStyle m_headerStyle;
	private string m_headerText;

	private Rect m_entryRect;
	private GUIStyle m_entryStyle;
	private string m_entryText;

	private Rect m_backRect;
	private GUIStyle m_backStyle;
	private string m_backText;

	private Rect m_editRect;
	private GUIStyle m_editStyle;
	private string m_editText;

	private Rect m_saveRect;
	private GUIStyle m_saveStyle;
	private string m_saveText;

	private Rect m_closeRect;
	private GUIStyle m_closeStyle;
	private string m_closeText;

	private iPhoneKeyboard m_iPhoneKeyboard = null;

	public GuiViewDiaryEntry( System.DateTime dateTime )
	{
		m_dateTime = dateTime;

		DiaryEntry diaryEntry = DiaryManager.Instance.GetDiaryEntryForDate( m_dateTime, false );

		m_backgroundRect = new Rect( 0, 0, Screen.width, Screen.height );
		m_backgroundImage = ( Texture2D ) Resources.Load( "Textures/TextBackground" );

		int entryWidth = Screen.width - 2 * MARGIN;

		m_headerRect = new Rect( 0, 5, Screen.width, 50 );
		m_headerStyle = new GUIStyle();
		m_headerStyle.font = ( Font ) Resources.Load( "Fonts/Arial Rounded Bold 40" );
	//	m_headerStyle.normal.background = ( Texture2D ) Resources.Load( "Textures/TextBackground" );
		m_headerStyle.normal.textColor = Color.white;
		m_headerStyle.alignment = TextAnchor.UpperCenter;

		if( m_dateTime.Year == System.DateTime.Now.Year &&
			m_dateTime.Month == System.DateTime.Now.Month &&
			m_dateTime.Day == System.DateTime.Now.Day )
		{
			m_headerText = LanguageManager.Instance.LookupString( "Today" );
		}
		else
			m_headerText = m_dateTime.ToString();

		m_entryRect = new Rect( MARGIN, 60, entryWidth, 350 );
		m_entryStyle = GUI.skin.box;
		m_entryStyle.alignment = TextAnchor.UpperLeft;
		m_entryText = ( diaryEntry != null ) ? diaryEntry.m_text : "";

		m_backRect = new Rect( MARGIN, Screen.height - MARGIN - 50, 80, 50 );
		m_backStyle = GUI.skin.button;
		m_backText = LanguageManager.Instance.LookupString( "Back" );

		m_editRect = new Rect( 100, Screen.height - MARGIN - 50, 120, 50 );
		m_editStyle = GUI.skin.button;
		m_editText = LanguageManager.Instance.LookupString( "Edit" );

		m_saveRect = new Rect( Screen.width - MARGIN - 80, Screen.height - MARGIN - 50, 80, 50 );
		m_saveStyle = GUI.skin.button;
		m_saveText = LanguageManager.Instance.LookupString( "Save" );

		m_closeRect = new Rect( 280, 10, 30, 30 );
		m_closeStyle = GUI.skin.button;
		m_closeText = "X";
	}

	public override void Update()
	{
		if( m_iPhoneKeyboard != null )
		{
			m_entryText = m_iPhoneKeyboard.text;
			if( m_iPhoneKeyboard.done )
				m_iPhoneKeyboard = null;
		}
	}

	public override void OnGUI()
	{
		GUI.DrawTexture( m_backgroundRect, m_backgroundImage );

		GUI.Label( m_headerRect, m_headerText, m_headerStyle );
		GUI.Label( m_entryRect, m_entryText, m_entryStyle );

		if( GUI.Button( m_backRect, m_backText, m_backStyle ) )
		{
			GuiManager.Instance.OpenViewDiary();
		}
		if( GUI.Button( m_editRect, m_editText, m_editStyle ) )
		{
			m_iPhoneKeyboard = iPhoneKeyboard.Open( m_entryText, iPhoneKeyboardType.ASCIICapable, true, true );
		}
		if( GUI.Button( m_saveRect, m_saveText, m_saveStyle ) )
		{
			DiaryEntry diaryEntry = DiaryManager.Instance.GetDiaryEntryForDate( m_dateTime, true );
			diaryEntry.m_text = m_entryText;
			PrefsManager.Instance.Save();
		}

		/*if( GUI.Button( m_closeRect, m_closeText, m_closeStyle ) )
		{
			OnClose();
		}*/
	}

	public override void Destroy()
	{}
}