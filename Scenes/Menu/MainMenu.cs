using Godot;


public partial class MainMenu : Control {
	public override void _Ready(){
		GetNode<Button>("%Start").Pressed += _Play;
		GetNode<Button>("%Quit").Pressed += _QuitGame;
	}

	private void _Play(){
		GetTree().ChangeSceneToFile("res://Scenes/Level1/level_1.tscn");
	}

	private void _QuitGame(){
		GetTree().Quit();
	}

}
