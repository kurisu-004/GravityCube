using Godot;
using System;

public partial class DifficultyMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnEasyPressed()
	{
		StartGame("Easy");
	}

	private void OnMediumPressed()
	{
		StartGame("Medium");
	}

	private void OnHardPressed()
	{
		StartGame("Hard");
	}

	private void StartGame(string difficulty)
	{
		if (difficulty == "Hard") {
			GetTree().ChangeSceneToFile("res://Scenes/LevelStaircase.tscn");
		} else {		
			GetTree().ChangeSceneToFile("res://Scenes/Room.tscn");
		}
	}
}
