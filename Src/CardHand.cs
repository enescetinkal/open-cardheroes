using Godot;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class CardHand : Node2D
{
	private bool _cardInputEnabled = false;
	private List<PlayingCard> cardsInHand { get; init; } = new List<PlayingCard>();
	private CardRoster roster { get; init; }  = new CardRoster();
	private CardElement[] Cards = null;
	private ushort updateInputAttempts = 0;
	private ushort maxAttemptInputSet = 5;
	
	public delegate void CardDrawnHandler(CardElement card, ushort remainingDraws);
	public event CardDrawnHandler CardDrawn;
	
	[Export]
	public bool CardInputEnabled 
	{ 
		get => _cardInputEnabled;
		set
		{
			_cardInputEnabled = value;
			UpdateInput();
		}
	}
	
	[Export]
	public bool IsPlayersHand { get; set; } = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Cards = this.FindChildren<CardElement>("Card?", owned: false);
		LoadCardRoster();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void DrawCards(ushort number = 1)
	{
		//represents the UI for the card
		CardElement cardObj = null;
		//cardObjs that are not visible aren't currently used
		foreach (var card in Cards)
		{
			if (!card.Visible)
			{
				cardObj = card;
				break;
			}
		}
		
		//TODO: draw random PlayingCard from player's available pool
		if (CardDrawn != null && cardObj != null)
		{
			number--;
			//Debug.WriteLine("Drawing card, remaining: " + number);
			CardDrawn(cardObj, number);
		}
	}
	
	private void UpdateInput()
	{
		if (Cards != null && Cards.Length > 0)
		{
			Parallel.ForEach(Cards, (CardElement card) =>
			{
				//Debug.WriteLine("Setting card input enabled");
				card.CardInputEnabled = _cardInputEnabled;
			});
		}
		
		//recursively try to update card input status
		else if (updateInputAttempts++ < maxAttemptInputSet)
		{
			Task.Run(() => 
			{
				Task.Delay(10);
				UpdateInput();
			});
		}
	}
	
	private void LoadCardRoster()
	{
		//TODO: load cards from savestate or default if no savestate
	}
}
