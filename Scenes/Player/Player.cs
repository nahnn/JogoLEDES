using Godot;
using System;
using System.Collections;

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
	private state _state;




	// Declaring status properties
	private float _health = 3.0f;
	private float _maxHealth = 10.0f;
	private float _shield = 3.0f;
	

	// Declaring move properties
	private Godot.Vector2 _input = Godot.Vector2.Zero;
	private Godot.Vector2 _prevDir;
	private Godot.Vector2 _velocity;

	// Move state (horMoves)
	private float _initialMoveSpeed = 500.0f;
	private float _maxMoveSpeed = 1000.0f;
	private float _currentHSpeed;
	private float _acelleration = 500.0f;

	// Dash state
	private float _dashSpeed = 90000.0f;
	private bool _canDash = true;

	// Jump/ falling state (vertical moves)
	private float _jumpSpeed = -50000.0f;
	private float _currentVSpeed = 0;
	private float _gravity = 5000.0f;
	private bool _canJump;
	private int _currentJumpCount = 1;
	private int _jumpCount;

	// Climb state
	private float _climbSpeed = 7500.0f;
	private bool _canClimb = false;
	private bool _canColect = false;
	

	// Nodes variables
	private Area2D wall; 
	private Area2D item; 

	// Childs variables
	private Sprite2D _sprite;
	private Area2D _wallDetector;
	private Area2D _itemsDetector;
	private AnimationPlayer _anim;
	private Timer _dashTimer;
	private Timer _jumpTimer;
	private Hud _hud;
	private LifeBar _lifeBar;
	private Inventory _inventory;
	
	// Getting custom signals variable
	private CustomSignals _customSignals;

	public override void _Ready()
	{
		// Getting childs
		_sprite = GetNode<Sprite2D>("Sprite");
		_wallDetector = GetNode<Area2D>("WallDetector");
		_itemsDetector = GetNode<Area2D>("ItemsDetector");
		_anim = GetNode<AnimationPlayer>("Anim");
		_dashTimer = GetNode<Timer>("DashTimer");
		_jumpTimer = GetNode<Timer>("JumpTimer");
		_hud = GetNode<Hud>("HUD");
		_lifeBar = GetNode<LifeBar>("HUD/LifeBar");
		_inventory = GetNode<Inventory>("HUD/Inventory");

		// Getting the custom signal reff
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");

		// Setting inicial behaviors
		_state = state.MOVING;
		_jumpCount = _currentJumpCount;
		PlayAnim("Idle");
		UpdateHealth();
		
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
				Dashing(delta);
				break;
			case state.CLIMBING:
				Climbing(delta);
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
			// Dealling with horizontal move
			if (_input.X != 0){
				PlayAnim("Walking");
			}
			else{
				PlayAnim("Idle");
			}
			_jumpCount = _currentJumpCount;
			_canDash = true;
			_velocity.Y = 0;
			_currentVSpeed = 0;
			HorMove(delta);
		}
		else{
			_state = state.FALLING;
		}
		
		// actions
		JumpInput();
		ClimbInput();
		DashInput();
		CollectItem();
		UseItem();
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

		DashInput();
		ClimbInput();
		JumpInput();
		Flip();	
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

		DashInput();
		ClimbInput();
		JumpInput();
		

		// Back to moving state
		if (IsOnFloor()){
			_state = state.MOVING;
		}
		Flip();
	}

	// Function that matches the dashing state
	private void Dashing(double delta)
	{
		// does the dash at the direction that player is moving
		_velocity = _input * (float)delta;
		Velocity = _velocity;
		
	}

	// Function that matches the climbing state
	private void Climbing(double delta)
	{
		// Dealing vertical move
		_canJump = true;
		VerClimbMove(delta);
		JumpInput();

		// if player reach the floor
		if (_input.Y > 0 && IsOnFloor()){
			_state = state.MOVING;
		}
	}

	// Callback function of wall detector shape entered signal
	private void OnWallDetectorAreaEntered(Area2D area)
	{	
		_canClimb = true;
		wall = area;		
		
	}

	// Callback function of wall detector shape exited signal
	private void OnWallDetectorAreaExited(Area2D area)
	{
		_canClimb = false;
		if (_state == state.CLIMBING){
			_state = state.JUMPING;
			_jumpTimer.Start();
		}
	}

	// Callback function of item detector shape entered signal
	private void OnItemsDetectorAreaEntered(Area2D area)
	{
		_canColect = true;
		item = area;
	}

	private void OnItemsDetectorAreaExited(Area2D area)
	{
		
	}

	// Callback Function of JumpTimer timeout signal
	private void OnJumpTimerTimeout()
	{
		// When the jump time is over, go to falling state with a inicial vSpeed
		PlayAnim("Falling");
		_state = state.FALLING;
		_currentVSpeed = 1200.0f;
	}

	// Callback Function of DashTimer timeout signal
	private void OnDashTimerTimeout()
	{
		// there are two different behaviors
		if (IsOnFloor()){
			_state = state.MOVING;
		}
		else{
			_state = state.FALLING;
		}
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
			if (Math.Abs(_velocity.X) < _maxMoveSpeed){
				_currentHSpeed += _acelleration;
				_velocity.X = _input.X * _currentHSpeed * (float)delta;
			}
			else{
				// Otherwise, set velocity to the max velocity
				_velocity.X = _input.X * _maxMoveSpeed * (float)delta;
			}
		}
		else{
			// Otherwise, player will stop
			_velocity.X = _input.X;
		}
		Velocity = _velocity;
		return;
	}

	// Function that deals with player's vertical movement when climbing
	private void VerClimbMove(double delta)
	{
		// Setting player's velocity component X to zero and getting input
		_velocity.X = 0;
		_input.Y = Input.GetAxis("ui_up", "ui_down");

		_velocity.Y = _input.Y * _climbSpeed * (float)delta;
		Velocity = _velocity;
	}

	// Function that switch to dash
	private void DashInput()
	{
		// if player press the dash button, then state will be dashing
		if (Input.IsActionJustPressed("ui_dash") && _canDash){
			_state = state.DASHING;
			_canDash = false;
			_input.X = Input.GetAxis("ui_left", "ui_right");
			_input.Y = Input.GetAxis("ui_up", "ui_down");
			_input = _input.Normalized() * _dashSpeed;
			_dashTimer.Start();
		}
	}

	// Function that switches to climb
	private void ClimbInput()
	{
		// if player press the climb button, then state will be climbing
		if (Input.IsActionJustPressed("ui_climb") && _canClimb){
			PlayAnim("Climbing");
			_state = state.CLIMBING;
			_velocity.X = 0;
		}
	}

	// Function that switches to Jump
	private void JumpInput()
	{
		// if the jumps are over
		if (_jumpCount != 0){
			_canJump = true;
		}
		else{
			_canJump = false;
		}
		// if player press the jump button and can jump, the state will be jumping
		if (Input.IsActionJustPressed("ui_jump") && _canJump){
			PlayAnim("Jumping");
			_state = state.JUMPING;
			_jumpCount -= 1;
			_jumpTimer.Start();
		}
	}

	// Function that collects a item
	private void CollectItem()
	{
		// if player get an item
		if (Input.IsActionJustPressed("ui_interact") && _canColect){
			// Emiting signal and deleting item
			_customSignals.EmitSignal(CustomSignals.SignalName.GotItem, item);
			item.QueueFree();
		}
	}

	// Function that interacts with a item on inventory
	private void UseItem()
	{
		// variable that holds current slot
		var slot = _inventory._slots[_inventory._currentPosition];
		if (Input.IsActionJustPressed("ui_action") && !slot.IsEmpty())
		{
			// Matching behavior with item
			switch(slot._name){
				case "Wings":
					IncreaseJumps();
					slot.DecreaseNumber();
					break;
				case "RedAmulet":
					IncreaseDefense();
					slot.DecreaseNumber();
					break;
				default:
					// default case will be the health potion case
					IncreaseHealth();
					slot.DecreaseNumber();
					break;
			}
		}
	}

	// Function that increases health
	private void IncreaseHealth()
	{
		// the unic form of increasing health will be health potions
		if (_health < _maxHealth){
			HealthPotion potion = new HealthPotion();
			_health += potion._cureAmount;
		}
		UpdateHealth();
	}

	// Function that decreases health
	public void DecreaseHealth(float amount)
	{
		// Decreasing player helth
		var damage = (amount - _shield);
		if (damage >= 0){
			_health -= damage; 
			UpdateHealth();
			if (_health <= 0){
				QueueFree();
			}
			
		}
	}

	// Function that increases the player shield
	private void IncreaseDefense()
	{
		RedAmulet amulet = new RedAmulet();
		_shield += amulet._increaseRatio;
	}

	// Function that increases number of jumps
	private void IncreaseJumps()
	{
		// increases the jump count when gets a wing
		Wings wing = new Wings();
		_currentJumpCount += wing._moreJumps;
	}

	// Function that updates health
	private void UpdateHealth()
	{
		Godot.Vector2 lifeBarSize = new Godot.Vector2(0, 32);
		if (_health >= _maxHealth){
			_health = _maxHealth;
		}
		lifeBarSize.X = 323 * (_health/_maxHealth);
		_lifeBar._life.Size = lifeBarSize;
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
