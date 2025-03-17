using Godot;
using System;

public partial class Wings : Area2D
{
	// Exporting commum properties of all items
	[Export] public string _name = "";
	[Export] public Texture2D _icon = null;
	[Export] public bool _isStackable = false;

	// Properties
	public int _moreJumps = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
