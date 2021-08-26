using UnityEngine;
using System.Collections;

public class PrefsManager
{
	public delegate void PostLoadHandler(PrefsManager prefsManager);
	public delegate void PreSaveHandler(PrefsManager prefsManager);

	public event PostLoadHandler m_postLoadHandlers;
	public event PreSaveHandler m_preSaveHandlers;


	private static PrefsManager m_instance = null;

	private string m_selectedLanguageName = "";

	private bool m_conceptionDateSet = false;
	private System.DateTime m_conceptionDate;

	private ArrayList m_diaryEntries = new ArrayList();

	private ArrayList m_names = new ArrayList();


	private PrefsManager()
	{
	}

	public static PrefsManager Instance
	{
		get
		{
			if( m_instance == null )
				m_instance = new PrefsManager();
			return m_instance;
		}
	}

	public void Reset()
	{
		m_conceptionDateSet = false;
		m_diaryEntries.Clear();
		m_names.Clear();
	}

	public void Load()
	{
	//	Debug.Log( "PrefsManager.Load()" );

		Reset();

		m_selectedLanguageName = PlayerPrefs.GetString( "selectedLanguageName", "" );

		string conceptionDateString = PlayerPrefs.GetString( "ConceptionDate", "" );
		if( conceptionDateString != "" )
		{
			m_conceptionDateSet = true;
			m_conceptionDate = System.DateTime.Parse( conceptionDateString );
		}

		int numDiaryEntries = PlayerPrefs.GetInt( "NumDiaryEntries", 0 );
		for( int i = 0; i < numDiaryEntries; i++ )
		{
			string diaryEntryDateTimeString = PlayerPrefs.GetString( "DiaryEntryDateTime" + i, "" );
			string diaryEntryTextString = PlayerPrefs.GetString( "DiaryEntryText" + i, "" );
			if( diaryEntryDateTimeString != "" && diaryEntryTextString != "" )
			{
				DiaryEntry diaryEntry = new DiaryEntry( System.DateTime.Parse( diaryEntryDateTimeString ), diaryEntryTextString );
				m_diaryEntries.Add( diaryEntry );
			}
		}

		int numNames = PlayerPrefs.GetInt( "NumNames", 0 );
		for( int i = 0; i < numNames; i++ )
		{
			string nameString = PlayerPrefs.GetString( "Name" + i, "" );
			if( nameString != "" )
				m_names.Add( nameString );
		}

	//	Debug.Log( "conceptionDateTimeSet=" + m_conceptionDateSet );
	//	Debug.Log( "conceptionDateTime=" + m_conceptionDate );
	//	Debug.Log( "m_diaryEntries.Count=" + m_diaryEntries.Count );

		if( m_postLoadHandlers != null )
			m_postLoadHandlers( this );
	}

	public void Save()
	{
		if( m_preSaveHandlers != null )
			m_preSaveHandlers( this );

		PlayerPrefs.SetString( "selectedLanguageName", m_selectedLanguageName );

		if( m_conceptionDateSet )
			PlayerPrefs.SetString( "ConceptionDate", m_conceptionDate.ToString() );

		PlayerPrefs.SetInt( "NumDiaryEntries", m_diaryEntries.Count );
		for( int i = 0; i < m_diaryEntries.Count; i++ )
		{
			DiaryEntry diaryEntry = ( DiaryEntry )m_diaryEntries[ i ];
			PlayerPrefs.SetString( "DiaryEntryDateTime" + i, diaryEntry.m_dateTime.ToString() );
			PlayerPrefs.SetString( "DiaryEntryText" + i, diaryEntry.m_text );
		}

		PlayerPrefs.SetInt( "NumNames", m_names.Count );
		for( int i = 0; i < m_names.Count; i++ )
		{
			PlayerPrefs.SetString( "Name" + i, ( string )m_names[ i ] );
		}
	}

	public string GetSelectedLanguageName()
	{
		return m_selectedLanguageName;
	}

	public void SetSelectedLanguageName( string selectedLanguageName )
	{
		m_selectedLanguageName = selectedLanguageName;
	}

	public bool IsConceptionDateSet()
	{
		return m_conceptionDateSet;
	}

	public System.DateTime GetConceptionDate()
	{
		return m_conceptionDate;
	}

	public System.TimeSpan GetAge()
	{
		return System.DateTime.Now - m_conceptionDate;
	}

	public void SetConceptionDate( System.DateTime conceptionDate )
	{
		Debug.Log("SetConceptionDateTime()");
		Debug.Log("conceptionDate=" + conceptionDate);

		m_conceptionDateSet = true;
		m_conceptionDate = conceptionDate;

		Save();
	}

	public void ClearDiaryEntries()
	{
		m_diaryEntries.Clear();
	}

	public void AddDiaryEntry( DiaryEntry diaryEntry )
	{
		m_diaryEntries.Add( diaryEntry );
	}

	public int GetNumDiaryEntries()
	{
		return m_diaryEntries.Count;
	}

	public DiaryEntry GetDiaryEntry( int index )
	{
		return ( DiaryEntry )m_diaryEntries[ index ];
	}

	public void ClearNames()
	{
		m_names.Clear();
	}

	public void AddName( string name )
	{
		m_names.Add( name );
	}

	public int GetNumNames()
	{
		return m_names.Count;
	}

	public string GetName( int index )
	{
		return ( string )m_names[ index ];
	}

}
