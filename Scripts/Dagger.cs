using Godot;

public partial class Dagger : Area3D
{
	[Signal]
	public delegate void PlayerInteractedEventHandler();
	private bool _isPlayerNearby = false;
	private Window victorywindow;
	

	public override void _Ready()
	{
		victorywindow=GetNode<Window>("/root/Room/Dagger/Victory");
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
	}
	
	public override void _Input(InputEvent @event)
	{
		if (!_isPlayerNearby) return;

		if (@event.IsActionPressed("interact"))
		{
			victorywindow?.Show();
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

	public override void _Process(double delta)
	{
		
	}

	private void _on_Player_Interaction()
	{
		// when interacted
		EmitSignal("PlayerInteracted");
	}
}
