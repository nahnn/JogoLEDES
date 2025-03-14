using Godot;
using System;

public partial class Item : Area2D
{
	// Declearing properties
	private bool _selected = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Callback function of input
	private void OnInputEvent(Node viewport, InputEvent Event, int shapeIdx)
	{
		if (Event is InputEventScreenTouch && Event.IsPressed())
		{
			_selected = !_selected;
		}
	}
}
