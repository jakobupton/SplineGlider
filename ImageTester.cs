using Godot;
using System;
using System.Collections.Generic;
public partial class ImageTester : Node2D
{
	ImageTexture _tex;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var img = Image.CreateEmpty(100, 100, false, Image.Format.Rgba8);

        Vector3 p0 = new Vector3(30, 10, 0);
        Vector3 p1 = new Vector3(10, 40, 0);
        Vector3 p2 = new Vector3(80, 60, 0);
        Vector3 p3 = new Vector3(50, 80, 0);

        List<Vector3> points = new List<Vector3>();

        for (float t = 0f; t <= 1; t += 0.01f)
        {
            Vector3 point = BezierCurve.MakeCurve(p0, p1, p2, p3, t);
            points.Add(point);
        }

        // Fill with black
        for (int y = 0; y < 100; y++)
		{
			for (int x = 0; x < 100; x++)
			{
				img.SetPixel(x, y, Color.Color8(0, 0, 0));
			}
		}

		// Put the grid contents to the image
		foreach (var point in points)
		{
			img.SetPixel((int)point.X, (int)point.Y, Color.Color8(255, 255, 255));
		}

		// The output texture
		_tex = ImageTexture.CreateFromImage(img);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

    public override void _Draw()
    {
        DrawTextureRect(_tex, new Rect2(0, 0, new Vector2(200, 200)), false);
    }
}
