using UnityEngine;

public class Floaters
{
	private const int N = 8;
	private Floater[] floaters;
	private Floater floater;

	public Floaters()
	{
		floaters = new Floater[ N ];

		for( int i = 0; i < N; ++i )
		{
			floaters[ i ] = new Floater( i );
		}

		floater = null;
	}

	public void Turn( Vector3 T )
	{
		for( int i = 0; i < N; ++i )
		{
			floater = floaters[ i ];
			floater.Turn( T );
		}
	}

	public void FixedUpdate()
	{
		for( int i = 0; i < N; ++i )
		{
			floater = floaters[ i ];
			floater.FixedUpdate();
		}
	}

	public void Focus( Vector3 P )
	{
		for( int i = 0; i < N; ++i )
		{
			floater = floaters[ i ];
			floater.Focus( P );
		}
	}

	public void Destroy()
	{
		for( int i = 0; i < N; ++i )
		{
			floater = floaters[ i ];
			floater.Destroy();
			floaters[ i ] = null;
		}

		floaters = null;
	}
}