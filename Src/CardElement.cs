using Godot;
using System;
using System.Diagnostics;

public partial class CardElement : Node2D
{
	[Export]
	public bool CardInputEnabled { get; set; } = false;
	
	public delegate void CardPickedHandler(CardElement card, Vector2 offset);
	public delegate void CardDroppedHandler(CardElement card);
	public event CardPickedHandler CardPicked;
	public event CardDroppedHandler CardDropped;
	
	/// Where the card is in the player's hand
	private short initialPosition = -1;
	private CollisionShape2D shape;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Debug.WriteLine("card ready");
		shape = (CollisionShape2D)FindChild("CollisionShape2D", owned: false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _Input(InputEvent eventAction)
	{
		if (CardInputEnabled)
		{
			var cardRect = GetCardWorldRect();
			
			//if (eventAction is InputEventMouseButton tmp1) Debug.WriteLine($"card: {GlobalPosition} mouse input received: pressed?){tmp1.Pressed}");
			//if (eventAction is InputEventScreenTouch tmp2) Debug.WriteLine($"card: {GlobalPosition} touch input received: pressed?){tmp2.Pressed}");
			
			if (
				(
					eventAction is InputEventMouseButton tmpMouseEvent 
					&& tmpMouseEvent.ButtonIndex == MouseButton.Left
					&& (
						(tmpMouseEvent.Pressed && CardPicked != null && tmpMouseEvent.Position.IsInArea(cardRect))
						|| (!tmpMouseEvent.Pressed && CardDropped != null)
					)
				) 
				|| (
					eventAction is InputEventScreenTouch tmpTouchEvent
					&& (
						(tmpTouchEvent.Pressed && CardPicked != null && tmpTouchEvent.Position.IsInArea(cardRect))
						|| (!tmpTouchEvent.Pressed && CardDropped != null)
					)
				)
			)
			{
				//Debug.WriteLine($"Triggering card picked or dropped for card at: x){GlobalPosition.X}, y){GlobalPosition.Y}");
				if (eventAction is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
				{
					//Debug.WriteLine($"Mouse clicked at: x){mouseEvent.Position.X}, y){mouseEvent.Position.Y}");
					CardPicked(this, mouseEvent.Position);
					GetViewport().SetInputAsHandled();
				}
				else if (eventAction is InputEventScreenTouch touchEvent && touchEvent.Pressed)
				{
					//Debug.WriteLine($"Touched at: x){touchEvent.Position.X}, y){touchEvent.Position.Y}");
					CardPicked(this, touchEvent.Position);
					GetViewport().SetInputAsHandled();
				}
				else
				{
					CardDropped(this);
				}
			}
		}
	}
	
	private Rect2? GetCardWorldRect()
	{
		var shapeRectOriginal = shape?.Shape.GetRect();
		
		if (shapeRectOriginal != null)
		{
			var playerFieldScale = ((Node2D)FindParent("FieldSidePlayer")).Transform.Scale;
			var backdropScale = ((Node2D)FindParent("Backdrop")).Transform.Scale;
			var shapeSize = new Vector2(shapeRectOriginal.Value.Size.X * playerFieldScale.X * backdropScale.X, shapeRectOriginal.Value.Size.Y * playerFieldScale.Y * backdropScale.Y);
			
			return new Rect2(GlobalPosition, shapeSize);
		}
		else return null;
	}
}
