using Godot;
using System;

public partial class HealthPotion : Area2D
{
	// Exporting commum properties of all items
	[Export] public string _name = "";
	[Export] public Texture2D _icon = null;
	[Export] public bool _isStackable = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("Items");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
