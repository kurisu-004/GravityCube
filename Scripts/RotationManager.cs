using Godot;
using System;

public partial class RotationManager : Node3D
{

	public Vector3 gravityVec = new Vector3(0, -1, 0);
	public Vector3 forwardVec = new Vector3(0, 0, -1);

	[Export]
	public float globalGravityStrength = 10.0f;
	[Export]
	public int rotationPeriodMS = 1000;


	private bool isRotating = false;
	private Vector3 currentRotationAxis;
	private Vector3 currentRotationGravityOriginal;
	private Vector3 currentRotationForwardOriginal;
	private int currentRotationDirection;
	private ulong currentRotationTimeStart;
	private ulong currentRotationTimeEnd;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isRotating) {
			ulong timeNow = Time.GetTicksMsec();
			float progress = (timeNow-currentRotationTimeStart)/(float)(currentRotationTimeEnd-currentRotationTimeStart);
			
			if (progress >= 1.0f) {
				progress = 1.0f;
				isRotating = false;
			}

			float angle = (float) (currentRotationDirection * progress * (Math.PI/2));
			this.gravityVec = this.currentRotationGravityOriginal.Rotated(currentRotationAxis, angle);
			this.forwardVec = this.currentRotationForwardOriginal.Rotated(currentRotationAxis, angle).Normalized();
		}
	}

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

		// float rotationSpeed = 0.2f;
		if (!isRotating) {
			if (@event.IsActionPressed("rotate_left")) {
				Vector3 rotationAxis = (GlobalTransform.Basis * new Vector3(0,0,1)).Normalized();
				this.startRotation(rotationAxis, this.gravityVec, this.forwardVec, -1, rotationPeriodMS);
				// float rotationAngle = (float) (-1 * rotationSpeed * Math.PI/2);

				// this.gravityVec = this.gravityVec.Rotated(rotationAxis, rotationAngle);
				// this.forwardVec = this.forwardVec.Rotated(rotationAxis, rotationAngle);
			}
			if (@event.IsActionPressed("rotate_right")) {
				Vector3 rotationAxis = (GlobalTransform.Basis * new Vector3(0,0,1)).Normalized();
				this.startRotation(rotationAxis, this.gravityVec, this.forwardVec, 1, rotationPeriodMS);
				// float rotationAngle = (float) (rotationSpeed * Math.PI/2);

				// this.gravityVec = this.gravityVec.Rotated(rotationAxis, rotationAngle);
				// this.forwardVec = this.forwardVec.Rotated(rotationAxis, rotationAngle);
			}
			if (@event.IsActionPressed("rotate_forward")) {
				Vector3 rotationAxis = (GlobalTransform.Basis * new Vector3(-1,0,0)).Normalized();
				this.startRotation(rotationAxis, this.gravityVec, this.forwardVec, 1, rotationPeriodMS);
				// float rotationAngle = (float) (-1 * rotationSpeed * Math.PI/2);

				// this.gravityVec = this.gravityVec.Rotated(rotationAxis, rotationAngle);
				// this.forwardVec = this.forwardVec.Rotated(rotationAxis, rotationAngle);
			}
			if (@event.IsActionPressed("rotate_backwards")) {
				Vector3 rotationAxis = (GlobalTransform.Basis * new Vector3(-1,0,0)).Normalized();
				this.startRotation(rotationAxis, this.gravityVec, this.forwardVec, -1, rotationPeriodMS);
				// float rotationAngle = (float) (rotationSpeed * Math.PI/2);

				// this.gravityVec = this.gravityVec.Rotated(rotationAxis, rotationAngle);
				// this.forwardVec = this.forwardVec.Rotated(rotationAxis, rotationAngle);
			}
		}
    }

	public void RotateRoom(Vector3 rotationAxixBase)
	{
		if (!isRotating)
		{
			Vector3 rotationAxis = (GlobalTransform.Basis * rotationAxixBase).Normalized();
			this.startRotation(rotationAxis, this.gravityVec, this.forwardVec, 1, rotationPeriodMS);
		}
	}

	private void startRotation(Vector3 axis, Vector3 gravityOriginal, Vector3 forwardOriginal, int direction, int durationMsec) {
		this.isRotating = true;
		this.currentRotationAxis = axis;
		this.currentRotationGravityOriginal = gravityOriginal;
		this.currentRotationForwardOriginal = forwardOriginal;
		this.currentRotationDirection = direction;
		this.currentRotationTimeStart = Time.GetTicksMsec();
		this.currentRotationTimeEnd = Time.GetTicksMsec() + (ulong) durationMsec;
	}
}
