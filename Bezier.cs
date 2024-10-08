using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class BezierCurve
{
    public static List<Vector3> MakeCurve(Vector3[] points)
    {
        var spots = new List<Vector3>();

        // TODO: Put a curve into points list

        // This is just a demo path until this method works
        for (float theta = 0; theta < 2 * Math.PI; theta += (float)(Math.PI / 180))
        {
            spots.Add(new Vector3((int)Math.Round(50 + (Math.Cos(theta) * 25)), (int)Math.Round(50 + (Math.Sin(theta) * 25)), 50));
        }

        return spots;
    }
}