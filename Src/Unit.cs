using Godot;
using System;

public partial class Unit : Element
{
	public PlayingCard UnitInfo { get; set; }
	public AnimatedSprite2D Placeholder { get; set; }
	public CollisionShape2D CollisionArea { get; set; }
	public AnimatedSprite2D Sprite { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		Placeholder = GetNode<AnimatedSprite2D>("UnitPlaceholder");
		CollisionArea = GetNode<CollisionShape2D>("Element/ElementBounds/ElementBoundsShape");
		Sprite = GetNode<AnimatedSprite2D>("Element/ElementSprite");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
