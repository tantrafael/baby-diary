using UnityEngine;

public class Floater
{
	private const float MEAN_MASS = 0.01f;
	private const float ACCELERATION = 0.001f;
	private const float RESTITUTION = 1000.0f;
	private const float SPONTANEITY = 0.0003f;
	private const float DRAG = 0.001f;
	private const float START_SPEED = 0.01f;

	private Transform transform;
	private Rigidbody body;
	private float radius;

	private float m;
	private float v2;
	private float r;
	private float f;
	private Vector3 V;
	private Vector3 R;
	private Vector3 F;

	public Floater( int index )
	{
		GameObject resource = ( GameObject ) Resources.Load( "Baby/Floater" );
		GameObject gameObject = ( GameObject ) MonoBehaviour.Instantiate( resource );
		resource = null;

		gameObject.name = "Floater" + index;

		float scale = 0.5f + 1.0f * Random.value;

		body = gameObject.rigidbody;
		body.mass = scale * MEAN_MASS;
		body.drag = 0;
		body.angularDrag = 0;

		m = body.mass;

		transform = gameObject.transform;
		transform.localScale = scale * Vector3.one;
	//	transform.localScale = 100.0f * body.mass * Vector3.one;

		Vector3 P0 = ( 0.08f + 0.05f * Random.value ) * Vector3.Normalize( Random.insideUnitSphere );
		radius = P0.magnitude;

		R = P0.normalized;
		Vector3 V0 = START_SPEED * Vector3.Cross( R, Random.insideUnitSphere );

		body.position = P0;
		body.velocity = V0;
	}

	public void Turn( Vector3 T )
	{
		F = ACCELERATION * Vector3.Cross( R, T );
		body.AddForce( F );
	}

//	public void Update( Vector3 focus )
	public void FixedUpdate()
	{
		V = body.velocity;
		v2 = V.sqrMagnitude;
		r = body.position.magnitude;
		R = body.position.normalized;
		f = ( 1.0f + RESTITUTION * ( radius - r ) );

		F = Vector3.zero;
		F += f * ( m * v2 / r ) * R;
	//	F += SPONTANEITY * Vector3.Cross( R, Random.insideUnitSphere );
	//	F -= DRAG * V;
		body.AddForce( F );

	//	transform.LookAt( focus );
	}

	public void Focus( Vector3 P )
	{
		transform.LookAt( P );
	}

	public void Destroy()
	{}
}