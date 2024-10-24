using Godot;
using System;
using System.Collections.Generic;

public partial class Glider : MeshInstance3D
{
    private float Speed = 5.0f;
    private float RotationSpeed = 2.0f;
    private float WaypointThreshold = 0.5f;
    
    private List<Vector3> waypoints = new List<Vector3>();
    private int currentWaypointIndex = 0; 
    private bool isMoving = false;
    private Vector3 initialScale;
    
    public override void _Ready()
    {
        initialScale = Scale;
        
        waypoints = new List<Vector3>(BezierCurve.GetBakedCurve());
        isMoving = true;
        Position = waypoints[0]; // start at first waypoint
    }
    
    public override void _Process(double delta)
    {
        if (!isMoving || waypoints.Count == 0)
            return;
            
        Vector3 targetPosition = waypoints[currentWaypointIndex];
        Vector3 currentPosition = Position;
        
        // Calculate direction
        Vector3 direction = targetPosition - currentPosition;
        float distance = direction.Length(); 
        
        // check if we're at waypoint
        if (distance < WaypointThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            return;
        }
        
        direction = direction.Normalized();
        
        // Move towards target
        Position += direction * Speed * (float)delta;

        // Rotating glider
        Transform = Transform.LookingAt(targetPosition, Vector3.Up);
        
        // Rotations to make the object face the right way 
        RotateObjectLocal(Vector3.Up, Mathf.Pi);
        RotateObjectLocal(Vector3.Right, Mathf.Pi / 2);
        
        Scale = initialScale * (1.0f - (distance / 100.0f));
    }
    
    public Vector3 GetGliderPosition()
    {
        return Position;
    }

    public Quaternion GetGliderRotation()
    {
        return Transform.Basis.GetRotationQuaternion();
    }
}