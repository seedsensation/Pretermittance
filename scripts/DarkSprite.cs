using Godot;
using System;

#nullable enable


[Tool]
public partial class DarkSprite : Node2D
{
	[Export]
	public Texture2D? DarkTexture { get; set; }

	[Export]
	public Texture2D? LightTexture { get; set; }

	[Export]
	public bool Update
	{
		get => false;
		set => UpdateTree();
	}


	public DarkSprite(Texture2D darkTexture, Texture2D? lightTexture)
	{
		DarkTexture = darkTexture;
		LightTexture = lightTexture;
	}

	public DarkSprite() { }


	private static Sprite2D CreateSprite(Texture2D texture, bool light = false)
	{
		Sprite2D Sprite = new Sprite2D();
		Sprite.Texture = texture;

		ShaderMaterial SpriteMaterial = new ShaderMaterial();
		SpriteMaterial.Shader = GD.Load<Shader>((light) ?
			"res://shaders/hide_in_light.gdshader"
			: "res://shaders/smooth_filter.gdshader"
			);

		Sprite.Material = SpriteMaterial;

		Sprite.Name = (light) ? "LightSprite" : "DarkSprite";
		return Sprite;
	}

	public void UpdateTree()
	{
		if (DarkTexture == null)
		{
			GD.PushWarning("Missing DarkTexture");
			return;
		}
		foreach (Node node in GetChildren())
		{
			node.Free();
		}
		Sprite2D DarkNode = CreateSprite(DarkTexture);
		AddChild(DarkNode);
		DarkNode.Owner = GetTree().EditedSceneRoot;
		if (LightTexture != null)
		{
			Sprite2D LightNode = CreateSprite(LightTexture!);
			AddChild(LightNode);
			LightNode.Owner = GetTree().EditedSceneRoot;
		}
		
	}



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("READY");
		UpdateTree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
