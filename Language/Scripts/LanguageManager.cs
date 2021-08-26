using UnityEngine;
using System.Collections;

public class LanguageManager
{

	private static LanguageManager m_instance = null;

	private PrefsManager.PostLoadHandler m_prefsManagerPostLoadHandler = null;
	private PrefsManager.PreSaveHandler m_prefsManagerPreSaveHandler = null;

	private LanguageManager()
	{
		m_prefsManagerPostLoadHandler = new PrefsManager.PostLoadHandler(HandlePrefsManagerPostLoad);
		m_prefsManagerPreSaveHandler = new PrefsManager.PreSaveHandler(HandlePrefsManagerPreSave);

		PrefsManager.Instance.m_postLoadHandlers += m_prefsManagerPostLoadHandler;
		PrefsManager.Instance.m_preSaveHandlers += m_prefsManagerPreSaveHandler;

		LoadLanguages();

		string defaultLanguageName = "English";
		if( GetNumLanguages() > 0 )
			defaultLanguageName = ( string )m_languageNames[ 0 ];

		SelectLanguage( defaultLanguageName );
	}

	public static LanguageManager Instance
	{
		get
		{
			if( m_instance == null )
				m_instance = new LanguageManager();
			return m_instance;
		}
	}

	public int GetNumLanguages()
	{
		return m_languageNames.Count;
	}

	public string GetLanguageName( int languageIndex )
	{
		return ( string )m_languageNames[ languageIndex ];
	}

	public string GetLanguageResource( int languageIndex )
	{
		return ( string )m_languageResources[ languageIndex ];
	}

	public bool LanguageExists( string languageName )
	{
		foreach( string iterLanguageName in m_languageNames )
		{
			if( iterLanguageName == languageName )
				return true;
		}
		return false;
	}

	public int FindLanguageIndex( string languageName )
	{
		int numLanguages = GetNumLanguages();
		for( int i = 0; i < numLanguages; i++ )
		{
			if( ( string )m_languageNames[ i ] == languageName )
				return i;
		}
		return -1;
	}

	public string GetSelectedLanguageName()
	{
		return m_selectedLanguageName;
	}

	public void SelectLanguage( string languageName )
	{
		if ( !LanguageExists( languageName ) )
			return;

		m_selectedLanguageName = languageName;

		LoadLanguage();

		//PlayerPrefs.SetString( "selectedLanguageName", m_selectedLanguageName );
	}

	public string LookupString( string key )
	{
		string str = "";
		if( m_languageHashtable.Contains( key ) )
			str = ( string )m_languageHashtable[ key ];
		return str;
	}

	public string LookupString( string key, Hashtable replaceTable )
	{
		string str = LookupString( key );
		while( true )
		{
			int replaceStartMarkerIndex = str.IndexOf( '%' );
			if( replaceStartMarkerIndex < 0 )
				break;

			int replaceEndMarkerIndex = str.IndexOf( '%', replaceStartMarkerIndex + 1 );
			if( replaceEndMarkerIndex < 0 )
				replaceEndMarkerIndex = str.Length;

			string replaceKey = str.Substring( replaceStartMarkerIndex + 1, replaceEndMarkerIndex - ( replaceStartMarkerIndex + 1 ) );

			string replaceString = "";
			if( replaceTable != null )
			{
				if( replaceTable.Contains( replaceKey ) )
					replaceString = ( string )replaceTable[ replaceKey ];
			}

			str = str.Substring( 0, replaceStartMarkerIndex ) + replaceString + str.Substring( replaceEndMarkerIndex + 1 );
		}
		return str;
	}

	private void LoadLanguages()
	{
		string resourceText = "{}";

		TextAsset textAsset = Resources.Load( "Languages/Languages" ) as TextAsset;
		if( textAsset != null )
		{
			resourceText = textAsset.text;
			//Object.Destroy(textAsset);
			textAsset = null;
		}

		object jsonObject = JSON.JsonDecode( resourceText );
		if( jsonObject is ArrayList )
		{
			ArrayList languagesArrayList = ( ArrayList )jsonObject;
			foreach( Hashtable languageHashtable in languagesArrayList )
			{
				if( languageHashtable.Contains( "Name" ) && languageHashtable.Contains( "Resource" ) )
				{
					string languageName = ( string )languageHashtable[ "Name" ];
					string languageResource = ( string )languageHashtable[ "Resource" ];
					if( languageName != "" && languageResource != "" )
					{
					//	Debug.Log("Adding language: languageName=" + languageName + ", languageResource=" + languageResource);
						m_languageNames.Add( languageName );
						m_languageResources.Add( languageResource );
					}
				}
			}
		}
	}

	private void LoadLanguage()
	{
	//	Debug.Log( "LoadLanguage()" );

		int languageIndex = FindLanguageIndex( m_selectedLanguageName );
		if( languageIndex < 0 )
			return;

		string languageResource = ( string )m_languageResources[ languageIndex ];

		string resourceText = "{}";

		TextAsset textAsset = Resources.Load( languageResource ) as TextAsset;
		if( textAsset != null )
		{
			resourceText = textAsset.text;
			//Object.Destroy(textAsset);
			textAsset = null;
		}

		object jsonObject = JSON.JsonDecode( resourceText );
		if( jsonObject is Hashtable )
			m_languageHashtable = ( Hashtable )jsonObject;
		else
			m_languageHashtable = new Hashtable();
	}

	private void HandlePrefsManagerPostLoad( PrefsManager prefsManager )
	{
		Debug.Log( "LanguageManager.HandlePrefsManagerPostLoad()" );

		string selectedLanguageName = prefsManager.GetSelectedLanguageName();
		if( LanguageExists( selectedLanguageName ) )
			SelectLanguage( selectedLanguageName );
	}

	private void HandlePrefsManagerPreSave( PrefsManager prefsManager )
	{
		Debug.Log( "LanguageManager.HandlePrefsManagerPreSave()" );

		prefsManager.SetSelectedLanguageName( m_selectedLanguageName );
	}

	private ArrayList m_languageNames = new ArrayList();
	private ArrayList m_languageResources = new ArrayList();

	private string m_selectedLanguageName = "";

	Hashtable m_languageHashtable = null;

}
