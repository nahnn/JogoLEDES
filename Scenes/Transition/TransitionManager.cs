using Godot;

public partial class TransitionManager : CanvasLayer
{
	private string scene;

	public void _FadeToScene(string novaCena)
	{
		scene = novaCena;
		var animationPlayer = GetNode<AnimationPlayer>("%animas");
		animationPlayer.Play("FadeInOut");
	}

	private void _ChangeScene()
	{
		GetTree().ChangeSceneToFile(scene);
	}
}
