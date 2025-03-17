using Godot;
using System;

public partial class Door : Area2D
{

	// Childs
	private AnimationPlayer _anim;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_anim = GetNode<AnimationPlayer>("Anim");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Callback function of area2D on body entered
	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player")){
			_anim.Play("Opening");
		}
	}

	// Method that goes to the other scene
	public void GoToNextScene()
	{
		TransitionManager transitionManager = GetNode<TransitionManager>("/root/TransitionManager");
		// First, identify the level and depending on which level u are, chooses the next
		string levelName = GetParent().Name;
		switch(levelName){
		case "Level2":
			transitionManager._FadeToScene("res://Scenes/Menu/Credits.tscn");
			break;
		default:
			transitionManager._FadeToScene("res://Scenes/Levels/Level2/level_2.tscn");
			break;
		}
		
	}
}
