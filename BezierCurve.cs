using Godot;
using System;
using System.Collections.Generic;

public partial class BezierCurve : Node2D
{
    // Function to calculate a point on the cubic Bézier curve
    public static Vector3 MakeCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 q0 = p0.Lerp(p1, t);
        Vector3 q1 = p1.Lerp(p2, t);
        Vector3 q2 = p2.Lerp(p3, t);
        
        Vector3 r0 = q0.Lerp(q1, t);
        Vector3 r1 = q1.Lerp(q2, t);

        Vector3 s = r0.Lerp(r1, t);
        return s;
    }

    // Sample multiple points along the curve
    public static List<Vector3> SampleCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int sampleCount)
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i <= sampleCount; i++)
        {
            float t = (float)i / sampleCount; // Normalized time between 0 and 1
            points.Add(MakeCurve(p0, p1, p2, p3, t));
        }
        return points;
    }

    
    // trial use of curve
    /*
    public override void _Ready()
    {
        Vector3 p0 = new Vector3(0, 0, 1000);
        Vector3 p1 = new Vector3(25, 100, 1000);
        Vector3 p2 = new Vector3(75, 100, 1000);
        Vector3 p3 = new Vector3(100, 0, 1000);

        int sampleCount = 20; // Number of sample points
        List<Vector3> curvePoints = SampleCurve(p0, p1, p2, p3, sampleCount);
        
        //debugging curve points
        foreach (Vector3 point in curvePoints)
        {
            GD.Print(point); // For debugging purposes
        }
    }
       */
}
