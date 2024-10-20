using Godot;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public partial class FieldSidePlayer : FieldSide
{
	private (CardElement card, Vector2 offset)? activeCardEvent = null;
	private Vector2? originalCardPosition = null;
	private float cardMoveTimeIncrement = 0;
	private bool mouseWasCentered = false;
	private CancellationTokenSource cancelTokenSrc = null;
	private List<Unit> laneUnits = new List<Unit>();
	private List<Unit> secondaryUnits = new List<Unit>();
	private FieldLane[] lanes;
	private Dictionary<Unit, Action> UnitHooks = new Dictionary<Unit, Action>();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		
		var cards = this.FindChildren<CardElement>("Card?", owned: false);
		
		lanes = this.FindChildren<FieldLane>("FieldLane?", owned: false);
		
		Parallel.ForEach(lanes, lane =>
		{
			laneUnits.Add(lane.LaneUnit);
			secondaryUnits.Add(lane.SecondaryUnit);
		});
		
		if (cards.Length > 0)
		{
			foreach (CardElement card in cards)
			{
				card.CardPicked += _on_Card_picked;
				card.CardMouseUp += _on_Card_mouse_up;
			}
		}
	}
	
	//TODO: set up turn advancement

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
		
		HandleCardMovement((float)delta);
	}
	
	public override void Activate()
	{
		base.Activate();
	}
	
	protected override void Deactivate()
	{
		throw new NotImplementedException();
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
	
	void _on_Card_mouse_up(CardElement card)
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
			mouseWasCentered = false;
			
			HandleUnitPlacementLogic(card);
			
			originalCardPosition = null;
		}
	}
	
	private void HandleUnitPlacementLogic(CardElement card)
	{
		var laneUnitSlot = laneUnits.FirstOrDefault(u => card.GlobalPosition.IsInCenteredArea(u.GetWorldRect()));
		var secondaryUnitSlot = secondaryUnits.FirstOrDefault(u => card.GlobalPosition.IsInCenteredArea(u.GetWorldRect()));
		//var laneAbilitySlot = laneAbilitySlot.FirstOrDefault(a => card.GlobalPosition.IsInCenteredArea(a.GetWorldRect()));
		var cardStats = card.CardStats;
		
		if (laneUnitSlot != null)
		{
			if (!cardStats.IsSecondaryUnit && !cardStats.IsAbilityCard) SetUpUnitSlot(cardStats, laneUnitSlot);
			else ;//TODO: show an error
		}
		else if (secondaryUnitSlot != null)
		{
			if (cardStats.IsSecondaryUnit) SetUpUnitSlot(cardStats, secondaryUnitSlot);
			else ;//TODO: show an error
		}
		//else if (cardStats.IsAbilityCard) TODO: apply the ability to lane
	}
	
	private void SetUpUnitSlot(PlayingCard cardStats, Unit unitSlot)
	{
		unitSlot.UnitInfo = cardStats;
		unitSlot.Sprite.SpriteFrames = cardStats.TexturePath;
		UnitHooks.Add(unitSlot, () => HandleUnitAppearFinished(unitSlot));
		unitSlot.Sprite.AnimationFinished += UnitHooks[unitSlot];
		unitSlot.Sprite.Play("appear");
	}
	
	public void HandleUnitAppearFinished(Unit unitSlot)
	{
		unitSlot.Sprite.AnimationFinished -= UnitHooks[unitSlot];
		UnitHooks.Remove(unitSlot);
		unitSlot.Sprite.Play("default");
	}
	
	private void HandleCardMovement(float delta)
	{		
		if (activeCardEvent != null)
		{
			var activeCard = activeCardEvent.Value.card;
			var mousePosition = GetViewport().GetMousePosition();
			var mouseOffset = new Vector2(mousePosition.X - (activeCard.Size.X / 2), mousePosition.Y - (activeCard.Size.Y / 2));
			var cardPosition = new Vector2(activeCard.GlobalPosition.X, activeCard.GlobalPosition.Y);
			
			originalCardPosition = originalCardPosition ?? cardPosition;
			cardMoveTimeIncrement += delta * 0.6f;
			
			//TODO: toggle highlight for possible target areas
			
			//Debug.WriteLine($"moving card: {cardPosition}, mouse pos: {mousePosition}, mouse off: {mouseOffset}");
			if (((cardPosition.X - mouseOffset.X) > 10 || (cardPosition.Y - mouseOffset.Y) > 10) && !mouseWasCentered)
			{
				//Debug.WriteLine("moving card, setting position: " + cardPosition);
				activeCard.SetGlobalPosition(cardPosition.Lerp(mouseOffset, cardMoveTimeIncrement));
			}
			else
			{
				mouseWasCentered = true;
				activeCard.SetGlobalPosition(mouseOffset);
				cardMoveTimeIncrement = 0;
			}
			
			activeCard.ZIndex = 1;
			// TODO: animate card movement
		}
	}
}
