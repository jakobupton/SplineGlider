using Godot;
using System;

public partial class PathMovement : PathFollow2D
{
    [Export] private float speed = 0.1f; //speed of glider
    private Node3D glider2;

    public override void _Ready()
    {
        // Get the 3D model (glider2) from the scene tree
        glider2 = GetNode<Node3D>("glider2");
    }

    public override void _Process(double delta)
    {   
        // Do not know if this is the best way to do this, fix maybe?
        // Object moves along path by adding speed multiplied by the frametime
        ProgressRatio += (float)(speed * delta);

        // Loop back if ProgressRatio exceeds 1 (100% of the path)
        if (ProgressRatio > 1)
        {
            ProgressRatio = 0;
        }

        // Update glider position to follow the path
        Vector2 pathPosition = Position; // current position of PathFollow2D
        glider2.GlobalPosition = new Vector3(pathPosition.X,10, pathPosition.Y); 
    }
}
