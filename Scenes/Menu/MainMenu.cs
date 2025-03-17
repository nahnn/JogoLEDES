using Godot;


public partial class MainMenu : Control {
	public override void _Ready(){
		GetNode<Button>("%Start").Pressed += _Play;
		GetNode<Button>("%Quit").Pressed += _QuitGame;
		GetNode<Button>("%Credits").Pressed += _Credits;
	}


	private void _Play()
	{
		TransitionManager transitionManager = GetNode<TransitionManager>("/root/TransitionManager");
        transitionManager._FadeToScene("res://Scenes/Levels/Level1/level_1.tscn");
	}

	private void _QuitGame(){
		GetTree().Quit();
	}
	
	private void _Credits(){
		TransitionManager transitionManager = GetNode<TransitionManager>("/root/TransitionManager");
		transitionManager._FadeToScene("res://Scenes/Menu/Credits.tscn");
	}

}
