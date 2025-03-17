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
    private float _strength = 3.0f;

    // Move Properties
    private float _speed = 3000.0f;
    private float _dir;
    private Godot.Vector2 _velocity;
    

    // Child Vars
    private RayCast2D _forward;

    public override void _Ready()
	{
		// Getting childs
		_forward = GetNode<RayCast2D>("Forward");

		// Setting inicial behaviors
		_state = state.MOVING;
        
	
		
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
        _velocity.X = _speed * (float)delta;
        Velocity = _velocity;
        if (_forward.IsColliding()){
            //then flip
            Flip();
        }
    }

    // Callback function of hitbox Body entered
    private void OnHitboxBodyEntered(Node2D body)
    {
        if (body is Player player){
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
