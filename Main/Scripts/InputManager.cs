using System;
using UnityEngine;

public class TouchCountChangeArgs : EventArgs
{
	private readonly int touchCount;

	public TouchCountChangeArgs( int touchCount )
	{
		this.touchCount = touchCount;
	}

	public int TouchCount
	{
		get { return touchCount; }
	}
}

/*
public class DragEventArgs : EventArgs
{
	private readonly int[] indices;

	public DragEventArgs( int[] indices )
	{
		this.indices = indices;
	}

	public Vector2 Indices
	{
		get { return indices; }
	}
}
*/

/*
public class DragEventArgs : EventArgs
{
	private readonly Vector2 drag;

	public DragEventArgs( Vector2 drag )
	{
		this.drag = drag;
	}

	public Vector2 Drag
	{
		get { return drag; }
	}
}
*/

//public delegate void DragEventHandler( object sender, DragEventArgs e );
public delegate void DragEventHandler( object sender, EventArgs e );
public delegate void TouchCountChangeHandler( object sender, TouchCountChangeArgs e );

public class InputManager
{
	public event DragEventHandler Drag;
//	public event AccelerationEventHandler Acceleration;
	public event TouchCountChangeHandler TouchCountChange;

	private const float MOUSE_DRAG_FACTOR = 40.0f;
	private const float ACCELERATION_SMOOTHING = 0.75f;
	private const float NATURAL_LEAN_ANGLE = 45.0f;
	private const float R_1 = 1.0f / 300.0f;
	private const int MAX_TOUCH_COUNT = 2;

	private static InputManager instance;

	private Vector2 screenCenter;
	private bool simulation;
	private Vector3 A;
	private Vector3 acceleration;
	private Quaternion naturalLean;
	private Vector2 drag;

	private Vector2 [] touchPositions;
	private Vector2 [] deltaPositions;
	private bool changeTouchCount;
	private bool dragDetected;
	private int touchCount;
	private int inputTouchCount;

	private InputManager()
	{
		simulation = ( iPhoneInput.orientation == iPhoneOrientation.Unknown );
		screenCenter = 0.5f * new Vector2( Screen.width, Screen.height );
		naturalLean = Quaternion.AngleAxis( NATURAL_LEAN_ANGLE, Vector3.right );
	//	touchPositions = new Vector2 [ MAX_TOUCH_COUNT ];
	//	deltaPositions = new Vector2 [ MAX_TOUCH_COUNT ];
		touchCount = 0;
		changeTouchCount = false;
		dragDetected = false;
		drag = Vector2.zero;
	}

	public static InputManager Instance
	{
		get
		{
			if( instance == null )
			{
				instance = new InputManager();
			}
			
			return instance;
		}
	}

	public Quaternion NaturalLean
	{
		get { return naturalLean; }
	}

	public Vector3 Acceleration
	{
		get { return acceleration; }
	}

	public int TouchCount
	{
		get
		{
			return touchCount;
		}
	}

	public Vector2[] TouchPositions
	{
		get
		{
			Vector2[] touchPositions = null;

			if( touchCount > 0 )
			{
				int n = Mathf.Min( touchCount, MAX_TOUCH_COUNT );
				touchPositions = new Vector2[ n ];

				if( simulation == false )
				{
					for( int i = 0; i < n; i++ )
					{
						touchPositions[ i ] = iPhoneInput.GetTouch( i ).position - screenCenter;
					}
				}
				else
				{
					touchPositions[ 0 ] = new Vector2( Input.mousePosition.x, Input.mousePosition.y ) - screenCenter;

					if( n > 1 )
					{
						touchPositions[ 1 ] = -touchPositions[ 0 ];
					}

				/*
					int i = 2;

					while( i < touchCount )
					{
						touchPositions[ i ] = Vector2.zero;
						i++;
					}
				*/
				}
			}

			return touchPositions;
		}
	}

	public Vector2 GetTouchPosition( int index )
	{
		Vector2 P = Vector2.zero;

		if( simulation == false )
		{
			if( iPhoneInput.touchCount > index )
			{
				P = iPhoneInput.GetTouch( index ).position - screenCenter;
			}
		}
		else
		{
		//	if( touchCount > index )
			if( Input.GetMouseButton( index ) == true )
			{
				P = ( Vector2 ) Input.mousePosition - screenCenter;
			}
		}

		return P;
	}

	public Vector2 GetDeltaPosition( int index )
	{
		Vector2 D = Vector2.zero;

		if( simulation == false )
		{
			if( iPhoneInput.touchCount > index )
			{
				D = iPhoneInput.GetTouch( index ).deltaPosition;
			}
		}
		else
		{
			if( Input.GetMouseButton( index ) == true )
			{
				D.x = MOUSE_DRAG_FACTOR * Input.GetAxis( "Mouse X" );
				D.y = MOUSE_DRAG_FACTOR * Input.GetAxis( "Mouse Y" );
			}
		}

		return D;
	}

/*
	protected virtual void OnAccelerate( AccelerationEventArgs e )
	{
		if( Acceleration != null )
		{
			Acceleration( this, e );
		}
	}
*/

//	protected virtual void OnDrag( DragEventArgs e )
	protected virtual void OnDrag( EventArgs e )
	{
		if( Drag != null )
		{
			Drag( this, e );
		}
	}

	protected virtual void OnTouchCountChange( TouchCountChangeArgs e )
	{
		if( TouchCountChange != null )
		{
			TouchCountChange( this, e );
		}
	}

	public void Update()
	{
	//	Debug.Log( "InputManager.Update()" );
	//	drag = Vector2.zero;

		dragDetected = false;
		drag = Vector2.zero;

		if( simulation == false )
		{
			A = iPhoneInput.acceleration;
			A.z = -A.z;
		//	acceleration = Vector3.Lerp( acceleration, A, ACCELERATION_SMOOTHING );
			acceleration = A;

			inputTouchCount = iPhoneInput.touchCount;

			if( inputTouchCount > 0 )
			{
				int n = Mathf.Min( inputTouchCount, MAX_TOUCH_COUNT );
				int i = 0;

				while( ( dragDetected == false ) && ( i < n ) )
				{
					if( iPhoneInput.GetTouch( i ).phase == iPhoneTouchPhase.Moved )
					{
						dragDetected = true;
					}

					++i;
				}
			}
		}
		else
		{
			A = Vector3.zero;
			A.x = R_1 * ( Input.mousePosition.x - screenCenter.x );
			A.y = R_1 * ( Input.mousePosition.y - screenCenter.y );

			if( A.sqrMagnitude < 1.0f )
			{
				A.z = Mathf.Sqrt( 1.0f - A.sqrMagnitude );
			}
			else
			{
				A.Normalize();
			}

			acceleration = naturalLean * A;

			inputTouchCount = 0;

			for( int i = 0; i < MAX_TOUCH_COUNT; ++i )
			{
				if( Input.GetMouseButton( i ) == true )
				{
					++inputTouchCount;

				/*
					Vector2 P = new Vector2( Input.GetAxis( "Mouse X" ), Input.GetAxis( "Mouse Y" ) );
					Vector2 D = P - touchPositions[ i ];

					if( D.sqrMagnitude > 0 )
					{
						deltaPosition[ i ] = DRAG_FACTOR * new Vector2( Input.GetAxis( "Mouse X" ), Input.GetAxis( "Mouse Y" ) );
					}
				*/
				}
			}

		//	Vector3 P = Vector3.right;

			if( inputTouchCount > 0 )
			{
				if( ( Input.GetAxis( "Mouse X" ) > 0 ) || ( Input.GetAxis( "Mouse Y" ) > 0 ) )
				{
				//	Debug.Log( "InputManager: dragDetected " + DateTime.Now );
				//	P = 1.0f * new Vector3( Input.GetAxis( "Mouse X" ), Input.GetAxis( "Mouse Y" ), 0 );
				//	dragDetected = true;
				}

			//	Debug.DrawLine( Vector3.zero, P, Color.red );
			}
		}

	//	if( changeTouchCount == true )
		if( touchCount != inputTouchCount )
		{
			touchCount = inputTouchCount;
			TouchCountChangeArgs e = new TouchCountChangeArgs( touchCount );
			OnTouchCountChange( e );
		}

		if( dragDetected == true )
		{
		/*
			Vector2 D = Vector2.zero;

			if( simulation == false )
			{
				D = iPhoneInput.GetTouch( 0 ).deltaPosition;
			}
			else
			{
				D.x = Input.GetAxis( "Mouse X" );
				D.y = Input.GetAxis( "Mouse Y" );
			}

			DragEventArgs e = new DragEventArgs( D );
		*/

		//	DragEventArgs e = new DragEventArgs( drag );
		//	OnDrag( e );
		//	OnDrag( EventArgs.Empty );
		}
	}

	public void Destroy()
	{
	//	instance = null;
	//	iPhoneInput.orientation = iPhoneOrientation.Unknown;
	}
}