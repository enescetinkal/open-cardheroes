using Godot;
using System;

public partial class FieldSideOpponent : FieldSide
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}
	
	public override void Activate()
	{
		throw new NotImplementedException();
	}
	
	protected override void Deactivate()
	{
		throw new NotImplementedException();
	}
}
