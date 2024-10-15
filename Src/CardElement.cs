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
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Debug.WriteLine("card ready");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _Input(InputEvent eventAction)
	{
		if (CardInputEnabled)
		{
			//Debug.WriteLine("card input received: " + eventAction.GetType().Name);
			if (
				(
					eventAction is InputEventMouseButton tmpMouseEvent 
					&& tmpMouseEvent.ButtonIndex == MouseButton.Left
					&& (
						(tmpMouseEvent.Pressed && CardPicked != null)
						|| (!tmpMouseEvent.Pressed && CardDropped != null)
					)
				) 
				|| (
					eventAction is InputEventScreenTouch tmpTouchEvent
					&& (
						(tmpTouchEvent.Pressed && CardPicked != null)
						|| (!tmpTouchEvent.Pressed && CardDropped != null)
					)
				)
			)
			{
				//Debug.WriteLine("Triggering card picked or dropped...");
				if (eventAction is InputEventMouseButton mouseEvent && mouseEvent.Pressed) CardPicked(this, mouseEvent.Position);
				else if (eventAction is InputEventScreenTouch touchEvent && touchEvent.Pressed) CardPicked(this, touchEvent.Position);
				else CardDropped(this);
				
				GetViewport().SetInputAsHandled();
			}
		}
	}
}
