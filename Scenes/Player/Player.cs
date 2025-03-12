using Godot;
using System;

public partial class Player : CharacterBody2D
{
	// Declaring states
	enum state{
		MOVING,
		JUMPING,
		FALLING,
		DASHING,
		CLIMBING
	}


	// Declaring move properties
	private state _state;
	private float _initialMoveSpeed = 500.0f;
	private float _maxMoveSpeed = 1500.0f;
	private float _currentSpeed;
	private float _acelleration = 1000.0f;
	private float _dashSpeed = 2000.0f;
	private bool _canJump = true;
	private bool _canDash = true;
	private Godot.Vector2 _input = Godot.Vector2.Zero;
	private Godot.Vector2 _dir;
	private Godot.Vector2 _velocity;
	
	// Childs variables
	private Sprite2D _sprite;
	private Area2D _wallDetector;
	private Area2D _itemsDetector;
	private AnimationPlayer _anim;
	private Timer _dashCooldown;

	public override void _Ready()
	{
		// Getting childs
		_sprite = GetNode<Sprite2D>("Sprite");
		_wallDetector = GetNode<Area2D>("WallDetector");
		_itemsDetector = GetNode<Area2D>("ItemsDetector");
		_anim = GetNode<AnimationPlayer>("Anim");
		_dashCooldown = GetNode<Timer>("DashCooldownTimer");

		// Setting inicial behaviors
		_state = (int)state.MOVING;
		PlayAnim("Idle");
	}

	public override void _Process(double delta)
	{
		switch (_state){
			case state.JUMPING:
				break;
			case state.FALLING:
				break;
			case state.DASHING:
				break;
			case state.CLIMBING:
				break;
			default:
				Moving(delta);
				break;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}

	// Function that matches the moving state
	private void Moving(double delta)
	{
		// Getting player input and setting direction
		_dir = _input;
		_input.X = Input.GetAxis("ui_left", "ui_right");
		
		// Dealling with horizontal speed
		HorMove(delta);
	}

	// Function that matches the jumping state
	private void Jumping(double delta)
	{

	}

	// Function that matches the falling state
	private void Falling(double delta)
	{

	}

	// Function that matches the dashing state
	private void Dashing(double delta)
	{

	}

	// Function that matches the climbing state
	private void Climbing(double delta)
	{

	}

	// Function that deals with the horizontal movement
	private void HorMove(double delta)
	{
		
		// if the movement is just starting
		if (_dir.X == 0 && _input.X != 0){
			// then, start from inicial velocity
			_velocity.X = _input.X * _initialMoveSpeed * (float)delta;
			_currentSpeed = _velocity.X;
		}
		else if (_dir.X != 0 && _input.X != 0){
			// if the player was moving and didnt reached the max velocity, then just aceleratte 
			if (_velocity.X < _maxMoveSpeed){
				_currentSpeed += _acelleration;
				_velocity.X = _input.X * _currentSpeed * (float)delta;
			}
			else{
				// Otherwise, set velocity to the max velocity
				_velocity.X = _maxMoveSpeed;
			}
		}
		else{
			// Otherwise, player will stop
			_velocity.X = _input.X;
		}
		Velocity = _velocity;
		return;
	}

	// Function that plays an animation of the node
	private void PlayAnim(string name)
	{
		var anim = name;
		_anim.Play(anim);
		return;
	}

	// Function that flips the player
	private void Flip()
	{}	
}
