using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	private Camera3D camera;

	private float mouseSensitivity_v = 0.1f;
	private float mouseSensitivity_h = 0.001f;
	private float verticalRotation = 0.0f;

	private Vector3 gravityVec = new Vector3(0, -1, 0).Normalized();
	private float gravityStrength = 10;
	private Vector3 forwardVec = new Vector3(0, 0, -1);

	public override void _Ready()
	{
		camera = GetNode<Camera3D>("Camera3D");
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _PhysicsProcess(double delta)
	{
		// Vector3 velocityNew = new Vector3(0,0,0);
		float velocityUp = 0;
		float velocityRight = 0;
		float velocityForward = 0;

		// update gravity (gravityVec vector)

		// only consider velocity in the gravities direction
		float angle_vel_grav = Mathf.Acos(Math.Clamp(gravityVec.Normalized().Dot(this.Velocity.Normalized()), -1, 1));
		float vel_projected_on_gravity = Velocity.Length() * Mathf.Cos(angle_vel_grav);
		velocityUp = -vel_projected_on_gravity;
		GD.Print(gravityVec.Normalized(), this.Velocity.Normalized(), " Dot: " + gravityVec.Normalized().Dot(this.Velocity.Normalized()), " ",  angle_vel_grav, " ", vel_projected_on_gravity, " ", velocityUp);

		// Add the gravity.
		if (!IsOnFloor())
		{
			// velocity += GetGravity() * (float)delta;
			velocityUp -= gravityVec.Length() * (float)delta * gravityStrength;
		}
		else { // Is on Floor
			velocityUp = 0;

			if (Input.IsActionJustPressed("ui_accept")) {
				velocityUp = JumpVelocity;
				GD.Print("Jump registered");
			}
		}

		// // Handle Jump.
		// if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		// {
		// 	// velocity.Y = JumpVelocity;
		// 	velocityUp = JumpVelocity;
		// 	GD.Print("Jump registered");
		// }

		GD.Print(IsOnFloor());


		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		// Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		// Vector3 direction = (camera.GlobalTransform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		Vector3 direction = new Vector3(inputDir.X, 0, -inputDir.Y).Normalized();
		if (direction != Vector3.Zero)
		{
			// velocity.X = direction.X * Speed;
			velocityRight = direction.X * Speed;
			// velocity.Z = direction.Z * Speed;
			velocityForward = direction.Z * Speed;
		}
		else
		{
			// velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocityRight = Mathf.MoveToward(Velocity.X, 0, Speed);
			// velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
			velocityForward = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		// Vector3 rightVec = -1 * gravityVec.Normalized().Cross(forwardVec.Normalized());
		Vector3 rightVec = camera.GlobalTransform.Basis * new Vector3(1, 0, 0);
		GD.Print("Right: " + rightVec, "VelocityRight: " + velocityRight, "Up: " + velocityUp, "Right: " + velocityRight, "Forward: " + velocityForward);
		Velocity = (velocityUp * -1 * gravityVec) + forwardVec * velocityForward + (rightVec)*velocityRight;

		// Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseEvent)
		{
			RotateY(-mouseEvent.Relative.X * mouseSensitivity_h);
			verticalRotation -= mouseEvent.Relative.Y * mouseSensitivity_v;
			verticalRotation = Mathf.Clamp(verticalRotation, -90.0f, 90.0f);
			// camera.RotationDegrees = new Vector3(verticalRotation, 0, 0);
		}

		if (@event is InputEventKey eventKey)
		{
			// if (eventKey.IsPressed() && eventKey.Keycode == Key.Escape) {
			// 	// this.gravityVec = new Vector3(-1, 0, 0);
			// 	// this.UpDirection = new Vector3(1, 0, 0);
			// 	// this.forwardVec = new Vector3(0, 0, 1);
			// 	// this.Transform = this.Transform.LookingAt(GlobalPosition + forwardVec, this.UpDirection);

			// 	// rotate the room to the right
			// 	Vector3 rotationAxis = new Vector3(0,0,1);
			// 	float rotationAngle = (float)Math.PI/2/10;

			// 	this.gravityVec = this.gravityVec.Rotated(rotationAxis, rotationAngle);
			// 	this.UpDirection = -this.gravityVec;
			// 	this.forwardVec = this.forwardVec.Rotated(rotationAxis, rotationAngle);
			// 	// Rotate Player
			// 	this.Transform = this.Transform.LookingAt(GlobalPosition + forwardVec, this.UpDirection);
			// }
		}

		float rotationSpeed = 0.2f;
		if (@event.IsActionPressed("rotate_left")) {
			Vector3 rotationAxis = GlobalTransform.Basis * new Vector3(0,0,1);
			float rotationAngle = (float) (-1 * rotationSpeed * Math.PI/2);

			this.gravityVec = this.gravityVec.Rotated(rotationAxis, rotationAngle);
			this.UpDirection = -this.gravityVec;
			this.forwardVec = this.forwardVec.Rotated(rotationAxis, rotationAngle);
			// Rotate Player
			this.Transform = this.Transform.LookingAt(GlobalPosition + forwardVec, this.UpDirection);
		}
		if (@event.IsActionPressed("rotate_right")) {
			Vector3 rotationAxis = GlobalTransform.Basis * new Vector3(0,0,1);
			float rotationAngle = (float) (rotationSpeed * Math.PI/2);

			this.gravityVec = this.gravityVec.Rotated(rotationAxis, rotationAngle);
			this.UpDirection = -this.gravityVec;
			this.forwardVec = this.forwardVec.Rotated(rotationAxis, rotationAngle);
			// Rotate Player
			this.Transform = this.Transform.LookingAt(GlobalPosition + forwardVec, this.UpDirection);
		}
		if (@event.IsActionPressed("rotate_forward")) {
			Vector3 rotationAxis = GlobalTransform.Basis * new Vector3(1,0,0);
			float rotationAngle = (float) (-1 * rotationSpeed * Math.PI/2);

			this.gravityVec = this.gravityVec.Rotated(rotationAxis, rotationAngle);
			this.UpDirection = -this.gravityVec;
			this.forwardVec = this.forwardVec.Rotated(rotationAxis, rotationAngle);
			// Rotate Player
			this.Transform = this.Transform.LookingAt(GlobalPosition + forwardVec, this.UpDirection);
		}
		if (@event.IsActionPressed("rotate_backwards")) {
			Vector3 rotationAxis = GlobalTransform.Basis * new Vector3(1,0,0);
			float rotationAngle = (float) (rotationSpeed * Math.PI/2);

			this.gravityVec = this.gravityVec.Rotated(rotationAxis, rotationAngle);
			this.UpDirection = -this.gravityVec;
			this.forwardVec = this.forwardVec.Rotated(rotationAxis, rotationAngle);
			// Rotate Player
			this.Transform = this.Transform.LookingAt(GlobalPosition + forwardVec, this.UpDirection);
		}


	}

}