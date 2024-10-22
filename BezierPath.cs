using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

public partial class BezierPath : Path2D
{
    // Curve2D to store sampled points
    private Curve2D curve;
    public MeshInstance3D cube;

    //using cube to show current path, remove later
    [Export] public PackedScene spawn_object = (PackedScene)GD.Load("res://cube.tscn");
    

    public override void _Ready()
    {
        curve = new Curve2D();

        Vector3 p0 = new Vector3(0, 0, 0);
        Vector3 p1 = new Vector3(25, 100, 0);
        Vector3 p2 = new Vector3(75, 100, 0);
        Vector3 p3 = new Vector3(100, 0, 0);

        // Number of sample points
        int sampleCount = 20;

        // Sample points from the Bezier curve
        List<Vector3> curvePoints = BezierCurve.SampleCurve(p0, p1, p2, p3, sampleCount);

        // converting Vector3 points to Vector2 and adding them to the Curve2D
        foreach (Vector3 point in curvePoints)
        {
            curve.AddPoint(new Vector2(point.X, point.Y));  // Using x and y, ignoring z
        }
        
        // !!!FIXME Displaying cubes to view path, replace with image later
        foreach (Vector3 point in curvePoints)
        {
             var obj = (Node3D)spawn_object.Instantiate();
             obj.Translate(new Vector3(point.X,10,point.Y));
             AddChild(obj);
        }
        // Assign the Curve2D to Path2D
        Curve = curve;
    }
}
