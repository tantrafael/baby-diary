//using System;
using UnityEngine;

public class GuiViewDiaryBrowseEntries : GuiView
{
	private const int MARGIN = 10;

	private Rect m_backgroundRect;
	private GUIStyle m_backgroundStyle;
	private Texture m_backgroundImage;

	private Rect m_headerRect;
	private GUIStyle m_headerStyle;
	private string m_headerText;

	private Rect m_entriesRect;
	private Vector2 m_entriesScrollPos = Vector2.zero;

	private GUIStyle m_entryStyle;

	private Rect m_backRect;
	private GUIStyle m_backStyle;
	private string m_backText;

	private Rect m_closeRect;
	private GUIStyle m_closeStyle;
	private string m_closeText;

	private iPhoneKeyboard m_iPhoneKeyboard = null;

	public GuiViewDiaryBrowseEntries()
	{
		m_backgroundRect = new Rect( 0, 0, Screen.width, Screen.height );
		m_backgroundImage = ( Texture2D ) Resources.Load( "Textures/TextBackground" );

		int entriesWidth = Screen.width - 2 * MARGIN;

		m_headerRect = new Rect( 0, 5, Screen.width, 50 );
		m_headerStyle = new GUIStyle();
		m_headerStyle.font = ( Font ) Resources.Load( "Fonts/Arial Rounded Bold 40" );
		m_headerStyle.normal.textColor = Color.white;
		m_headerStyle.alignment = TextAnchor.UpperCenter;
		m_headerText = LanguageManager.Instance.LookupString( "SelectEntry" );

		m_entriesRect = new Rect( MARGIN, 60, entriesWidth, 340 );

		m_entryStyle = GUI.skin.button;

		m_backRect = new Rect( MARGIN, 410, entriesWidth, 50 );
		m_backStyle = GUI.skin.button;
		m_backText = LanguageManager.Instance.LookupString( "Back" );

		m_closeRect = new Rect( 280, 10, 30, 30 );
		m_closeStyle = GUI.skin.button;
		m_closeText = "X";
	}

	public override void OnGUI()
	{
		GUI.DrawTexture( m_backgroundRect, m_backgroundImage );

		GUI.Label( m_headerRect, m_headerText, m_headerStyle );

		int numEntries = DiaryManager.Instance.GetNumDiaryEntries();

		float scrollbarWidth = GUI.skin.verticalScrollbar.fixedWidth;
		float scrollbarHeight = GUI.skin.horizontalScrollbar.fixedHeight;

		Vector2 entriesContentSize = new Vector2( m_entriesRect.width - scrollbarWidth - 0.1f, numEntries * 50.0f );
		if( entriesContentSize.y < m_entriesRect.height )
			entriesContentSize.y = m_entriesRect.height;
		Rect entriesContentRect = new Rect( 0.0f, 0.0f, entriesContentSize.x, entriesContentSize.y );

		m_entriesScrollPos = GUI.BeginScrollView( m_entriesRect, m_entriesScrollPos, entriesContentRect );

		for( int i = 0; i < numEntries; i++ )
		{
			DiaryEntry diaryEntry = DiaryManager.Instance.GetDiaryEntry( i );

			Rect entryRect = new Rect( 0.0f, i * 50.0f, entriesContentSize.x - 10.0f, 50.0f );
			string entryText = diaryEntry.m_dateTime.ToString();
			if( GUI.Button( entryRect, entryText, m_entryStyle ) )
			{
				GuiManager.Instance.OpenViewDiaryEntry( diaryEntry.m_dateTime );
			}
		}

		GUI.EndScrollView();

		if( GUI.Button( m_backRect, m_backText, m_backStyle ) )
		{
			GuiManager.Instance.OpenViewDiary();
		}

		/*if( GUI.Button( m_closeRect, m_closeText, m_closeStyle ) )
		{
			OnClose();
		}*/
	}

	public override void Destroy()
	{}
}