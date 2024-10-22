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
        
        // Create target rotation
        Vector3 targetForward = -direction;
        Vector3 targetRight = Vector3.Up.Cross(targetForward).Normalized();
        Vector3 targetUp = targetForward.Cross(targetRight).Normalized();
        
        // Create target transform
        Transform3D targetTransform = new Transform3D(
            targetRight * initialScale.X,
            targetUp * initialScale.Y,
            targetForward * initialScale.Z,
            currentPosition
        );
        
        // get our current rotation and our target rotation as quaternions
        Quaternion currentRotation = Transform.Basis.GetRotationQuaternion();
        Quaternion targetRotation = targetTransform.Basis.GetRotationQuaternion();
        
        // normalize to avoid weirdness
        currentRotation = currentRotation.Normalized();
        targetRotation = targetRotation.Normalized();
    

        float rotationWeight = Mathf.Clamp((float)delta * RotationSpeed, 0f, 1f);
        Quaternion newRotation = currentRotation.Slerp(targetRotation, rotationWeight);
        
        // idk if we even need this scale stuff, but it was doing weird stuff
        Transform = new Transform3D(
            new Basis(newRotation).Scaled(initialScale),
            currentPosition
        );
        
        Position += direction * Speed * (float)delta;
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