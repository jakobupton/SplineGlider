using Godot;
using System;

public partial class Smoke : CpuParticles3D
{
    // Called when node enters scene tree
    public override void _Ready()
    {

        Emitting = true; // Particles are emitted when game starts
    }

}
