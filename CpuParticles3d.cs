using Godot;
using System;

public partial class CpuParticles3d : CpuParticles3D
{
    private Glider glider;
    
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
            GlobalPosition = glider.Position;
        }
    }
}
