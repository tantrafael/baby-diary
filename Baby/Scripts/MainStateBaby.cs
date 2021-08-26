using System;
using UnityEngine;

public class MainStateBaby : MainState
{
	private const int GESTATION = 280;
	private const int DAYS_IN_WEEK = 7;

	private const int BABY_STATE = 0;
	private const int RULER_STATE = 1;
	private const int INFO_STATE = 2;

	private int state = -1;
	private int touchMode = -1;
	private int touchCount = 0;

//	private TouchCountChangeHandler touchCountChangeHandler;

	private InputManager inputManager = InputManager.Instance;
	private PrefsManager prefsManager = PrefsManager.Instance;
	private GuiManager guiManager = GuiManager.Instance;

	private Baby baby;
	private UmbilicalCord cord;
	private Floaters floaters;
	private BabyCamera cam;
	private Ruler ruler;

	private Vector3 A;
	private Vector3 T;
	private Quaternion naturalLeanInverse;

	private GuiViewMenu viewMenu;

	public MainStateBaby()
	{
		naturalLeanInverse = Quaternion.Inverse( inputManager.NaturalLean );

	//	touchCountChangeHandler = new TouchCountChangeHandler( HandleTouchCountChange );
	//	inputManager.TouchCountChange += new TouchCountChangeHandler( HandleTouchCountChange );
	//	inputManager.TouchCountChange += touchCountChangeHandler;
	//	inputManager.Drag += new DragEventHandler( HandleInputDrag );
		guiManager.ChangeWeek += new EventHandler( HandleGuiChangeWeek );

		GameObject resource = ( GameObject ) Resources.Load( "Baby/Uterus" );
		GameObject uterus = ( GameObject ) MonoBehaviour.Instantiate( resource );
		uterus.name = "Uterus";
		resource = null;

		float age = prefsManager.GetAge().Days / ( float ) GESTATION;
		int week = ( int ) Mathf.Floor( age * ( float ) GESTATION / ( float ) DAYS_IN_WEEK ) + 1;
	//	int week = 0;

		baby = new Baby( week );
	//	cord = new UmbilicalCord();
		floaters = new Floaters();
		cam = new BabyCamera();

		guiManager.OpenViewMenu();
		viewMenu = ( GuiViewMenu ) guiManager.CurrentView;
		viewMenu.InfoToggle += new InfoToggleHandler( HandleInfoToggle );

		EnterState( BABY_STATE );
	}

	private void EnterState( int state )
	{
		if( this.state != state )
		{
			this.state = state;

			if( state == RULER_STATE )
			{
				if( ruler == null )
				{
					ruler = new Ruler();
				}
			}
			else
			{
				if( ruler != null )
				{
					ruler.Destroy();
					ruler = null;
				}
			}

			inputManager.TouchCountChange -= new TouchCountChangeHandler( HandleTouchCountChange );

			if( ( state == BABY_STATE ) || ( state == RULER_STATE ) )
			{
				inputManager.TouchCountChange += new TouchCountChangeHandler( HandleTouchCountChange );
			}
		}
	}

	private void HandleInfoToggle( object sender, InfoToggleArgs e )
	{
		if( e.Visible == true )
		{
			EnterState( INFO_STATE );
		}
		else
		{
			EnterState( BABY_STATE );
		}
	}

	private void HandleTouchCountChange( object sender, TouchCountChangeArgs e )
	{
		switch( e.TouchCount )
		{
			case 0:
			{
				break;
			}

			case 1:
			{
				if( state == RULER_STATE )
				{
					Vector2 P = inputManager.GetTouchPosition( 0 );
					ruler.SelectEnd( P );

					if( ( touchCount == 0 ) && ( ruler.SelectedEnd < 0 ) )
					{
						EnterState( BABY_STATE );
					}
				}

				break;
			}

			default:
			{
				EnterState( RULER_STATE );
				break;
			}
		}

		touchCount = e.TouchCount;
	}

/*
	private void HandleInputDrag( object sender, DragEventArgs e )
//	private void HandleInputDrag( object sender, EventArgs e )
	{
	//	Vector2 D = inputManager.GetDeltaPosition( 0 );
		Vector2 D = 100.0f * e.Drag;
		Debug.Log( D );

		switch( state )
		{
			case BABY_STATE:
			{
				cam.Spin( D );
				break;
			}

			case INFO_STATE:
			{
				viewMenu.ScrollInfo( D.y );
				break;
			}
		}
	}
*/

	private void HandleGuiChangeWeek( object sender, EventArgs e )
	{
		int week = guiManager.GetSelectedWeek();
		baby.SetAge( week );
	}

	public override void FixedUpdate()
	{
		A = naturalLeanInverse * inputManager.Acceleration;

	//	T = A.x * Vector3.up - A.y * cam.transform.right;
	//	baby.Turn( T );
		baby.Turn( A );
		baby.FixedUpdate();

	//	cord.FixedUpdate();

	//	floaters.Turn( T );
		floaters.FixedUpdate();

		cam.FixedUpdate();
	}

	public override void Update()
	{
		floaters.Focus( cam.position );
		cam.Update();

		if( touchCount > 0 )
		{
			if( state == RULER_STATE )
			{
				ruler.Update( inputManager.TouchPositions );
			}
			else
			{
				Vector2 D = inputManager.GetDeltaPosition( 0 );
		
				if( state == BABY_STATE )
				{
					cam.Spin( D );
				}
				else
				{
					viewMenu.ScrollInfo( D.y );
				}
			}
		}

	/*
		switch( state )
		{

			case BABY_STATE:
			{
				break;
			}

			case RULER_STATE:
			{
				ruler.Update( inputManager.TouchPositions );
				break;
			}

			case INFO_STATE:
			{
				break;
			}
		}
	*/
	}

	public override void Destroy()
	{
		baby.Destroy();
		baby = null;

	//	cord.Destroy();
	//	cord = null;

		floaters.Destroy();
		floaters = null;

		cam.Destroy();
		cam = null;
	}
}