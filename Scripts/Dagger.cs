using Godot;

public partial class Dagger : Area3D
{
	[Signal]
	public delegate void PlayerInteractedEventHandler();

	public override void _Ready()
	{
		
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
