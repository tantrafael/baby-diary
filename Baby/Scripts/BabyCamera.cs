using UnityEngine;

public class BabyCamera
{
	private const float RADIUS = 0.25f;
	private const float SPEED = 0.2f;
	private const float FRICTION = 1.2f;
	private const float RESTITUTION = 0.05f;

	private GameObject cam;
	private GameObject guide;
	private GameObject pointLight;
	private GameObject directionalLight;

	private Vector3 T;
	private float angle;
	private Vector3 axis;
	private Quaternion R;

	public BabyCamera()
	{
		guide = new GameObject( "BabyCameraGuide" );
		Rigidbody body = ( Rigidbody ) guide.AddComponent( typeof( Rigidbody ) );
		body.useGravity = false;
		body.drag = body.angularDrag = 0;

		GameObject resource = ( GameObject ) Resources.Load( "Baby/BabyCamera" );
		cam = ( GameObject ) MonoBehaviour.Instantiate( resource );
		cam.name = "BabyCamera";
		resource = null;

		Light light = null;

		pointLight = new GameObject( "BabyPointLight" );
		light = ( Light ) pointLight.AddComponent( typeof( Light ) );
		light.type = LightType.Point;
		light.intensity = 0.75f;
		light.color = new Color( 1.0f, 0.9f, 0.8f );

	/*
		directionalLight = new GameObject( "BabyDirectionalLight" );
		light = ( Light ) directionalLight.AddComponent( typeof( Light ) );
		light.type = LightType.Directional;
		light.intensity = 0.2f;
	*/

		light = null;
	}

	public Transform transform
	{
		get
		{
			return guide.transform;
		}
	}

	public Vector3 position
	{
		get
		{
			return cam.transform.position;
		}
	}

	public void Spin( Vector2 input )
//	public void Spin( Vector3 W )
	{
	//	rigidbody.AddForce( input.x * transform.right );
		T = SPEED * ( input.x * Vector3.up - input.y * guide.transform.right );
	//	T = SPEED * ( input.x * Vector3.up );
		guide.rigidbody.AddTorque( T );
	}

//	public void Update()
	public void FixedUpdate()
	{
		R = Quaternion.FromToRotation( guide.transform.up, Vector3.up );
		R.ToAngleAxis( out angle, out axis );
		T = RESTITUTION * angle * axis;
		T -= FRICTION * guide.rigidbody.angularVelocity;
		guide.rigidbody.AddTorque( T );
	}

	public void Update()
	{
		cam.transform.position = -RADIUS * guide.transform.forward;
		cam.transform.LookAt( Vector3.zero, Vector3.up );

		pointLight.transform.position = RADIUS * ( guide.transform.right - 0.5f * guide.transform.forward );
	//	directionalLight.transform.rotation = cam.transform.rotation;
	}

	public void Destroy()
	{}
}