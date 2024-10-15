using Godot;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public partial class FieldSidePlayer : Node2D
{
	private (CardElement card, Vector2 offset)? activeCardEvent = null;
	private Vector2? originalCardPosition = null;
	private float cardMoveTimeIncrement = 0;
	private bool mouseWasCentered = false;
	private Task nextMove = null;
	private CancellationTokenSource cancelTokenSrc = null;
	
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
			var cardPosition = activeCardEvent.Value.card.GlobalPosition;
			
			originalCardPosition = originalCardPosition ?? cardPosition;
			cardMoveTimeIncrement += (float)delta * 0.4f;
			
			//Debug.WriteLine("moving card...");
			if (((cardPosition.X - mousePosition.X) > 10 || (cardPosition.Y - mousePosition.Y) > 10) && !mouseWasCentered)
			{
				activeCardEvent?.card.SetGlobalPosition(cardPosition.Lerp(mousePosition, cardMoveTimeIncrement));
			}
			else
			{
				mouseWasCentered = true;
				activeCardEvent?.card.SetGlobalPosition(mousePosition);
			}
				
			activeCardEvent.Value.card.ZIndex = 1;
			// TODO: animate card movement
		}
	}
	
	void _on_Card_picked(CardElement card, Vector2 offset)
	{
		//Debug.WriteLine("Card was picked...");
		if (activeCardEvent == null)
		{
			originalCardPosition = null;
			//Debug.WriteLine("Card picked immediate: " + card.GlobalPosition);
			activeCardEvent = (card: card, offset: offset);
		}
		else
		{
			if (cancelTokenSrc != null) cancelTokenSrc.Cancel();
			
			//Debug.WriteLine("Card picked wait: " + card.GlobalPosition);
			Task.Run(async () =>
			{
				cancelTokenSrc = new CancellationTokenSource();
				await Task.Delay(100);
				//Debug.WriteLine("Card picked delayed: " + card.GlobalPosition);
				originalCardPosition = null;
				activeCardEvent = (card: card, offset: offset);
			}, cancelTokenSrc.Token).ContinueWith((t) => cancelTokenSrc = null);
		}
	}
	
	void _on_Card_dropped(CardElement card)
	{
		//Debug.WriteLine("Card was dropped...");
		//Debug.WriteLine($"Card dropped check: {card.GlobalPosition}, active: {activeCardEvent?.card.GlobalPosition}");
		if (activeCardEvent != null && activeCardEvent.Value.card == card) 
		{
			GetViewport().SetInputAsHandled();
			activeCardEvent.Value.card.ZIndex = 0;
			activeCardEvent = null;
			//Debug.WriteLine("Card dropped: " + originalCardPosition.Value);
			card.SetGlobalPosition(originalCardPosition.Value);
			originalCardPosition = null;
			mouseWasCentered = false;
		}
	}
}
