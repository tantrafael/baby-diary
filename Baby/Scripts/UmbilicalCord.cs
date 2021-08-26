using UnityEngine;

public class UmbilicalCord
{
	private const int N = 16;
	private GameObject[] links;
	private GameObject link;
	private Rigidbody body;
	private Vector3 F;
	private Vector3 T;

	public UmbilicalCord()
	{
		links = new GameObject[ N ];

		float dx = 0.01f;
		float x0 = -0.5f * dx * N;
		Rigidbody body;

		for( int i = 0; i < N; i++ )
		{
		//	link = new GameObject( "Link" + i );
			link = ( GameObject ) MonoBehaviour.Instantiate( Resources.Load( "Baby/UmbelicalCordElement" ) );
			link.name = "Link" + i;
			link.transform.position = new Vector3( x0 + dx * i, 0, 0 );
			body = link.rigidbody;
			body.mass = 0.01f;
			body.inertiaTensor = 0.01f * Vector3.one;
			links[ i ] = link;
		}

		for( int i = 0; i < N - 1; i++ )
	//	for( int i = 0; i < N; i++ )
		{
			link = links[ i ];
		//	link.rigidbody.velocity = 2.0f * Random.insideUnitSphere;
			link.rigidbody.angularVelocity = 1.0f * Random.insideUnitSphere;

			SoftJointLimit linearLimit = new SoftJointLimit();
			linearLimit.limit = 0.1f;

			SoftJointLimit lowAngularXLimit = new SoftJointLimit();
			lowAngularXLimit.limit = -2.0f;
		//	lowAngularXLimit.spring = 10.0f;
		//	lowAngularXLimit.damper = 1.0f;

			SoftJointLimit highAngularXLimit = new SoftJointLimit();
			highAngularXLimit.limit = 2.0f;
		//	lowAngularXLimit.spring = 10.0f;
		//	highAngularXLimit.damper = 1.0f;

			SoftJointLimit angularYLimit = new SoftJointLimit();
			angularYLimit.limit = 30.0f;
		//	angularYLimit.spring = 10.0f;
		//	angularYLimit.damper = 10.0f;

			ConfigurableJoint joint = ( ConfigurableJoint ) link.AddComponent( typeof( ConfigurableJoint ) );
			joint.axis = Vector3.right;
			joint.anchor = new Vector3( 0.5f * dx, 0, 0 );
			joint.linearLimit = linearLimit;
			joint.lowAngularXLimit = lowAngularXLimit;
			joint.highAngularXLimit = highAngularXLimit;
			joint.angularYLimit = angularYLimit;
			joint.angularZLimit = angularYLimit;
			joint.xMotion = ConfigurableJointMotion.Locked;
			joint.yMotion = ConfigurableJointMotion.Locked;
			joint.zMotion = ConfigurableJointMotion.Locked;
			joint.angularXMotion = ConfigurableJointMotion.Limited;
			joint.angularYMotion = ConfigurableJointMotion.Limited;
			joint.angularZMotion = ConfigurableJointMotion.Limited;

			joint.connectedBody = links[ i + 1 ].rigidbody;
		}
	}
	
	public void FixedUpdate()
	{
	//	link = links[ 0 ];
	//	link.rigidbody.AddTorque( 10.0f * ( 1.0f - 2.0f * Random.value ) * Vector3.right );

		for( int i = 0; i < N; i++ )
		{
			link = links[ i ];
			
		//	link.rigidbody.AddForce( 0.1f * Random.insideUnitSphere );
		//	link.rigidbody.AddTorque( 0.1f * Random.insideUnitSphere );
		//	link.rigidbody.AddForce( -0.01f * link.rigidbody.velocity );
		//	link.rigidbody.AddTorque( -0.01f * link.rigidbody.angularVelocity );
		}
	}

	public void Destroy()
	{}
}