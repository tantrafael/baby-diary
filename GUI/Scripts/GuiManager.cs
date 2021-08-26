using System;
using UnityEngine;

public class GuiManager
{
	public const int VIEW_NULL = 0;
	public const int VIEW_SELECT_LANGUAGE = 1;
	public const int VIEW_CONCEPTION_DATE = 2;
	public const int VIEW_MENU = 3;
	public const int VIEW_DEVELOPMENT_INFO = 4;
	public const int VIEW_DIARY = 5;
	public const int VIEW_DIARY_ENTRY = 6;
	public const int VIEW_DIARY_SELECT_DATE = 7;
	public const int VIEW_DIARY_BROWSE_ENTRIES = 8;
	public const int VIEW_NAMES = 9;

	private static GuiManager m_instance = null;

	private int m_selectedWeek = 1;

	private GuiView m_view = null;

//	private int currentStateId = -1;

	public event EventHandler ChangeWeek;

	private GuiManager()
	{
	//	Debug.Log( GUI.skin.box );
	}

	public static GuiManager Instance
	{
		get
		{
			if( m_instance == null )
			{
				m_instance = new GuiManager();
			}
			
			return m_instance;
		}
	}

	public GuiView CurrentView
	{
		get
		{
			return m_view;
		}
	}

/*
	public void EnterState( int stateId )
	{}

	public void Update()
	{}
*/
	public int GetSelectedWeek()
	{
		return m_selectedWeek;
	}

	public void OpenViewSelectLanguage()
	{
		GuiViewSelectLanguage guiViewSelectLanguage = new GuiViewSelectLanguage();
		OpenView( guiViewSelectLanguage );
	}

	public void OpenViewConceptionDate()
	{
		GuiViewConceptionDate guiViewConceptionDate = new GuiViewConceptionDate();
		OpenView( guiViewConceptionDate );
	}

	public void OpenViewMenu()
	{
		GuiViewMenu guiViewMenu = new GuiViewMenu();
		guiViewMenu.m_previousWeekHandlers += new GuiViewMenu.PreviousWeekHandler( HandlePreviousWeek );
		guiViewMenu.m_nextWeekHandlers += new GuiViewMenu.NextWeekHandler( HandleNextWeek );
	//	guiViewMenu.MenuSelection += new MenuSelectionEventHandler( HandleMenuSelection );
		OpenView( guiViewMenu );
	}

	public void OpenViewDiary()
	{
		GuiViewDiary viewDiary = new GuiViewDiary();
		OpenView( viewDiary );
	}

	public void OpenViewDiaryEntry( System.DateTime dateTime )
	{
		GuiViewDiaryEntry viewDiaryEntry = new GuiViewDiaryEntry( dateTime );
		OpenView( viewDiaryEntry );
	}

	public void OpenViewDiarySelectDate()
	{
		GuiViewDiarySelectDate viewDiarySelectDate = new GuiViewDiarySelectDate();
		OpenView( viewDiarySelectDate );
	}

	public void OpenViewDiaryBrowseEntries()
	{
		GuiViewDiaryBrowseEntries viewDiaryBrowseEntries = new GuiViewDiaryBrowseEntries();
		OpenView( viewDiaryBrowseEntries );
	}

	public void OpenViewNames()
	{
		GuiViewNames viewNames = new GuiViewNames();
		OpenView( viewNames );
	}

	public void OpenViewNamesRemove()
	{
		GuiViewNamesRemove viewNamesRemove = new GuiViewNamesRemove();
		OpenView( viewNamesRemove );
	}

	private void OpenView( GuiView view )
	{
		CloseView();

		m_view = view;

		if( m_view != null )
			m_view.Close += new EventHandler( HandleGuiViewClose );
	}

	public void CloseView()
	{
		if( m_view != null )
		{
			m_view.Destroy();
			m_view = null;
		}
	}

/*
	public void HandleInput()
	{
		if( m_view != null )
		{
			m_view.HandleInput();
		}
	}
*/

	public void Update()
	{
		if( m_view != null )
			m_view.Update();
	}

	public void OnGUI()
	{
		GUI.skin = ( GUISkin ) Resources.Load( "GUI/GUISkin" );
	//	GUI.contentColor = Color.yellow;
	//	GUIStyle boxStyle = GUI.skin.box;
	//	GUI.contentColor = Color.red;
	//	GUI.skin = new GUISkin();
		if( m_view != null )
		{
			if( !m_view.InitGuiCalled() )
				m_view.InitGui();
			m_view.OnGUI();
		}
	}

	public void Destroy()
	{}

	public void HandleGuiViewClose( object sender, EventArgs e )
	{
	//	Debug.Log( sender );
		OpenViewMenu();
	}

	private void HandlePreviousWeek()
	{
		if( m_selectedWeek > 1 )
		{
			m_selectedWeek--;
			OnChangeWeek();
		}
	}

	private void HandleNextWeek()
	{
		if( m_selectedWeek < 40 )
		{
			m_selectedWeek++;
			OnChangeWeek();
		}
	}

	protected virtual void OnChangeWeek()
	{
		if( ChangeWeek != null )
		{
			ChangeWeek( this, EventArgs.Empty );
		}
	}
}