using Godot;
using System;

public partial class weatherposition: GpuParticles3D
{
    private Glider glider;
    private Vector3 offset = new Vector3(5, 10, 0); // Adjust this offset as needed

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        glider = GetParent().GetNode<Glider>("Glider");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (glider != null)
        {
            GlobalPosition = glider.Position + offset;
        }
    }
}