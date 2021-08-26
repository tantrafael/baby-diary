using System;
using UnityEngine;

public class GuiViewSelectLanguage : GuiView
{
	private const int MARGIN = 10;

	private Rect m_backgroundRect;
	private GUIStyle m_backgroundStyle;
	private Texture m_backgroundImage;

	private Rect m_headerRect;
	private GUIStyle m_headerStyle;
	private string m_headerText;

	private Rect m_languageRect;
	private GUIStyle m_languageStyle;
	private string m_languageText;

	private Rect m_confirmRect;
	private GUIStyle m_confirmStyle;
	private string m_confirmText;

	private int m_languageIndex = 0;
	private string m_languageName = "";

	public GuiViewSelectLanguage()
	{
		m_languageName = LanguageManager.Instance.GetSelectedLanguageName();
		m_languageIndex = LanguageManager.Instance.FindLanguageIndex( m_languageName );
	}

	public override void InitGui()
	{
		m_backgroundRect = new Rect( 0, 0, Screen.width, Screen.height );
		m_backgroundImage = ( Texture2D ) Resources.Load( "Textures/TextBackground" );

		m_headerRect = new Rect( 0, 5, Screen.width, 100 );
		m_headerStyle = new GUIStyle();
		m_headerStyle.font = ( Font ) Resources.Load( "Fonts/Arial Rounded Bold 40" );
		m_headerStyle.normal.textColor = Color.white;
		m_headerStyle.alignment = TextAnchor.UpperCenter;
		m_headerText = "";

		m_languageRect = new Rect( 0, 150, Screen.width, 100 );
		m_languageStyle = new GUIStyle();
		m_languageStyle.font = ( Font ) Resources.Load( "Fonts/Arial Rounded Bold 40" );
		m_languageStyle.normal.textColor = Color.white;
		m_languageStyle.alignment = TextAnchor.MiddleCenter;
		m_languageText = "";

		m_confirmRect = new Rect( MARGIN, Screen.height - MARGIN - 50, Screen.width - 2 * MARGIN, 50 );
		m_confirmStyle = GUI.skin.button;
		m_confirmText = "";

		UpdateHeaderText();
		UpdateLanguageText();
		UpdateConfirmText();
	}

	public override void OnGUI()
	{
		GUI.DrawTexture( m_backgroundRect, m_backgroundImage );

		GUI.Label( m_headerRect, m_headerText, m_headerStyle );
		GUI.Label( m_languageRect, m_languageText, m_languageStyle );

		if( GUI.Button( new Rect( 10, 175, 40, 50 ), "<" ) )
		{
			PreviousLanguage();
		}
		if( GUI.Button( new Rect( Screen.width - 50, 175, 40, 50 ), ">" ) )
		{
			NextLanguage();
		}

		if( GUI.Button( m_confirmRect, m_confirmText, m_confirmStyle ) )
		{
			MainStateManager.Instance.SendMessage( "LanguageSelected" );
		}
	}

	public override void Destroy()
	{
	}

	private void PreviousLanguage()
	{
		m_languageIndex--;
		if( m_languageIndex < 0 )
			m_languageIndex += LanguageManager.Instance.GetNumLanguages();

		m_languageName = LanguageManager.Instance.GetLanguageName( m_languageIndex );

		LanguageManager.Instance.SelectLanguage( m_languageName );

		UpdateHeaderText();
		UpdateLanguageText();
	}

	private void NextLanguage()
	{
		m_languageIndex++;
		if( m_languageIndex >= LanguageManager.Instance.GetNumLanguages() )
			m_languageIndex -= LanguageManager.Instance.GetNumLanguages();

		m_languageName = LanguageManager.Instance.GetLanguageName( m_languageIndex );

		LanguageManager.Instance.SelectLanguage( m_languageName );

		UpdateHeaderText();
		UpdateLanguageText();
	}

	private void UpdateHeaderText()
	{
		m_headerText = LanguageManager.Instance.LookupString( "SelectLanguage" );
	}

	private void UpdateLanguageText()
	{
		m_languageText = m_languageName;
	}

	private void UpdateConfirmText()
	{
		m_confirmText = LanguageManager.Instance.LookupString( "Confirm" );
	}

}