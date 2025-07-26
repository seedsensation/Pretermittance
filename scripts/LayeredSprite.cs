using Godot;
using System;

#nullable enable

[Tool]
public partial class LayeredSprite : Node2D
{

	private int _layerCount = 1;
	[Export]
	public int LayerCount
	{
		get => _layerCount;
		set => _layerCount = value <= 0 ? 1 : value;
	}

	private double _layerHeight = 1;
	[Export]
	public double LayerHeight
	{
		get => _layerHeight;
		set
		{
			_layerHeight = value <= 0 ? 0 : value;
			UpdateTree();
		}
	}

	private float _distance = 1;

	[Export]
	public float Distance {
		get => _distance;
		set
		{
			_distance = value <= 0 ? (float)0.005 : value;
			UpdateTree();
		} }

	[Export]
	public Texture2D? DarkAtlas { get; set; }

	[Export]
	public Texture2D? LightAtlas { get; set; }

	[Export]
	private bool Update
	{
		get => false;
		set => UpdateTree();
	}

	public AtlasTexture? CreateAtlas(Texture2D? atlas, Rect2 region)
	{
		if (atlas == null)
		{
			return null;
		}
		AtlasTexture Result = new AtlasTexture();
		Result.Atlas = atlas;
		Result.Region = region;
		return Result;
	}

	public void UpdateTree()
	{
		if (DarkAtlas == null)
		{
			GD.PushWarning("Missing DarkAtlas");
			return;
		}
		foreach (Node node in GetChildren())
		{
			node.Free();
		}
		int TextureWidth = DarkAtlas.GetWidth() / LayerCount;
		for (int i = 0; i < LayerCount; i++)
		{
			// Set up CanvasLayer
			CanvasLayer Layer = new CanvasLayer();
			Layer.Name = $"Layer{i+1}";
			Layer.FollowViewportEnabled = true;
			Layer.FollowViewportScale = Distance + (float)(LayerHeight * i);
			Layer.Scale = new Vector2(1 / Distance, 1 / Distance);

			// Set up Region, create textures from atlas
			Rect2 Region = new Rect2(i * TextureWidth, 0, TextureWidth, DarkAtlas.GetHeight());
			AtlasTexture DarkTexture = CreateAtlas(DarkAtlas, Region)!;
			AtlasTexture? LightTexture = CreateAtlas(LightAtlas, Region);

			// Create final sprite object, add to tree
			DarkSprite Sprite = new DarkSprite(DarkTexture, LightTexture);
			Sprite.Name = "LayerSprite";
			Layer.AddChild(Sprite);
			AddChild(Layer);
			Sprite.Owner = GetTree().EditedSceneRoot;
			Layer.Owner = GetTree().EditedSceneRoot;
			
		}




	}




	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		UpdateTree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
