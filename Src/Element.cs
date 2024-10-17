using Godot;
using System;

public partial class Element : Node2D
{
	private Vector2? _size = null;
	public CollisionShape2D CollisionShape;
	private FieldSide playerField;
	private FieldSide opponentField;
	private FieldSide fieldSide;
	
	public Vector2 Size
	{
		get
		{
			if (_size == null) GetWorldRect();
			
			return _size.Value;
		}
		set
		{
			_size = value;
		}
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CollisionShape = this.FindChild<CollisionShape2D>("ElementBoundsShape", owned: false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public Rect2? GetWorldRect()
	{
		if (playerField == null) playerField = this.FindParent<FieldSide>("FieldSidePlayer");
		if (opponentField == null) opponentField = this.FindParent<FieldSide>("FieldSideOpponent");
		if (fieldSide == null) fieldSide = playerField ?? opponentField;
		
		var retVal = NodeHelper.GetRectWithScale(GlobalPosition, CollisionShape, this.Scale * fieldSide.Scale);
		
		if (_size == null && retVal != null)
		{
			//Debug.WriteLine("card size: " + retVal.Value.Size);
			_size = retVal.Value.Size;
		}
		
		return retVal;
	}
}
