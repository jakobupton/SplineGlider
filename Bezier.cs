using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public class BezierCurve
{
    // This will be set to a list of points once MakeCurve is called
    public static List<Vector3> bakedCurve = null;

    // Reflects a point about a given line (line is just two points)
    public static Vector3 ReflectPoint(Vector3 point, Vector3 linePoint1, Vector3 linePoint2)
    {
        // Find line components
        float m = (linePoint2.Y - linePoint1.Y) / (linePoint2.X - linePoint1.X);
        float b = linePoint2.Y - (linePoint2.X * m);
        float perpendicular = -1.0f / m;

        // Get point in line where the other point is perpendicular
        float xInt = (m * point.X - point.Y + b) / (m - perpendicular);
        xInt = (point.X + (m * (point.Y - b))) / ((m * m) + 1);
        float yInt = m * xInt + b;
        
        // Reflect point
        float xOut = (2 * xInt) - point.X;
        float yOut = (2 * yInt) - point.Y;

        return new Vector3(xOut, yOut, point.Z);
    }

    // From Godot documentation - https://docs.godotengine.org/en/stable/tutorials/math/beziers_and_curves.html
    public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 q0 = p0.Lerp(p1, t);
        Vector3 q1 = p1.Lerp(p2, t);
        Vector3 q2 = p2.Lerp(p3, t);
        
        Vector3 r0 = q0.Lerp(q1, t);
        Vector3 r1 = q1.Lerp(q2, t);

        Vector3 s = r0.Lerp(r1, t);
        return s;
    }

    // Returns a list with an example curve, cacheing the results so this method is quick
    public static List<Vector3> GetBakedCurve() 
    {
        if (bakedCurve == null)
        {
            Vector3 p0 = new Vector3(25, 25, 0);
            Vector3 p1 = new Vector3(38, 7, 0);
            Vector3 p2 = new Vector3(95, 50, 0);
            Vector3 p3 = new Vector3(75, 75, 0);
            bakedCurve = MakeCurve(p0, p1, p2, p3);
        }
        return bakedCurve;
    }

    // Whatever you make with this becomes this class's baked curve
    public static List<Vector3> MakeCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) 
    {
        var spots = new List<Vector3>();
        float steps = 100;
        
        // First bezier
        for (float i = 0; i <= 1; i += 1 / steps)
        {
            spots.Add(Bezier(p0, p1, p2, p3, i));
        }

        Vector3 p12 = ReflectPoint(p1, p0, p3);
        Vector3 p22 = ReflectPoint(p2, p0, p3);

        // Second bezier is just a reflection so it smoothly connects
        for (float i = 0; i <= 1; i += 1 / steps)
        {
            spots.Add(Bezier(p3, p22, p12, p0, i));
        }

        return spots;
    }
}