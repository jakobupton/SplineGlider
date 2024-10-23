using Godot;
using System;

public partial class Weather : Sprite3D
{
// Create a reference to the Glider object
    private Glider glider;

    public override void _Ready()
    {
    // Set the glider variable to the glider node through the parent node
        glider = GetParent().GetNode<Glider>("Glider");
    }

    public override void _Process(double delta)
    {
    //Check if glider is a valid reference point / not null
       if(glider != null)
        {
        //Set position of sprite to position of the glider node
            GlobalPosition = glider.Position;
           
        }
    }
}
