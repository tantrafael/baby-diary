using UnityEngine;
using System.Collections;

public class NamesManager
{

	private static NamesManager m_instance = null;

	private ArrayList m_names = new ArrayList();

	private PrefsManager.PostLoadHandler m_prefsManagerPostLoadHandler = null;
	private PrefsManager.PreSaveHandler m_prefsManagerPreSaveHandler = null;


	private NamesManager()
	{
		m_prefsManagerPostLoadHandler = new PrefsManager.PostLoadHandler(HandlePrefsManagerPostLoad);
		m_prefsManagerPreSaveHandler = new PrefsManager.PreSaveHandler(HandlePrefsManagerPreSave);

		PrefsManager.Instance.m_postLoadHandlers += m_prefsManagerPostLoadHandler;
		PrefsManager.Instance.m_preSaveHandlers += m_prefsManagerPreSaveHandler;
	}

	public static NamesManager Instance
	{
		get
		{
			if( m_instance == null )
				m_instance = new NamesManager();
			return m_instance;
		}
	}

	public void AddName( string name )
	{
		m_names.Add( name );
	}

	public void RemoveName( string name )
	{
		m_names.Remove( name );
	}

	public int GetNumNames()
	{
		return m_names.Count;
	}

	public string GetName( int index )
	{
		return ( string )m_names[ index ];
	}

	private void HandlePrefsManagerPostLoad( PrefsManager prefsManager )
	{
		Debug.Log("NamesManager.HandlePrefsManagerPostLoad()");

		m_names.Clear();

		int numNames = prefsManager.GetNumNames();
		for( int i = 0; i < numNames; i++ )
		{
			string name = prefsManager.GetName( i );
			if( name != "" )
				m_names.Add( name );
		}
	}

	private void HandlePrefsManagerPreSave( PrefsManager prefsManager )
	{
		Debug.Log("NamesManager.HandlePrefsManagerPreSave()");

		prefsManager.ClearNames();

		// Remove all empty names

		int iterIndex = 0;
		while( iterIndex < m_names.Count )
		{
			string name = ( string )m_names[ iterIndex ];
			if( name == "" )
				m_names.RemoveAt( iterIndex );
			else
				iterIndex++;
		}

		foreach( string name in m_names )
			prefsManager.AddName( name );
	}

}