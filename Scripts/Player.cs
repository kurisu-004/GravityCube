using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	
	[Signal]
	public delegate void PlayerInteractedEventHandler();

	private Camera3D camera;

	[Export]
	public RotationManager rotationManager;
	private Vector3 globalForwardVec = new Vector3(0, 0, -1);

	// private Vector3 gravityVec = new Vector3(0, -1, 0).Normalized();
	// private Vector3 forwardVec = new Vector3(0, 0, -1);

	private float mouseSensitivity_v = 0.1f;
	private float mouseSensitivity_h = 0.001f;
	// private float verticalRotation = 0.0f;
	private float horizontalRotation = 0.0f;
	private bool isNearObject = false;
	private Area3D Dagger;
	private Window victoryWindow;

	public override void _Ready()
	{
		camera = GetNode<Camera3D>("Camera3D");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		// Get the Area3D node for the interactive object
		Dagger = GetNode<Area3D>("/root/Room/Dagger");
		
		// Get the victory window 
		victoryWindow = GetNode<Window>("/root/Room/Dagger/Victory");

		// Connect signals to detect when the player interacts with the object
		Dagger.Connect("body_entered",new Callable(this, nameof(OnBodyEntered)));
		Dagger.Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
	}
	
	public override void _Process(double delta)
	{
		// Check if the player is near the object and presses the interact key
		if (isNearObject && Input.IsActionJustPressed("interact"))
		{
			_on_Interact();
		}
	}
	
	// When the player enters the interactive area
	private void OnBodyEntered(Node body)
   {
	if (body == this)  // 检查进入区域的是玩家
	{
		isNearObject = true;
	}
	}

	private void OnBodyExited(Node body)
   {
	if (body == this)
	{
		isNearObject = false;
	}
	}


	// Handle interaction
	private void _on_Interact()
	{
		// Show the victory window
		victoryWindow.Show();
	}
	

	public override void _PhysicsProcess(double delta)
	{

		// Load global directions from RotationManager
		Vector3 gravityVec = rotationManager.gravityVec.Normalized();
		this.globalForwardVec = this.rotationManager.forwardVec.Normalized();
		this.UpDirection = -gravityVec; // Parameter in class RigidBody: UpDirection. Used for IsOnFloor().
		
		// Compute and set where player is looking
		Vector3 forwardVec = this.globalForwardVec.Rotated(gravityVec.Normalized(), horizontalRotation);
		this.Transform = this.Transform.LookingAt(GlobalPosition + forwardVec, this.UpDirection);

		float velocityUp = 0;
		float velocityRight = 0;
		float velocityForward = 0;

		// only consider velocity in the gravities direction
		float angle_vel_grav = Mathf.Acos(Math.Clamp(gravityVec.Normalized().Dot(this.Velocity.Normalized()), -1, 1));
		float vel_projected_on_gravity = Velocity.Length() * Mathf.Cos(angle_vel_grav);
		velocityUp = -vel_projected_on_gravity;
		// GD.Print(gravityVec.Normalized(), this.Velocity.Normalized(), " Dot: " + gravityVec.Normalized().Dot(this.Velocity.Normalized()), " ",  angle_vel_grav, " ", vel_projected_on_gravity, " ", velocityUp);
		
		if (!IsOnFloor())
		{
			// Apply gravity
			velocityUp -= gravityVec.Length() * (float)delta * rotationManager.globalGravityStrength;
		}
		else { // Is on Floor
			velocityUp = 0;

			if (Input.IsActionJustPressed("ui_accept")) {
				velocityUp = JumpVelocity;
			}
		}

		// Get the input direction and handle the movement/deceleration.
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector3 direction = new Vector3(inputDir.X, 0, -inputDir.Y).Normalized();
		if (direction != Vector3.Zero) {
			velocityRight = direction.X * Speed;
			velocityForward = direction.Z * Speed;
		} else {
			velocityRight = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocityForward = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		// Set computed velocity
		Vector3 rightVec = camera.GlobalTransform.Basis * new Vector3(1, 0, 0);
		// GD.Print("Right: " + rightVec, "VelocityRight: " + velocityRight, "Up: " + velocityUp, "Right: " + velocityRight, "Forward: " + velocityForward);
		Velocity = -velocityUp*gravityVec + forwardVec*velocityForward + rightVec*velocityRight;

		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseEvent)
		{
			this.horizontalRotation += mouseEvent.Relative.X * mouseSensitivity_h;
			// RotateY(-mouseEvent.Relative.X * mouseSensitivity_h);
			// verticalRotation -= mouseEvent.Relative.Y * mouseSensitivity_v;
			// verticalRotation = Mathf.Clamp(verticalRotation, -90.0f, 90.0f);
			// camera.RotationDegrees = new Vector3(verticalRotation, 0, 0);
		}
		if (@event.IsActionPressed("interact"))
		{
			EmitSignal(nameof(PlayerInteracted)); 
		}

		if (@event is InputEventKey eventKey)
		{
		}

	}

}
