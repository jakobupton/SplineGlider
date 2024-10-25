using Godot;
using System;

public partial class WeatherController : Node3D
{
    GpuParticles3D snowParticles;
	GpuParticles3D rainParticles;

    public override void _Ready()
    {
        snowParticles = GetNode<GpuParticles3D>("snow");
        rainParticles = GetNode<GpuParticles3D>("rain");
    }

    public override void _Process(double delta)
    {
        // Check "1" key to toggle snow
        if (Input.IsActionJustPressed("toggle_snow"))
        {
            snowParticles.Emitting = !snowParticles.Emitting; // Toggle snow on/off
        }

        // Check "2" key to toggle rain
        if (Input.IsActionJustPressed("toggle_rain"))
        {
            rainParticles.Emitting = !rainParticles.Emitting; // Toggle rain on/off
        }
    }
}
