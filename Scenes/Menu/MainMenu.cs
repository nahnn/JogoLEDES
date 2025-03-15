using Godot;


public partial class MainMenu : Control {
	public override void _Ready(){
		GetNode<Button>("%Start").Pressed += _Play;
		GetNode<Button>("%Quit").Pressed += _QuitGame;
				GetNode<Button>("%Quit").Pressed += _QuitGame;
	}


	private void _Play()
	{
		TransitionManager transitionManager = GetNode<TransitionManager>("/root/TransitionManager");
		transitionManager._FadeToScene("res://Scenes/Player/newPlayer.tscn");
	}

	private void _QuitGame(){
		GetTree().Quit();
	}

}
