using Godot;
using System.Collections.Generic;

public partial class RedLine : Sprite3D
{
    ImageTexture _tex;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var img = Image.CreateEmpty(200, 200, false, Image.Format.Rgba8);

        List<Vector3> points = BezierCurve.GetBakedCurve();


        // Put the grid contents to the image
        foreach (var point in points)
        {
            img.SetPixel((int)point.X, (int)point.Z, Color.Color8(255, 0, 0));
        }

        // The output texture
        _tex = ImageTexture.CreateFromImage(img);

        Texture = _tex;
        Scale = new Vector3(-100, 100, 1); // Adjust the scale as needed

    }
}