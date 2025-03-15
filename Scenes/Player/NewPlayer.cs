using Godot;
using System;

public partial class NewPlayer : Sprite2D
{
	public override void _Ready()
	{
		GetNode<AnimationPlayer>("PlayerAnimation").Play("animation_idle");
	}
}
