using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public class BezierCurve
{
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
}