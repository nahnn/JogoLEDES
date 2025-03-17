using Godot;
using System;

public partial class Bat : CharacterBody2D
{
    // Declaring states
	enum state{
		MOVING,
	}
	private state _state;

    // status properties
    private float _strength = 5.0f;

    // Move Properties
    private float _speed = 3000.0f;
    private float _dir;
    private Godot.Vector2 _velocity;
    

    // Child Vars
    private RayCast2D _forward;
    private AnimationPlayer _anim;

    public override void _Ready()
	{
		// Getting childs
		_forward = GetNode<RayCast2D>("Forward");
        _anim = GetNode<AnimationPlayer>("Anim");

		// Setting inicial behaviors
		_state = state.MOVING;
        _velocity.X = _speed;
        _anim.Play("Moving");
	
		
	}

    public override void _Process(double delta)
	{
		switch (_state){
			default:
				Moving(delta);
				break;
		}
	}

    public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}

    // Function that deals movement
    private void Moving(double delta)
    {
        Velocity = _velocity * (float)delta;;
        if (_forward.IsColliding()){
            //then flip
            Flip();
        }
    }

    // Callback function of hitbox Body entered
    private void OnHitboxAreaEntered(Node2D body)
    {
        if (body.GetParent() is Player player){
            player.DecreaseHealth(_strength);
        }
    }

    // Function that flips the bat
	private void Flip()
    {
        _velocity *= -1;
        Godot.Vector2 scale = _forward.TargetPosition;
        scale *= -1;
        _forward.TargetPosition = scale;
    }

}
