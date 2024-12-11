using Godot;
using System;

public partial class Room : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// if escape key is pressed, go back to main menu
		if (Input.IsActionJustPressed("back_to_menu"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
			_on_pressed_esc();
		}
	}

	private void _on_pressed_esc()
	{
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
	}
}
