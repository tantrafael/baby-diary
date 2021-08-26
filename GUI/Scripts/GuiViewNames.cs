using UnityEngine;
using System.Collections;

public class GuiViewNames : GuiView
{
	private const int MARGIN = 10;

	private Rect m_backgroundRect;
	private GUIStyle m_backgroundStyle;
	private Texture m_backgroundImage;

	private Rect m_headerRect;
	private GUIStyle m_headerStyle;
	private string m_headerText;

	private Rect m_namesRect;
	private GUIStyle m_namesStyle;
	private string m_namesText;

	private Rect m_backRect;
	private GUIStyle m_backStyle;
	private string m_backText;

	private Rect m_addRect;
	private GUIStyle m_addStyle;
	private string m_addText;

	private Rect m_removeRect;
	private GUIStyle m_removeStyle;
	private string m_removeText;

	private Rect m_closeRect;
	private GUIStyle m_closeStyle;
	private string m_closeText;

	private iPhoneKeyboard m_iPhoneKeyboard = null;

	public GuiViewNames()
	{
		m_backgroundRect = new Rect( 0, 0, Screen.width, Screen.height );
		m_backgroundImage = ( Texture2D ) Resources.Load( "Textures/TextBackground" );

		m_headerRect = new Rect( 0, 5, Screen.width, 50 );
		m_headerStyle = new GUIStyle();
		m_headerStyle.font = ( Font ) Resources.Load( "Fonts/Arial Rounded Bold 40" );
		m_headerStyle.normal.textColor = Color.white;
		m_headerStyle.alignment = TextAnchor.UpperCenter;
		m_headerText = LanguageManager.Instance.LookupString( "Names" );

		m_namesRect = new Rect( MARGIN, 60, Screen.width - 2 * MARGIN, 350 );
		m_namesStyle = GUI.skin.box;
		m_namesStyle.alignment = TextAnchor.UpperLeft;
		m_namesText = "";

		m_backRect = new Rect( MARGIN, Screen.height - MARGIN - 50, 80, 50 );
		m_backStyle = GUI.skin.button;
		m_backText = LanguageManager.Instance.LookupString( "Back" );

		m_addRect = new Rect( 100, Screen.height - MARGIN - 50, 120, 50 );
		m_addStyle = GUI.skin.button;
		m_addText = LanguageManager.Instance.LookupString( "Add" );

		m_removeRect = new Rect( Screen.width - MARGIN - 80, Screen.height - MARGIN - 50, 80, 50 );
		m_removeStyle = GUI.skin.button;
		m_removeText = LanguageManager.Instance.LookupString( "Remove" );

		m_closeRect = new Rect( 280, 10, 30, 30 );
		m_closeStyle = GUI.skin.button;
		m_closeText = "X";

		UpdateNamesText();
	}

	public override void Update()
	{
		if( m_iPhoneKeyboard != null )
		{
			if( m_iPhoneKeyboard.done )
			{
				if( m_iPhoneKeyboard.text != "" )
				{
					NamesManager.Instance.AddName( m_iPhoneKeyboard.text );
					PrefsManager.Instance.Save();
					UpdateNamesText();
				}
				m_iPhoneKeyboard = null;
			}
		}
	}

	public override void OnGUI()
	{
		GUI.DrawTexture( m_backgroundRect, m_backgroundImage );

		GUI.Label( m_headerRect, m_headerText, m_headerStyle );

		GUI.Label( m_namesRect, m_namesText, m_namesStyle );

		if( GUI.Button( m_backRect, m_backText, m_backStyle ) )
		{
			GuiManager.Instance.OpenViewMenu();
		}
		if( GUI.Button( m_addRect, m_addText, m_addStyle ) )
		{
			m_iPhoneKeyboard = iPhoneKeyboard.Open( "", iPhoneKeyboardType.ASCIICapable, true, true );
		}
		if( GUI.Button( m_removeRect, m_removeText, m_removeStyle ) )
		{
			GuiManager.Instance.OpenViewNamesRemove();
		}

		/*if( GUI.Button( m_closeRect, m_closeText, m_closeStyle ) )
		{
			OnClose();
		}*/
	}

	public override void Destroy()
	{
	}

	private void UpdateNamesText()
	{
		m_namesText = "";

		int numNames = NamesManager.Instance.GetNumNames();
		for( int i = 0; i < numNames; i++ )
			m_namesText += NamesManager.Instance.GetName( i ) + "\n";
	}
}
