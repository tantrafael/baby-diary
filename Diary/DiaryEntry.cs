using UnityEngine;

public class DiaryEntry
{

	public System.DateTime m_dateTime;
	public string m_text = "";

	public DiaryEntry( System.DateTime dateTime, string text )
	{
		m_dateTime = dateTime;
		m_text = text;
	}

}