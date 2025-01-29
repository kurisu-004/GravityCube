using Godot;
using System;

public partial class Portal : Area3D
{
	// 传送门的目标位置(10, 0, 0)
	[Export] public Vector3 TargetPosition = new Vector3(10, 0, 0);

	public override void _Ready()
	{
		// 连接碰撞检测信号
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player player)  // 如果进入传送门的是玩家
		{
			// 设置玩家的新位置
			player.GlobalTransform = new Transform3D(Basis.Identity, TargetPosition);
			// this.GetChild<AudioStreamPlayer3D>(0).Play();

			// 可选：添加传送的视觉或音效效果
			// GD.Print($"Player teleported to {TargetPosition}");
		}
	}
}
