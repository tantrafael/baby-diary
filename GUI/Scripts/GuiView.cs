using System;
using UnityEngine;

public class GuiView
{
	public event EventHandler Close;

	protected bool m_initGuiCalled = false;

	public GuiView()
	{}

	public bool InitGuiCalled()
	{
		return m_initGuiCalled;
	}

	public virtual void InitGui()
	{
		m_initGuiCalled = true;
	}

	protected virtual void OnClose()
	{
		if( Close != null )
		{
			Close( this, EventArgs.Empty );
		}
	}

/*
	public virtual void HandleInput()
	{}
*/

	public virtual void Update()
	{}

	public virtual void OnGUI()
	{}

	public virtual void Destroy()
	{}
}