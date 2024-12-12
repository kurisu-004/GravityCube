using Godot;

public partial class RotationButton : Area3D
{
    [Export] public Vector3 RotationAxisBase = new(0, 1, 0);
    [Export] public string TriggerKey = "r";
    
    private bool _isPlayerNearby = false;
    [Export] public RotationManager rotationManager;

	public override void _Ready()
    {
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
    }

	public override void _Input(InputEvent @event)
    {
		if (!_isPlayerNearby) return;
        
		if (@event.IsActionPressed("interact"))
        {
            rotationManager?.RotateRoom(RotationAxisBase);
        }
    }

    private void OnBodyEntered(Node body)
	{
		if (body.IsInGroup("Player"))
		{
			_isPlayerNearby = true;
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body.IsInGroup("Player"))
		{
			_isPlayerNearby = false;
		}
	}
}
