using UnityEngine;

public class Main
{
	private InputManager inputManager = InputManager.Instance;
	private GuiManager guiManager = GuiManager.Instance;
	private PrefsManager prefsManager = PrefsManager.Instance;
	private DiaryManager diaryManager = DiaryManager.Instance;
	private MainStateManager mainStateManager = MainStateManager.Instance;

	public Main()
	{
		iPhoneSettings.screenOrientation = iPhoneScreenOrientation.Portrait;
		RenderSettings.ambientLight = new Color( 0.3f, 0.2f, 0.1f );
		Physics.gravity = Vector3.zero;

		prefsManager.Load();

		bool conceptionDateSet = prefsManager.IsConceptionDateSet();
	//	bool conceptionDateSet = false;

		if( !conceptionDateSet )
			mainStateManager.EnterState( MainStateManager.ENTER );
		else
			mainStateManager.EnterState( MainStateManager.BABY );
	}

	public void FixedUpdate()
	{
	//	inputManager.Update();
		mainStateManager.FixedUpdate();
	}

	public void Update()
	{
		inputManager.Update();
		guiManager.Update();
		mainStateManager.Update();
	}

	public void OnGUI()
	{
		guiManager.OnGUI();
	}

	public void Destroy()
	{
		mainStateManager.Destroy();
		inputManager.Destroy();
	}
}