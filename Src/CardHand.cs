using Godot;
using System;
using System.Diagnostics;

public partial class CardHand : Node2D
{
	private bool _cardInputEnabled = false;
	
	[Export]
	public bool CardInputEnabled 
	{ 
		get => _cardInputEnabled;
		set
		{
			_cardInputEnabled = value;
			var cards = FindChildren("Card?", owned: false);
			
			if (cards.Count > 0)
			{
				foreach (CardElement card in cards) card.CardInputEnabled = _cardInputEnabled;
			}
		}
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
