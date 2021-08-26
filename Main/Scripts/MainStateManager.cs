public class MainStateManager
{
	public const int INTRO = 0;
	public const int ENTER = 1;
	public const int BABY  = 2;
	public const int INFO  = 3;
	public const int DIARY = 4;
	public const int NAMES = 5;

	private static MainStateManager instance;

	private int currentStateId = -1;
	private MainState currentState = null;

	private MainStateManager()
	{}

	public static MainStateManager Instance
	{
		get
		{
			if( instance == null )
			{
				instance = new MainStateManager();
			}
			
			return instance;
		}
	}

	public void EnterState( int stateId )
	{
		if( currentStateId != stateId )
		{
			currentStateId = stateId;

			if( currentState != null )
			{
				currentState.Destroy();
			}

			switch( stateId )
			{
				case ENTER:
				{
					currentState = new MainStateEnter();
					break;
				}
				case BABY:
				{
					currentState = new MainStateBaby();
					break;
				}
			}
		}
	}

	public void FixedUpdate()
	{
		if( currentState != null )
		{
			currentState.FixedUpdate();
		}
	}

	public void Update()
	{
		if( currentState != null )
		{
			currentState.Update();
		}
	}

	public void SendMessage( string message )
	{
		if( currentState != null )
		{
			currentState.ReceiveMessage( message );
		}
	}
	public void Destroy()
	{
		if( currentState != null )
		{
			currentState.Destroy();
			currentState = null;
			currentStateId = -1;
		}
	}
}