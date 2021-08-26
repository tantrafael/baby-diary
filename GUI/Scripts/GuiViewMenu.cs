using System;
using UnityEngine;

public class InfoToggleArgs : EventArgs
{
	private readonly bool visible;

	public InfoToggleArgs( bool visible )
	{
		this.visible = visible;
	}

	public bool Visible
	{
		get { return visible; }
	}
}

public delegate void InfoToggleHandler( object sender, InfoToggleArgs e );

public class GuiViewMenu : GuiView
{
	public delegate void PreviousWeekHandler();
	public delegate void NextWeekHandler();

	public event InfoToggleHandler InfoToggle;
	public event PreviousWeekHandler m_previousWeekHandlers;
	public event NextWeekHandler m_nextWeekHandlers;

	private const float SCROLL_SPEED = 1.0f;

	private int m_menuIndex = -1;
	private string[] m_menuStrings = new string[ 2 ] { "Diary", "Names" };
	private Rect m_menuRect = new Rect( 10, Screen.height - 60, Screen.width - 20, 50 );

	private Rect m_backgroundRect;
	private GUIStyle m_backgroundStyle;
	private Texture m_backgroundImage;

	private Rect m_rulerRect;
	private Texture2D m_rulerImage;

	private Rect m_headerRect;
	private GUIStyle m_headerStyle;
	private string m_headerText;

	private Rect m_subHeaderRect;
	private GUIStyle m_subHeaderStyle;
	private string m_subHeaderText;

	private Rect m_showInfoRect;
	private GUIStyle m_showInfoStyle;

	private Rect m_previousWeekRect;
	private GUIStyle m_previousWeekStyle;
	private string m_previousWeekText;

	private Rect m_nextWeekRect;
	private GUIStyle m_nextWeekStyle;
	private string m_nextWeekText;

	private Rect m_infoRect;
	private Rect m_infoContentRect;
	private GUIStyle m_infoStyle;
	private string m_infoText;
	private Vector2 m_infoScrollPosition;

	private Rect m_diaryRect;
	private GUIStyle m_diaryStyle;
	private string m_diaryText;

	private Rect m_namesRect;
	private GUIStyle m_namesStyle;
	private string m_namesText;

	private int m_selectedWeek = 1;

	private bool m_showInfo = false;

	public GuiViewMenu()
	{
		m_selectedWeek = GuiManager.Instance.GetSelectedWeek();
	}

	public override void InitGui()
	{
	//	Debug.Log("GuiViewMenu.InitGui()");
		
		m_backgroundRect = new Rect( 0, 0, Screen.width, Screen.height );
		m_backgroundImage = ( Texture2D ) Resources.Load( "Textures/TextBackground" );

		m_rulerRect = new Rect( 6, 6, 4, 468 );
		m_rulerImage = ( Texture2D ) Resources.Load( "Textures/Ruler" );

		m_headerRect = new Rect( 0, 5, Screen.width, 50 );
		m_headerStyle = new GUIStyle();
		m_headerStyle.font = ( Font ) Resources.Load( "Fonts/Arial Rounded Bold 40" );
	//	m_headerStyle.normal.background = ( Texture2D ) Resources.Load( "Textures/TextBackground" );
		m_headerStyle.normal.textColor = Color.white;
		m_headerStyle.alignment = TextAnchor.UpperCenter;
		m_headerText = "";

		m_subHeaderRect = new Rect( 0, 40, Screen.width, 20 );
		m_subHeaderStyle = new GUIStyle();
		m_subHeaderStyle.font = ( Font ) Resources.Load( "Fonts/Arial Rounded Bold 12" );
	//	m_subHeaderStyle.normal.background = ( Texture2D ) Resources.Load( "Textures/TextBackground" );
		m_subHeaderStyle.normal.textColor = Color.white;
		m_subHeaderStyle.alignment = TextAnchor.LowerCenter;
		m_subHeaderText = "";

		m_showInfoRect = new Rect( 60, 0, Screen.width - 2 * 60, 60 );
		m_showInfoStyle = GUI.skin.FindStyle( "empty" );

		m_previousWeekRect = new Rect( 10, 10, 50, 50 );
		m_previousWeekStyle = GUI.skin.button;
		m_previousWeekText = "<";

		m_nextWeekRect = new Rect( Screen.width - 60, 10, 50, 50 );
		m_nextWeekStyle = GUI.skin.button;
		m_nextWeekText = ">";

		m_infoRect = new Rect( 10, 70, Screen.width - 20, 340 );
		m_infoContentRect = new Rect( 0, 0, m_infoRect.width - 16, m_infoRect.height + 100 );
		m_infoStyle = GUI.skin.box;
	//	GUI.skin.verticalScrollbarThumb.fixedWidth
	/*
		m_infoStyle = new GUIStyle();
		m_infoStyle.font = ( Font ) Resources.Load( "Fonts/Arial Rounded Bold 15" );
		m_infoStyle.normal.textColor = Color.white;
		m_infoStyle.normal.background = ( Texture2D ) Resources.Load( "Textures/TextBackground" );
		m_infoStyle.wordWrap = true;
	*/
		m_infoText = "";
		m_infoScrollPosition = Vector2.zero;

		m_diaryRect = new Rect( 10, Screen.height - 60, Screen.width / 2 - 15, 50 );
		m_diaryStyle = GUI.skin.button;
		m_diaryText = LanguageManager.Instance.LookupString( "Diary" );

		m_namesRect = new Rect( Screen.width / 2 + 5, Screen.height - 60, Screen.width / 2 - 15, 50 );
		m_namesStyle = GUI.skin.button;
		m_namesText = LanguageManager.Instance.LookupString( "Names" );

		UpdateHeaderText();
		UpdateSubHeaderText();
		UpdateInfoText();

		base.InitGui();
	}

	protected virtual void OnInfoToggle( InfoToggleArgs e )
	{
		if( InfoToggle != null )
		{
			InfoToggle( this, e );
		}
	}

	public void ScrollInfo( float value )
	{
		m_infoScrollPosition.y += SCROLL_SPEED * value;
	//	m_infoContentRect.y -= 0.5f * value;
	//	m_infoContentRect.y -= value;
	}

/*
	public override void HandleInput()
	{}
*/

	public override void Update()
	{
		int selectedWeek = GuiManager.Instance.GetSelectedWeek();
		if( m_selectedWeek != selectedWeek )
		{
			m_selectedWeek = selectedWeek;
			UpdateHeaderText();
			UpdateInfoText();
		}
	}

	public override void OnGUI()
	{
		if( m_showInfo )
		{
			GUI.DrawTexture( m_backgroundRect, m_backgroundImage );
		}

	//	GUI.DrawTexture( m_rulerRect, m_rulerImage );
		
		GUI.Label( m_headerRect, m_headerText, m_headerStyle );
		GUI.Label( m_subHeaderRect, m_subHeaderText, m_subHeaderStyle );

		if( GUI.Button( m_showInfoRect, "", m_showInfoStyle ) )
		{
			m_showInfo = !m_showInfo;

			InfoToggleArgs e = new InfoToggleArgs( m_showInfo );
			OnInfoToggle( e );

			if( m_showInfo )
				m_subHeaderText = LanguageManager.Instance.LookupString("ClickHereToHideInfo");
			else
				m_subHeaderText = LanguageManager.Instance.LookupString("ClickHereToShowInfo");
		}
		if( GUI.Button( m_previousWeekRect, m_previousWeekText, m_previousWeekStyle ) )
		{
			if( m_previousWeekHandlers != null )
				m_previousWeekHandlers();
		}
		if( GUI.Button( m_nextWeekRect, m_nextWeekText, m_nextWeekStyle ) )
		{
			if( m_nextWeekHandlers != null )
				m_nextWeekHandlers();
		}

		if( m_showInfo )
		{
			GUILayout.BeginArea( m_infoRect );
			m_infoScrollPosition = GUILayout.BeginScrollView( m_infoScrollPosition );
		//	GUILayout.Label( m_infoText, m_infoStyle );
			GUILayout.Label( m_infoText );
			GUILayout.EndScrollView();
			GUILayout.EndArea();

		/*
			GUI.BeginGroup( m_infoRect, m_infoStyle );
		//	GUI.Label( m_infoContentRect, m_infoText );
			GUILayout.Label( m_infoText, GUILayout.Width( m_infoRect.width - 10 ) );
			GUI.EndGroup();
		*/
		}

		if( GUI.Button( m_diaryRect, m_diaryText, m_diaryStyle ) )
		{
			GuiManager.Instance.OpenViewDiary();
		}
		if( GUI.Button( m_namesRect, m_namesText, m_namesStyle ) )
		{
			GuiManager.Instance.OpenViewNames();
		}
	}

	public override void Destroy()
	{}

	private void UpdateHeaderText()
	{
		m_headerText = LanguageManager.Instance.LookupString( "Week" ) + " " + m_selectedWeek;
	}

	private void UpdateSubHeaderText()
	{
		if( m_showInfo )
			m_subHeaderText = LanguageManager.Instance.LookupString( "ClickHereToHideInfo" );
		else
			m_subHeaderText = LanguageManager.Instance.LookupString( "ClickHereToShowInfo" );
	}

	private void UpdateInfoText()
	{
		m_infoText = LanguageManager.Instance.LookupString( "WeekInfo" + m_selectedWeek );
	}

}