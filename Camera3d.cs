using Godot;
using System;

public partial class Camera3d : Camera3D
{
    float _yaw = 0;  // Horizontal (yaw) rotation of the camera
    float _pitch = 0;  // Vertical (pitch) rotation of the camera
    Vector3 _offset = new Vector3(0, 5, -15);  // Initial offset from the glider
    Node3D _glider;  // Reference to the glider

    public override void _Ready()
    {
        // Hide the mouse cursor
        Input.MouseMode = Input.MouseModeEnum.Hidden;

        // Get the glider node
        _glider = GetParent().GetNode<Node3D>("Glider");  // Update path to the actual glider node
    }

    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.Escape)) 
        {
            GetTree().Quit();
        }

        // Mouse input for rotating the camera
        var currentMousePosition = GetViewport().GetMousePosition();
        var relativeMousePosition = (GetViewport().GetVisibleRect().Size / 2) - currentMousePosition;
        Input.WarpMouse(GetViewport().GetVisibleRect().Size / 2);  // Keep mouse centered

        // Update yaw and pitch based on mouse movement
        _yaw += relativeMousePosition.X * 0.01f;
        _pitch += relativeMousePosition.Y * 0.01f;

        // Clamp pitch to prevent flipping
        _pitch = Math.Clamp(_pitch, -((float)Math.PI * 0.4f), (float)Math.PI * 0.4f);

        // Calculate the camera's new position based on yaw, pitch, and the offset
        Vector3 direction = new Vector3(
            (float)(Math.Cos(_yaw) * Math.Cos(_pitch)),
            (float)Math.Sin(_pitch),
            (float)(Math.Sin(_yaw) * Math.Cos(_pitch))
        );

        // Set camera position and look at the glider
        Position = _glider.Position + direction * _offset.Length();
        LookAt(_glider.Position);
    }
}
