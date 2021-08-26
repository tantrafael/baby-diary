using System;
using UnityEngine;

public class Ruler
{
	private const int RULER_LAYER = 8;
	private const int NR_POINTS = 2;
	private const float MM_IN = 1.0f / 25.4f;
	private const float SCALE = 0.00125f;
	private const float TILING_FACTOR = 60.0f;
	private const float RAYCAST_LENGTH = 1.0f;
	private const float MEASURE_CENTER_CORRECTION = -0.0007f;
	private const String MM_FORMAT = "{0:0.0}";
	private const String IN_FORMAT = "{0:0.00}";
	
	private Vector3 center;
	private Vector3 cameraPosition;
	private GameObject cam;
	private GameObject light;
	private Vector3[] endPositions;
	private GameObject[] pointMarkers;
	private GameObject line;
	private GameObject measure;
	private TextMesh textMesh;
	private LineRenderer lineRenderer;
	private int selectedEnd;

	public Ruler()
	{
		center = new Vector3( -1.0f, 0, 0 );
		cameraPosition = center - 0.1f * Vector3.forward;
		endPositions = new Vector3[ NR_POINTS ];
		pointMarkers = new GameObject[ NR_POINTS ];
		selectedEnd = -1;

		GameObject resource;

		resource = ( GameObject ) Resources.Load( "Ruler/RulerCamera" );
		cam = ( GameObject ) MonoBehaviour.Instantiate( resource, cameraPosition, Quaternion.identity );
		cam.name = "RulerCamera";

		resource = ( GameObject ) Resources.Load( "Ruler/RulerLight" );
		light = ( GameObject ) MonoBehaviour.Instantiate( resource, cameraPosition, Quaternion.identity );
		light.name = "RulerLight";

		resource = ( GameObject ) Resources.Load( "Ruler/Line" );
		line = ( GameObject ) MonoBehaviour.Instantiate( resource );
		line.name = "Line";
		lineRenderer = ( LineRenderer ) line.GetComponent( typeof( LineRenderer ) );

		resource = ( GameObject ) Resources.Load( "Ruler/Measure" );
		measure = ( GameObject ) MonoBehaviour.Instantiate( resource );
		measure.name = "Measure";
		textMesh = ( TextMesh ) measure.GetComponent( typeof( TextMesh ) );
		textMesh.renderer.material.color = new Color( 1.0f, 1.0f, 0.2f );

		resource = ( GameObject ) Resources.Load( "Ruler/PointMarker" );

		for( int i = 0; i < NR_POINTS; ++i )
		{
			GameObject marker = ( GameObject ) MonoBehaviour.Instantiate( resource );
			marker.name = "PointMarker" + i;
			pointMarkers[ i ] = marker;
		}

		resource = null;
	}

	public int SelectedEnd
	{
		get
		{
			return selectedEnd;
		}
	}

	public void SelectEnd( Vector2 P )
	{
		Vector3 R = center + SCALE * ( Vector3 ) P;
		R.z = cameraPosition.z;
		Ray ray = new Ray( R, Vector3.forward );
		RaycastHit hit;
		Physics.Raycast( ray, out hit, RAYCAST_LENGTH );

		if( hit.transform != null )
		{
			selectedEnd = Array.IndexOf( pointMarkers, hit.transform.gameObject );
		}
		else
		{
			selectedEnd = -1;
		}
	}

	public void Update( Vector2[] touchPositions )
	{
		if( touchPositions != null )
		{
			switch( touchPositions.Length )
			{
				case 1:
				{
					if( selectedEnd >= 0 )
					{
						endPositions[ selectedEnd ] = center + SCALE * ( Vector3 ) touchPositions[ 0 ];
					}

					break;
				}

				default:
				{
					for( int i = 0; i < touchPositions.Length; ++i )
					{
						endPositions[ i ] = center + SCALE * ( Vector3 ) touchPositions[ i ];
					}

					break;
				}
			}

			for( int i = 0; i < NR_POINTS; ++i )
			{
				pointMarkers[ i ].transform.position = endPositions[ i ];
				lineRenderer.SetPosition( i, endPositions[ i ] );
			}
	
			int leftMostIndex;
			int rightMostIndex;

			if( endPositions[ 1 ].x > endPositions[ 0 ].x )
			{
				leftMostIndex = 0;
				rightMostIndex = 1;
			}
			else
			{
				leftMostIndex = 1;
				rightMostIndex = 0;
			}

			Vector3 D = endPositions[ rightMostIndex ] - endPositions[ leftMostIndex ];
			float d = D.magnitude;
			lineRenderer.material.mainTextureScale = new Vector2( TILING_FACTOR * d, 1.0f );

			measure.transform.position = endPositions[ leftMostIndex ] + 0.5f * D;
			measure.transform.rotation = Quaternion.FromToRotation( Vector3.right, D );
			measure.transform.position += MEASURE_CENTER_CORRECTION * measure.transform.up;

			float distance_mm = 100.0f * d;
			float distance_in = distance_mm * MM_IN;
			textMesh.text = String.Format( MM_FORMAT, distance_mm ) + " mm\n" + String.Format( IN_FORMAT, distance_in ) + " in";
		}
	}

	public void Destroy()
	{
		UnityEngine.Object.Destroy( cam );
		UnityEngine.Object.Destroy( light );
		UnityEngine.Object.Destroy( line );
		UnityEngine.Object.Destroy( measure );

		foreach( GameObject marker in pointMarkers )
		{
			UnityEngine.Object.Destroy( marker );
		}
	}
}
