using Godot;
using System;

#nullable enable


[Tool]
public partial class AnimatedDarkSprite : Node2D
{
	[Export]
	public SpriteFrames? Frames { get; set; }

	[Export]
	public bool Update
	{
		get => false;
		set => UpdateTree();
	}

	private bool _flipH = false;

	[Export]
	public bool FlipH
	{
		get => _flipH;
		set
		{
			foreach (Node node in GetChildren()) {
                if (node is not AnimatedSprite2D)
                {
                    return;
                }
				(node as AnimatedSprite2D)!.FlipH = value;
            }
		}
	}


	public AnimatedDarkSprite(SpriteFrames frames)
	{
		Frames = frames;
	}

	public AnimatedDarkSprite() { }


	private static AnimatedSprite2D CreateSprite(SpriteFrames frames, bool light = false)
	{
		AnimatedSprite2D Sprite = new AnimatedSprite2D();
		Sprite.SpriteFrames = frames;

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
		foreach (Node node in GetChildren())
		{
			node.Free();
		}
		AnimatedSprite2D DarkNode = CreateSprite(Frames!);
		AddChild(DarkNode);
		//DarkNode.Owner = GetTree().EditedSceneRoot;
		AnimatedSprite2D LightNode = CreateSprite(Frames!, true);
		AddChild(LightNode);
		//LightNode.Owner = GetTree().EditedSceneRoot;
		Play("Idle");
	}

	public void Play(StringName animation)
	{
		GetNode<AnimatedSprite2D>("DarkSprite")?.Play($"Dark{animation}");
		GetNode<AnimatedSprite2D>("LightSprite")?.Play($"Light{animation}");
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
