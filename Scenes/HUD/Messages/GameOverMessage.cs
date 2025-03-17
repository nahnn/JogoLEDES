using Godot;
using System;

public partial class GameOverMessage : Control
{
	// Getting custom signals variable
	private CustomSignals _customSignals;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		// Definying initial behaviors
		Hide();

		_customSignals.Die += GameOver;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Função de game over	
	public void GameOver()
	{
		//await ToSignal(GetTree().CreateTimer(.2), SceneTreeTimer.SignalName.Timeout);
		Show();
	}
}
