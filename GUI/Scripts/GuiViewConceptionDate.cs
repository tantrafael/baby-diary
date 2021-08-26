using System;
using UnityEngine;

public class GuiViewConceptionDate : GuiView
{
	private const int MARGIN = 10;

	private Rect m_backgroundRect;
	private GUIStyle m_backgroundStyle;
	private Texture m_backgroundImage;

	private Rect m_headerRect;
	private GUIStyle m_headerStyle;
	private string m_headerText;

	private Rect m_monthRect;
	private GUIStyle m_monthStyle;
	private string m_monthText;

	private Rect m_dayRect;
	private GUIStyle m_dayStyle;
	private string m_dayText;

	private Rect m_confirmRect;
	private GUIStyle m_confirmStyle;
	private string m_confirmText;

	private int m_month = 1;
	private int m_day = 1;

	public GuiViewConceptionDate()
	{
	}

	public override void InitGui()
	{
		m_backgroundRect = new Rect( 0, 0, Screen.width, Screen.height );
		m_backgroundImage = ( Texture2D ) Resources.Load( "Textures/TextBackground" );

		m_headerRect = new Rect( 0, 5, Screen.width, 100 );
		m_headerStyle = new GUIStyle();
		m_headerStyle.font = ( Font ) Resources.Load( "Fonts/Arial Rounded Bold 29" );
		m_headerStyle.normal.textColor = Color.white;
		m_headerStyle.alignment = TextAnchor.UpperCenter;
		m_headerText = LanguageManager.Instance.LookupString( "WhenDidThePregnancyStart?" );

		m_monthRect = new Rect( 0, 150, Screen.width, 100 );
		m_monthStyle = new GUIStyle();
		m_monthStyle.font = ( Font ) Resources.Load( "Fonts/Arial Rounded Bold 40" );
		m_monthStyle.normal.textColor = Color.white;
		m_monthStyle.alignment = TextAnchor.MiddleCenter;
		m_monthText = "";

		m_dayRect = new Rect( 0, 225, Screen.width, 100 );
		m_dayStyle = new GUIStyle();
		m_dayStyle.font = ( Font ) Resources.Load( "Fonts/Arial Rounded Bold 80" );
		m_dayStyle.normal.textColor = Color.white;
		m_dayStyle.alignment = TextAnchor.MiddleCenter;
		m_dayText = "";

		m_confirmRect = new Rect( MARGIN, Screen.height - MARGIN - 50, Screen.width - 2 * MARGIN, 50 );
		m_confirmStyle = GUI.skin.button;
		m_confirmText = LanguageManager.Instance.LookupString( "Confirm" );

		UpdateMonthText();
		UpdateDayText();
	}

	public override void OnGUI()
	{
		GUI.DrawTexture( m_backgroundRect, m_backgroundImage );

		GUI.Label( m_headerRect, m_headerText, m_headerStyle );
		GUI.Label( m_monthRect, m_monthText, m_monthStyle );
		GUI.Label( m_dayRect, m_dayText, m_dayStyle );

		if( GUI.Button( new Rect( 10, 175, 40, 50 ), "<" ) )
			SetMonth( m_month - 1 );
		if( GUI.Button( new Rect( Screen.width - 50, 175, 40, 50 ), ">" ) )
			SetMonth( m_month + 1 );
		if( GUI.Button( new Rect( 10, 250, 40, 50 ), "<" ) )
			SetDay( m_day - 1 );
		if( GUI.Button( new Rect( Screen.width - 50, 250, 40, 50 ), ">" ) )
			SetDay( m_day + 1 );

		if( GUI.Button( m_confirmRect, m_confirmText, m_confirmStyle ) )
		{
			DateTime conceptionDate = CalcDate();
			PrefsManager.Instance.SetConceptionDate( conceptionDate );
			MainStateManager.Instance.SendMessage( "ConceptionDateSelected" );
		}
	}

	public override void Destroy()
	{
	}

	private void SetMonth( int month )
	{
		m_month = month;

		if( m_month < 1 )
			m_month = 12;
		if( m_month > 12 )
			m_month = 1;

		UpdateMonthText();

		DateTime date = CalcDate();
		int daysInMonth = System.DateTime.DaysInMonth( date.Year, date.Month );

		if( m_day > daysInMonth )
			m_day = daysInMonth;

		SetDay( m_day );
	}

	private void SetDay( int day )
	{
		m_day = day;

		DateTime date = CalcDate();
		int daysInMonth = System.DateTime.DaysInMonth( date.Year, date.Month );

		if( m_day < 1 )
			m_day = daysInMonth;
		if( m_day > daysInMonth )
			m_day = 1;

		UpdateDayText();
	}

	private System.DateTime CalcDate()
	{
		int daysInMonthCurrentYear = DateTime.DaysInMonth( DateTime.Now.Year, m_month );
		int clampedDayCurrentYear = Mathf.Clamp( m_day, 1, daysInMonthCurrentYear );

		DateTime conceptionDate = new DateTime( DateTime.Now.Year, m_month, clampedDayCurrentYear );

		DateTime currentDate = new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day );
		if( conceptionDate > currentDate )
		{
			int daysInMonthLastYear = DateTime.DaysInMonth( DateTime.Now.Year - 1, m_month );
			int clampedDayLastYear = Mathf.Clamp( m_day, 1, daysInMonthLastYear );
			conceptionDate = new DateTime( DateTime.Now.Year - 1, m_month, clampedDayLastYear );
		}

		return conceptionDate;
	}

	private void UpdateMonthText()
	{
		m_monthText = LanguageManager.Instance.LookupString( "MonthName" + m_month );
	}

	private void UpdateDayText()
	{
		if( m_day < 10 )
			m_dayText = "0" + m_day;
		else
			m_dayText = "" + m_day;
	}

}