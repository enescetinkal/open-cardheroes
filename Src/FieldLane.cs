using Godot;
using System;
using System.Diagnostics;

public partial class FieldLane : Node2D
{
	[Export]
	public bool ShowDebugGraphics { get; set; } = false;
	
	private Sprite2D border = null;
	
	public Unit LaneUnit { get; set; }
	public Unit SecondaryUnit { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Debug.WriteLine("lane ready");
		border = (Sprite2D)GetNode("LaneBorderFirst");
		LaneUnit = this.FindChild<Unit>("LaneUnit", owned: false);
		SecondaryUnit = this.FindChild<Unit>("SecondaryUnit", owned: false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (border != null)
		{
			if (ShowDebugGraphics) border.Visible = true;
			else border.Visible = false;
		}
	}
}
