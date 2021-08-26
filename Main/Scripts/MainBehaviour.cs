using UnityEngine;

public class MainBehaviour : MonoBehaviour
{
	private Main main = null;

	public void Awake()
	{
		main = new Main();
	}

	public void FixedUpdate()
	{
		main.FixedUpdate();
	}

	public void Update()
	{
		main.Update();
	}

	public void OnGUI()
	{
		main.OnGUI();
	}

	public void OnApplicationQuit()
	{
		main.Destroy();
		main = null;
	}
}