using Godot;
using System;
using System.Diagnostics;

public partial class FieldSidePlayer : Node2D
{
	private (CardElement card, Vector2 offset)? activeCardEvent = null;
	private Vector2? originalCardPosition = null;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var cards = FindChildren("Card?", owned: false);
		if (cards.Count > 0)
		{
			foreach (CardElement card in cards)
			{
				card.CardPicked += _on_Card_picked;
				card.CardDropped += _on_Card_dropped;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (activeCardEvent != null)
		{
			var mousePosition = GetViewport().GetMousePosition();
			
			originalCardPosition = originalCardPosition ?? activeCardEvent?.card.GlobalPosition;
			
			//Debug.WriteLine("moving card...");
			activeCardEvent?.card.SetGlobalPosition(mousePosition);
			// TODO: animate card movement
		}
	}
	
	void _on_Card_picked(CardElement card, Vector2 offset)
	{
		//Debug.WriteLine("Card was picked...");
		activeCardEvent = (card: card, offset: offset);
	}
	
	void _on_Card_dropped(CardElement card)
	{
		//Debug.WriteLine("Card was dropped...");
		activeCardEvent = null;
		card.SetGlobalPosition(originalCardPosition.Value);
		originalCardPosition = null;
	}
}
