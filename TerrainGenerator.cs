using Godot;

public partial class TerrainGenerator : Node3D
{
	MeshInstance3D ground;

	public void AddQuad(SurfaceTool st, Vector2 pos, Vector2 size, Vector4 elevations) 
	{
		st.AddVertex(new Vector3(pos.X,          elevations.X, pos.Y));          // Vertex 0
        st.AddVertex(new Vector3(pos.X + size.X, elevations.Y, pos.Y));          // Vertex 1
        st.AddVertex(new Vector3(pos.X + size.X, elevations.Z, pos.Y + size.Y)); // Vertex 2

		st.AddVertex(new Vector3(pos.X,          elevations.W, pos.Y));          // Vertex 0
        st.AddVertex(new Vector3(pos.X + size.X, elevations.Z, pos.Y + size.Y)); // Vertex 2
        st.AddVertex(new Vector3(pos.X,          elevations.W, pos.Y + size.Y)); // Vertex 3
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        MeshInstance3D land = new MeshInstance3D();
        SurfaceTool st = new SurfaceTool();
        st.Begin(Mesh.PrimitiveType.Triangles);

        AddQuad(st, new Vector2(0, 0), new Vector2(1, 1), new Vector4(0, 0, 0, 1));
		AddQuad(st, new Vector2(1, 0), new Vector2(1, 1), new Vector4(0, 0, 0, 0));
		AddQuad(st, new Vector2(2, 0), new Vector2(1, 1), new Vector4(0, 0, 0, 0));
		AddQuad(st, new Vector2(0, 1), new Vector2(1, 1), new Vector4(0, 0, 0, 0));
		AddQuad(st, new Vector2(1, 1), new Vector2(1, 1), new Vector4(0, 0, 0, 0));
		AddQuad(st, new Vector2(2, 1), new Vector2(1, 1), new Vector4(0, 0, 0, 0));
		AddQuad(st, new Vector2(0, 2), new Vector2(1, 1), new Vector4(0, 0, 0, 0));
		AddQuad(st, new Vector2(1, 2), new Vector2(1, 1), new Vector4(0, 0, 0, 0));
		AddQuad(st, new Vector2(2, 2), new Vector2(1, 1), new Vector4(0, 0, 0, 0));

        st.GenerateNormals();
        Mesh mesh = st.Commit();
        land.Mesh = mesh;
        AddChild(land);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
