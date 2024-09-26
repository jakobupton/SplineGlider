using Godot;

public partial class TerrainGenerator : Node3D
{
	MeshInstance3D ground;

	public void AddQuad(SurfaceTool st, float x, float z, float w, float h) 
	{
		st.AddVertex(new Vector3(x,     0, z));     // Vertex 0
        st.AddVertex(new Vector3(x + w, 0, z));     // Vertex 1
        st.AddVertex(new Vector3(x + w, 0, z + h)); // Vertex 2

		st.AddVertex(new Vector3(x,     0, z));     // Vertex 0
        st.AddVertex(new Vector3(x + w, 0, z + h)); // Vertex 2
        st.AddVertex(new Vector3(x,     0, z + h)); // Vertex 3
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        MeshInstance3D land = new MeshInstance3D();
        SurfaceTool st = new SurfaceTool();
        st.Begin(Mesh.PrimitiveType.Triangles);

        AddQuad(st, 0, 0, 1, 1);
		AddQuad(st, 1, 1, 1, 1);

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
