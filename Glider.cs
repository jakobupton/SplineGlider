using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Glider : MeshInstance3D
{
    private float Speed = 5.0f;
    private float RotationSpeed = 2.0f;
    private float WaypointThreshold = 0.2f;
    
    private List<Vector3> waypoints = new List<Vector3>();
    private int currentWaypointIndex = 0; 
    private bool isMoving = false;
    private Vector3 initialScale;
    private float distanceToNextWaypoint; // Distance from previous waypoint to next one
    private float turningAmount; // Accumulator for rolling so its smooth
    
    public override void _Ready()
    {
        initialScale = Scale;
        
        waypoints = new List<Vector3>(BezierCurve.GetBakedCurve());
        isMoving = true;
        Position = waypoints[0]; // start at first waypoint
    }

    // Returns a waypoint at the index, wrapping around for negative numbers and indices > count
    private Vector3 getWaypoint(int index)
    {
        while (index < 0) {
            index += waypoints.Count;
        }
        return waypoints[index % waypoints.Count];
    }
    
    public override void _Process(double delta)
    {
        if (!isMoving || waypoints.Count == 0)
            return;
            
        // Various important positions in the game world
        Vector3 targetPosition = waypoints[currentWaypointIndex];
        Vector3 nextTargetPosition = getWaypoint(currentWaypointIndex + 1);
        Vector3 nextNextTargetPosition = getWaypoint(currentWaypointIndex + 2);
        Vector3 currentPosition = Position;
        
        // Calculate direction
        Vector3 direction = targetPosition - currentPosition;
        float distance = direction.Length(); 
        float interpolationWeight = distance / distanceToNextWaypoint;
        
        // check if we're at waypoint
        if (distance < WaypointThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            distanceToNextWaypoint = Position.DistanceTo(waypoints[currentWaypointIndex]);
            return;
        }
        
        direction = direction.Normalized();
        
        // Move towards target
        Position += direction * Speed * (float)delta;

        // Look at where the glider is supposed to face
        var lookAtCurrentNode = Transform.LookingAt(targetPosition, Vector3.Up);
        var lookAtNextNode = Transform.LookingAt(nextTargetPosition, Vector3.Up);
        // TODO: Interpolation breaks down at the point where the two curves meet, need to look into the waypoints
        Transform = lookAtNextNode.InterpolateWith(lookAtCurrentNode, interpolationWeight); 
        
        // Rotations to make the object face the right way 
        RotateObjectLocal(Vector3.Up, Mathf.Pi);
        RotateObjectLocal(Vector3.Right, Mathf.Pi / 2);

        /*
        // Tilt with the turns
        var targetTurn = 0.0f;
        var previousAngle = targetPosition.AngleTo(nextTargetPosition);
        var currentAngle = nextTargetPosition.AngleTo(nextNextTargetPosition);
        if (previousAngle > currentAngle)
            targetTurn = 0.3f;
        else if (previousAngle < currentAngle)
            targetTurn = -0.3f;
        turningAmount = turningAmount + ((targetTurn - turningAmount) * 0.5f * (float)delta);
        RotateObjectLocal(Vector3.ModelTop, turningAmount); 
        */

        // get future waypoint
        Vector3 nextWaypoint = waypoints[(currentWaypointIndex + 1) % waypoints.Count];
        // get future direction
        Vector3 futureDirection = (nextWaypoint - targetPosition).Normalized();
        
        // cross product to get the perpendicular vector of direction-future direction plane
        // Length function gets the magnitude of the roll angle.
        float rollAngle = futureDirection.Cross(direction).Length();
        RotateObjectLocal(Vector3.ModelTop, rollAngle * 2); //multiplied by 2 to increase intensity
        
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