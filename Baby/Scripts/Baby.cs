using UnityEngine;

public class Baby
{
	private const float SPEED = 1.2f;
	private const float DRAG = 1.2f;
	private const float RESTITUTION = 0.04f;
	private const int STAGES = 10;

	private Transform transform;
	private Rigidbody rigidbody;
	private Vector3 A;
	private Vector3 T;
	private float angle;
	private Vector3 axis;
	private Quaternion R;
	private GameObject model;

/*
	public Baby( int week )
	{
		if( week < 3 )
		{
			stage = 0;
		}
		else if( week < 9 )
		{
			stage = week - 3;
		}
		else if( week < 16 )
		{
			stage = 6;
		}
		else if( week < 20 )
		{
			stage = 7;
		}
		else if( week < 37 )
		{
			stage = 8;
		}
		else
		{
			stage = 9;
		}

		GameObject resource = ( GameObject ) Resources.Load( "Baby/Stages/Baby" + stage );
		GameObject gameObject = ( GameObject ) MonoBehaviour.Instantiate( resource );
		resource = null;

		gameObject.name = "Baby";

	//	rigidbody = gameObject.rigidbody;

		rigidbody = ( Rigidbody ) gameObject.AddComponent( typeof( Rigidbody ) );
		rigidbody.useGravity = false;
		rigidbody.mass = 1.0f;
		rigidbody.drag = 0;
		rigidbody.angularDrag = 0;
		rigidbody.inertiaTensor = Vector3.one;
	//	rigidbody.angularVelocity = new Vector3( 0, 0.1f, 0 );

		transform = gameObject.transform;
	}
*/

	public Baby( int week )
	{
		SetAge( week );
	}

	public void SetAge( int week )
	{
		Quaternion rotation = Quaternion.identity;
		Vector3 angularVelocity = Vector3.zero;
		int stage;

		if( week < 3 )
		{
			stage = 0;
		}
		else if( week < 9 )
		{
			stage = week - 3;
		}
		else if( week < 16 )
		{
			stage = 6;
		}
		else if( week < 20 )
		{
			stage = 7;
		}
		else if( week < 37 )
		{
			stage = 8;
		}
		else
		{
			stage = 9;
		}

		if( rigidbody != null )
		{
		//	rotation = rigidbody.rotation;
			rotation = transform.rotation;
			angularVelocity = rigidbody.angularVelocity;
			Object.Destroy( rigidbody.gameObject );
			rigidbody = null;
			transform = null;
		}

		GameObject resource = ( GameObject ) Resources.Load( "Baby/Stages/Baby" + stage );
		GameObject gameObject = ( GameObject ) MonoBehaviour.Instantiate( resource, Vector3.zero, rotation );
		gameObject.name = "Baby";
		resource = null;

		rigidbody = ( Rigidbody ) gameObject.AddComponent( typeof( Rigidbody ) );
		rigidbody.useGravity = false;
		rigidbody.mass = 1.0f;
		rigidbody.drag = 0;
		rigidbody.angularDrag = 0;
		rigidbody.inertiaTensor = Vector3.one;
		rigidbody.angularVelocity = angularVelocity;

		transform = gameObject.transform;
	}

/*
	public Baby( int week )
	{
		GameObject gameObject = new GameObject( "Baby" );

		rigidbody = ( Rigidbody ) gameObject.AddComponent( typeof( Rigidbody ) );
		rigidbody.useGravity = false;
		rigidbody.mass = 1.0f;
		rigidbody.drag = 0;
		rigidbody.angularDrag = 0;

		transform = gameObject.transform;

		SetAge( week );
	}

	public void SetAge( int week )
	{
		int stage;

		if( week < 1 )
		{
			stage = 0;
		}
		else if( week <= STAGES )
		{
			stage = week - 1;
		}
		else
		{
			stage = STAGES - 1;
		}

		if( model != null )
		{
			Object.Destroy( model );
		}

		GameObject resource = ( GameObject ) Resources.Load( "Baby/Stages/Baby" + stage );
		model = ( GameObject ) MonoBehaviour.Instantiate( resource );
		resource = null;
		model.name = "Baby" + stage;
		model.transform.parent = transform;
	}
*/

	public void Turn( Vector3 input )
//	public void Turn( Vector3 A )
	{
	//	T = SPEED * ( input.x * Vector3.up - input.y * Vector3.right );
		T = SPEED * ( input.x * Vector3.up - input.y * transform.forward );
	//	T = SPEED * A;
		rigidbody.AddTorque( T );
	}

	public void FixedUpdate()
	{
		R = Quaternion.FromToRotation( transform.up, Vector3.up );
		R.ToAngleAxis( out angle, out axis );
		T = Vector3.zero;
	//	T += 0.1f * Random.insideUnitSphere;
		T += RESTITUTION * angle * axis;
		T -= DRAG * rigidbody.angularVelocity;
		rigidbody.AddTorque( T );
	}

	public void Destroy()
	{}
}