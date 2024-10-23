using Godot;
using System;

public partial class Weather : Sprite3D
{
    private Glider glider;

    public override void _Ready()
    {
        glider = GetParent().GetNode<Glider>("Glider");
    }

    public override void _Process(double delta)
    {
       if(glider != null)
        {
            GlobalPosition = glider.Position;
           
        }
    }
}
