using Godot;

public partial class TerrainGenerator : Node3D
{
	MeshInstance3D ground;
	Color _color = Color.Color8(0, 240, 200, 1);

	public void AddQuad(SurfaceTool st, Vector2 pos, Vector2 size, Vector4 elevations) 
	{
		st.SetColor(_color);
		st.AddVertex(new Vector3(pos.X,          elevations.X, pos.Y));          // Vertex 0
		st.SetColor(_color);
        st.AddVertex(new Vector3(pos.X + size.X, elevations.Y, pos.Y));          // Vertex 1
		st.SetColor(_color);
        st.AddVertex(new Vector3(pos.X + size.X, elevations.W, pos.Y + size.Y)); // Vertex 2

		st.SetColor(_color);
		st.AddVertex(new Vector3(pos.X,          elevations.X, pos.Y));          // Vertex 0
		st.SetColor(_color);
        st.AddVertex(new Vector3(pos.X + size.X, elevations.W, pos.Y + size.Y)); // Vertex 2
		st.SetColor(_color);
        st.AddVertex(new Vector3(pos.X,          elevations.Z, pos.Y + size.Y)); // Vertex 3
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var noise = new FastNoiseLite();
        noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
        noise.Frequency = 0.032f;

        MeshInstance3D land = new MeshInstance3D();
        SurfaceTool st = new SurfaceTool();
        st.Begin(Mesh.PrimitiveType.Triangles);

		float effect = 10;

		for (int z = 0; z < 32; z++) 
		{
			for (int x = 0; x < 32; x++) 
			{
				AddQuad(st, new Vector2(x, z), new Vector2(1, 1), new Vector4(noise.GetNoise2D(x, z) * effect, noise.GetNoise2D(x + 1, z) * effect, noise.GetNoise2D(x, z + 1) * effect, noise.GetNoise2D(x + 1, z + 1) * effect));
			}
		}

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
