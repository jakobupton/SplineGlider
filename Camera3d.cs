using Godot;
using System;
using System.Security.AccessControl;

public partial class Camera3d : Camera3D
{
	float _direction = 0;  // Tracks the horizontal (yaw) rotation of the camera
	float _ydirection = 0;  // Tracks the vertical (pitch) rotation of the camera

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Hides the mouse cursor to create an FPS-like camera control experience
		Input.MouseMode = Input.MouseModeEnum.Hidden;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Handling movement input (WASD for forward/backward/strafe, Space for up, Shift for down)
		float forward = Input.IsKeyPressed(Key.W) ? -1 : 0;
		float backward = Input.IsKeyPressed(Key.S) ? 1 : 0;
		float left = Input.IsKeyPressed(Key.A) ? -1 : 0;
		float right = Input.IsKeyPressed(Key.D) ? 1 : 0;
		float up = Input.IsKeyPressed(Key.Space) ? 1 : 0;
		float down = Input.IsKeyPressed(Key.Shift) ? -1 : 0;

		float direction = forward + backward;
		float strafe = left + right;
		float vertical = up + down;

		// Creates a movement vector and normalizes it for smooth movement
		var move = new Vector3(strafe, vertical, direction);
		Translate(move.Normalized() * 0.1f);  // Applies the movement to the camera

		// Handles mouse movement to calculate the new camera direction
		var currentMousePosition = GetViewport().GetMousePosition();
		var relativeMousePosition = (GetViewport().GetVisibleRect().Size / 2) - currentMousePosition;
		Input.WarpMouse(GetViewport().GetVisibleRect().Size / 2);  // Keeps mouse centered in viewport

		// Updates camera rotation angles based on mouse movement
		_direction += relativeMousePosition.X * 0.01f;
		_ydirection += relativeMousePosition.Y * 0.01f;
		_ydirection = Math.Clamp(_ydirection, -((float)Math.PI * 0.4f), (float)Math.PI * 0.4f);  // Clamp pitch

		// Adjusts the camera's LookAt target based on the new rotation angles
		LookAt(new Vector3(
			Position.X - (float)Math.Cos(_direction), 
			Position.Y + (float)Math.Tan(_ydirection),
			Position.Z + (float)Math.Sin(_direction)
		));
	}
}

