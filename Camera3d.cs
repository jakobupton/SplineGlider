using Godot;
using System;
using System.Security.AccessControl;

public partial class Camera3d : Camera3D
{
	float _direction = 0;
	float _zdirection = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Hidden;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var currentMousePosition = GetViewport().GetMousePosition();
		var relativeMousePosition = (GetViewport().GetVisibleRect().Size / 2) - currentMousePosition;
		Input.WarpMouse(GetViewport().GetVisibleRect().Size / 2);

		_direction += relativeMousePosition.X * 0.01f;
		_zdirection += relativeMousePosition.Y * 0.01f;
		_zdirection = Math.Clamp(_zdirection, -((float)Math.PI * 0.4f), (float)Math.PI * 0.4f);
		
		LookAt(new Vector3(
			Position.X - (float)Math.Cos(_direction), 
			Position.Y + (float)Math.Tan(_zdirection),
			Position.Z + (float)Math.Sin(_direction)
		));

		float forward = Input.IsKeyPressed(Key.W) ? 1 : 0;
		float backward = Input.IsKeyPressed(Key.S) ? -1 : 0;
		float direction = forward + backward;
	}
}
