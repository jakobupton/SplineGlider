using Godot;

public partial class TerrainGenerator : Node3D
{
	MeshInstance3D ground;
	const int _width = 256; // Number of Quads
    const float _effect = 10; // Height of Quads
	Texture2D texture;

    public void AddQuad(SurfaceTool st, Vector2 pos, Vector2 size, Vector4 elevations) 
	{
		st.AddVertex(new Vector3(pos.X,          elevations.X, pos.Y));          // Vertex 0
        st.AddVertex(new Vector3(pos.X + size.X, elevations.Y, pos.Y));          // Vertex 1
        st.AddVertex(new Vector3(pos.X + size.X, elevations.W, pos.Y + size.Y)); // Vertex 2

		st.AddVertex(new Vector3(pos.X,          elevations.X, pos.Y));          // Vertex 0
        st.AddVertex(new Vector3(pos.X + size.X, elevations.W, pos.Y + size.Y)); // Vertex 2
        st.AddVertex(new Vector3(pos.X,          elevations.Z, pos.Y + size.Y)); // Vertex 3
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var noise = new FastNoiseLite();
        noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
        noise.Frequency = 0.032f;
		noise.SetFractalOctaves(4);


		var img_noise = new Image();
		for (int z = 0; z < _width; z++)
		{
            for (int x = 0; x < _width; x++)
            {
                float value = noise.GetNoise2D(x, z);
                img_noise.SetPixel(x, z, new Color(value, value, value));
            }

        }

        MeshInstance3D land = new MeshInstance3D();
        SurfaceTool st = new SurfaceTool();
        st.Begin(Mesh.PrimitiveType.Triangles);
		var mat = new StandardMaterial3D();
        mat.AlbedoColor = new Color(1f, 1f, 1f);
		st.SetMaterial(mat);





        for (int z = _width/-2; z < (_width / 2); z++) 
		{
			for (int x = _width / -2; x < (_width / 2); x++) 
			{
				AddQuad(st, new Vector2(x, z), new Vector2(1, 1), new Vector4(noise.GetNoise2D(x, z) * _effect, noise.GetNoise2D(x + 1, z) * _effect, noise.GetNoise2D(x, z + 1) * _effect, noise.GetNoise2D(x + 1, z + 1) * _effect));
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
