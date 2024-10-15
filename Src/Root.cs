using Godot;
using System;
using System.Diagnostics;

public partial class Root : Node
{
	private bool _showLaneDebugGraphic = false;
	private Node2D field;
	private Sprite2D fieldBackdrop;
	private Rect2 fieldBackdropRect;
	
	[Export]
	public bool ShowLaneDebugGraphic 
	{ 
		get => _showLaneDebugGraphic;
		set
		{
			_showLaneDebugGraphic = value;
			UpdateLaneDebug();
		}
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (ShowLaneDebugGraphic) UpdateLaneDebug();
		field = (Node2D)GetNode("PlayingField");
		fieldBackdrop = (Sprite2D)GetNode("PlayingField/Backdrop");
		fieldBackdropRect = new Rect2(fieldBackdrop.GlobalPosition, fieldBackdrop.Texture.GetSize());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var viewportRect = GetViewport().GetVisibleRect();
		var fieldSizeish = new Vector2(fieldBackdropRect.Size.X + (fieldBackdropRect.Position.X - (fieldBackdropRect.Size.X / 2)), fieldBackdropRect.Size.Y + 150);//TODO: just use fieldBackdropSize once we have a real backdrop graphic
		var fieldRect = new Rect2(field.GlobalPosition, fieldSizeish);
		Vector2 newCorner;
		
		//Debug.WriteLine($"vx: {viewportRect.Size.X}, vy: {viewportRect.Size.Y}, fx: {fieldRect.Size.X * field.Transform.Scale.X}, fy: {fieldRect.Size.Y * field.Transform.Scale.Y}");
		newCorner = new Vector2(viewportRect.Size.X / 2 - ((fieldRect.Size.X * field.Transform.Scale.X) / 2), viewportRect.Size.Y / 2 - ((fieldRect.Size.Y * field.Transform.Scale.Y) / 2) );
		
		field.SetGlobalPosition(newCorner);
	}
	
	private void UpdateLaneDebug()
	{
		var lanes = FindChildren("FieldLane?", owned: false);
		
		if (lanes.Count > 0)
		{
			//Debug.WriteLine("showing lane debug");
			foreach (FieldLane lane in lanes) { lane.ShowDebugGraphics = _showLaneDebugGraphic; }
		}
	}
}

public static class Extensions
{
	public static bool IsInArea(this Vector2 thisCoord, Rect2? area)
	{
		if (area == null)
		{
			//Debug.WriteLine("Can't check object in area.  Area is null.");
			return false;
		}
		
		//Debug.WriteLine($"Check if: {thisCoord} is in: {area}, {new Vector2(area.Value.Position.X - (area.Value.Size.X / 2), area.Value.Position.Y - (area.Value.Size.Y / 2))}, {new Vector2(area.Value.Position.X + (area.Value.Size.X / 2), area.Value.Position.Y + (area.Value.Size.Y / 2))}");
		var isInside = (thisCoord.X >= (area?.Position.X - (area?.Size.X / 2)) && thisCoord.X <= (area?.Position.X + (area?.Size.X / 2 )));
		
		//Debug.WriteLine("Object is inside x: " + isInside);
		if (!isInside) return false;
		
		isInside = (thisCoord.Y >= (area?.Position.Y - (area?.Size.Y / 2 )) && thisCoord.Y <= (area?.Position.Y + (area?.Size.Y /  2)));
		
		//Debug.WriteLine("Object is inside y: " + isInside);
		return isInside;
	}
}
