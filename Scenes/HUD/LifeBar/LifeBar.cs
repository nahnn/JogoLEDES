using Godot;
using System;

public partial class LifeBar : ColorRect
{
	// Getting Child
	public ColorRect _life;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Getting childs
		_life = GetNode<ColorRect>("Life");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
