using Godot;

public partial class TerrainGenerator : Node3D
{
    MeshInstance3D ground;
    const int _width = 256; // Width of the terrain in terms of quads
    const float _effect = 10; // Controls the height scaling of the terrain
    Texture2D texture;

    // Method to add a single quad to the terrain mesh, including UV mapping and vertex positioning
    public void AddQuad(SurfaceTool st, Vector2 pos, Vector2 size, Vector4 elevations)
    {
        // Calculate UV coordinates for texture mapping based on the quad's position
        Vector2 uv0 = new Vector2((pos.X + _width/2) / _width, (pos.Y + _width/2) / _width);
        Vector2 uv1 = new Vector2((pos.X + size.X + _width/2) / _width, (pos.Y + _width/2) / _width);
        Vector2 uv2 = new Vector2((pos.X + size.X + _width/2) / _width, (pos.Y + size.Y + _width/2) / _width);
        Vector2 uv3 = new Vector2((pos.X + _width/2) / _width, (pos.Y + size.Y + _width/2) / _width);
		
		// Add the vertices of the quad, each with a corresponding elevation value for height control
        st.SetUV(uv0);
        st.AddVertex(new Vector3(pos.X, elevations.X, pos.Y));
        st.SetUV(uv1);
        st.AddVertex(new Vector3(pos.X + size.X, elevations.Y, pos.Y));
        st.SetUV(uv2);
        st.AddVertex(new Vector3(pos.X + size.X, elevations.W, pos.Y + size.Y));

        st.SetUV(uv0);
        st.AddVertex(new Vector3(pos.X, elevations.X, pos.Y));
        st.SetUV(uv2);
        st.AddVertex(new Vector3(pos.X + size.X, elevations.W, pos.Y + size.Y));
        st.SetUV(uv3);
        st.AddVertex(new Vector3(pos.X, elevations.Z, pos.Y + size.Y));
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Generate a Perlin noise map for the terrain, which defines the height values
        var noise = new FastNoiseLite();
        noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
        noise.Frequency = 0.032f;
        noise.FractalOctaves = 4;
        
        // Create an image to store noise values for visualization or texture use
        Image noiseImage = Image.CreateEmpty(_width, _width, false, Image.Format.Rgb8);

        // Loop through each pixel in the image and assign a grayscale value based on noise
        for (int z = 0; z < _width; z++)
        {
            for (int x = 0; x < _width; x++)
            {
                float value = (noise.GetNoise2D(x, z) + 1) / 2; // Normalize noise to 0-1
                Color color = new Color(value, value, value); // Use grayscale color
                noiseImage.SetPixel(x, z, color);
            }
        }

        ImageTexture noiseTexture = ImageTexture.CreateFromImage(noiseImage);

        MeshInstance3D land = new MeshInstance3D();
        SurfaceTool st = new SurfaceTool();
        st.Begin(Mesh.PrimitiveType.Triangles);

        // Generate a grid of quads to form the terrain, each with height determined by noise
        for (int z = _width/-2; z < (_width / 2); z++) 
        {
            for (int x = _width / -2; x < (_width / 2); x++) 
            {
                AddQuad(st, new Vector2(x, z), new Vector2(1, 1), new Vector4(
                    noise.GetNoise2D(x, z) * _effect,
                    noise.GetNoise2D(x + 1, z) * _effect,
                    noise.GetNoise2D(x, z + 1) * _effect,
                    noise.GetNoise2D(x + 1, z + 1) * _effect
                ));
            }
        }

        // Generate normals to give the terrain correct lighting and shading
        st.GenerateNormals();
        Mesh mesh = st.Commit();
        land.Mesh = mesh;

        // Apply the noise texture to the terrain for a visual representation
        StandardMaterial3D material = new StandardMaterial3D();
        material.AlbedoTexture = noiseTexture;
        land.MaterialOverride = material;
        AddChild(land); // Add the terrain to the scene
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
