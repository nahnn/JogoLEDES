using Godot;
using System;

public partial class Credits : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("%Back").Pressed += _BackToTitle;
	}

	private void _BackToTitle(){
		TransitionManager transitionManager = GetNode<TransitionManager>("/root/TransitionManager");
		transitionManager._FadeToScene("res://Scenes/Menu/MainMenu.tscn");
	}
}
