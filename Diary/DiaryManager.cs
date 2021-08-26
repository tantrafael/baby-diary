using UnityEngine;
using System.Collections;

public class DiaryManager
{

	private static DiaryManager m_instance = null;

	private ArrayList m_diaryEntries = new ArrayList();

	private PrefsManager.PostLoadHandler m_prefsManagerPostLoadHandler = null;
	private PrefsManager.PreSaveHandler m_prefsManagerPreSaveHandler = null;


	private DiaryManager()
	{
		m_prefsManagerPostLoadHandler = new PrefsManager.PostLoadHandler(HandlePrefsManagerPostLoad);
		m_prefsManagerPreSaveHandler = new PrefsManager.PreSaveHandler(HandlePrefsManagerPreSave);

		PrefsManager.Instance.m_postLoadHandlers += m_prefsManagerPostLoadHandler;
		PrefsManager.Instance.m_preSaveHandlers += m_prefsManagerPreSaveHandler;
	}

	public static DiaryManager Instance
	{
		get
		{
			if( m_instance == null )
				m_instance = new DiaryManager();
			return m_instance;
		}
	}

	public DiaryEntry GetDiaryEntryForDate( System.DateTime dateTime, bool createIfNotFound )
	{
		DiaryEntry diaryEntry = null;
		foreach( DiaryEntry iterDiaryEntry in m_diaryEntries )
		{
			if( iterDiaryEntry.m_dateTime.Year == dateTime.Year &&
				iterDiaryEntry.m_dateTime.Month == dateTime.Month &&
				iterDiaryEntry.m_dateTime.Day == dateTime.Day )
			{
				diaryEntry = iterDiaryEntry;
				break;
			}
		}
		if( diaryEntry == null && createIfNotFound )
		{
			diaryEntry = new DiaryEntry( dateTime, "" );
			m_diaryEntries.Add( diaryEntry );
		}
		return diaryEntry;
	}

	public int GetNumDiaryEntries()
	{
		return m_diaryEntries.Count;
	}

	public DiaryEntry GetDiaryEntry( int index )
	{
		return ( DiaryEntry )m_diaryEntries[ index ];
	}

	private void HandlePrefsManagerPostLoad( PrefsManager prefsManager )
	{
	//	Debug.Log( "DiaryManager.HandlePrefsManagerPostLoad()" );

		m_diaryEntries.Clear();

		int numDiaryEntries = prefsManager.GetNumDiaryEntries();
		for( int i = 0; i < numDiaryEntries; i++ )
		{
			DiaryEntry diaryEntry = prefsManager.GetDiaryEntry( i );
			if( diaryEntry.m_text != "" )
				m_diaryEntries.Add( diaryEntry );
		}
	}

	private void HandlePrefsManagerPreSave( PrefsManager prefsManager )
	{
	//	Debug.Log( "DiaryManager.HandlePrefsManagerPreSave()" );

		prefsManager.ClearDiaryEntries();

		// Remove all empty diary entries

		int iterIndex = 0;
		while( iterIndex < m_diaryEntries.Count )
		{
			DiaryEntry diaryEntry = ( DiaryEntry )m_diaryEntries[ iterIndex ];
			if( diaryEntry.m_text == "" )
				m_diaryEntries.RemoveAt( iterIndex );
			else
				iterIndex++;
		}

		foreach( DiaryEntry diaryEntry in m_diaryEntries )
		{
			if( diaryEntry.m_text != "" )
				prefsManager.AddDiaryEntry( diaryEntry );
		}
	}

}