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


	// Declaring status properties
	private float _health = 10.0f;
	

	// Declaring move properties
	private state _state;
	private float _initialMoveSpeed = 500.0f;
	private float _maxMoveSpeed = 1500.0f;
	private float _currentHSpeed;
	private float _acelleration = 1000.0f;
	private float _dashSpeed = 2000.0f;
	private float _jumpSpeed = -50000.0f;
	private float _currentVSpeed = 0;
	private float _gravity = 850.0f;
	private bool _canJump;
	private int _jumpCount = 1;
	private bool _canDash = true;
	private Godot.Vector2 _input = Godot.Vector2.Zero;
	private Godot.Vector2 _prevDir;
	private Godot.Vector2 _velocity;
	
	// Childs variables
	private Sprite2D _sprite;
	private Area2D _wallDetector;
	private Area2D _itemsDetector;
	private AnimationPlayer _anim;
	private Timer _dashCooldown;
	private Timer _jumpTimer;
	private Inventory _inventory;

	public override void _Ready()
	{
		// Getting childs
		_sprite = GetNode<Sprite2D>("Sprite");
		_wallDetector = GetNode<Area2D>("WallDetector");
		_itemsDetector = GetNode<Area2D>("ItemsDetector");
		_anim = GetNode<AnimationPlayer>("Anim");
		_dashCooldown = GetNode<Timer>("DashCooldownTimer");
		_jumpTimer = GetNode<Timer>("JumpTimer");
		_inventory = GetNode<Inventory>("Inventory");

		// Setting inicial behaviors
		_state = (int)state.MOVING;
		PlayAnim("Idle");
		_inventory.Hide();
	}

	public override void _Process(double delta)
	{
		switch (_state){
			case state.JUMPING:
				Jumping(delta);
				break;
			case state.FALLING:
				Falling(delta);
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
		
		// If player is on floor, than he can jump
		if (IsOnFloor()){
			// Dealling with horizontal speed
			HorMove(delta);
			_canJump = true;
			_velocity.Y = 0;
			Velocity = _velocity;
		}
		else{
			_state = state.FALLING;
		}
		

		// if player press the jump button and can jump, the state will be jumping
		if (Input.IsActionJustPressed("ui_jump") && _canJump){
			_state = state.JUMPING;
			_canJump = false;
			_jumpTimer.Start();
		}

		// if player press inventory button, then the inventory opens or closes
		if (Input.IsActionJustPressed("ui_inventory")){
			_inventory.Visible = !_inventory.Visible;
		}

		// Fliping
		Flip();
	}

	// Function that matches the jumping state
	private void Jumping(double delta)
	{
		// In jump state, player still can move horizontally
		HorMove(delta);

		// Making player move vertically
		_velocity.Y = _jumpSpeed * (float)delta;
		Velocity = _velocity;
		
	}

	// Function that matches the falling state
	private void Falling(double delta)
	{
		// In falling state, player still can move horizontaly
		HorMove(delta);

		// Making player fall
		_currentVSpeed += _gravity;
		_velocity.Y = _currentVSpeed * (float)delta;
		Velocity = _velocity;

		// Back to state moving
		if (IsOnFloor()){
			_state = state.MOVING;
		}
	}

	// Function that matches the dashing state
	private void Dashing(double delta)
	{

	}

	// Function that matches the climbing state
	private void Climbing(double delta)
	{

	}

	// Callback Function of JumpTimer timeout signal
	private void OnJumpTimerTimeout()
	{
		// When the jump time is over, go to falling state
		_state = state.FALLING;
	}

	// Callback Function of AnimationPlayer animation finished signal
	private void OnAnimAnimationFinished(string animName)
	{

	}

	// Function that deals with the horizontal movement
	private void HorMove(double delta)
	{
		// Getting player input and setting direction
		_prevDir = _input;
		_input.X = Input.GetAxis("ui_left", "ui_right");

		// if the movement is just starting
		if (_prevDir.X == 0 && _input.X != 0){
			// then, start from inicial velocity
			_velocity.X = _input.X * _initialMoveSpeed * (float)delta;
			_currentHSpeed = _velocity.X;
		}
		else if (_prevDir.X != 0 && _input.X != 0){
			// if the player was moving and didnt reached the max velocity, then just aceleratte 
			if (_velocity.X < _maxMoveSpeed){
				_currentHSpeed += _acelleration;
				_velocity.X = _input.X * _currentHSpeed * (float)delta;
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
	{		
		Godot.Vector2 wallDetectorScale = _wallDetector.Scale;
		Godot.Vector2 itemsDetectorScale = _itemsDetector.Scale;

		if (_input.X != 0){
			if (_input.X >= 0){
				wallDetectorScale.X = 1;
				_wallDetector.Scale = wallDetectorScale;
				itemsDetectorScale.X = 1;
				_itemsDetector.Scale = itemsDetectorScale;

			}
			else{
				wallDetectorScale.X = -1;
				_wallDetector.Scale = wallDetectorScale;
				itemsDetectorScale.X = -1;
				_itemsDetector.Scale = itemsDetectorScale;
			}
			// Fliping sprite
			_sprite.FlipH = _input.X < 0;
		}
		return;
	}	
}
