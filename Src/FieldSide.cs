using Godot;
using System;
using System.Diagnostics;

public abstract partial class FieldSide : Control
{
	private bool waitingOnCardDrawMove = false;
	private PathFollow2D drawMovePath { get; set; }
	private CardElement drawnCard { get; set; }
	private Node cardParent { get; set; }
	private (Vector2 scale, float rotation, Vector2 position) cardStats;
	private float cardSettleTimer = 0;
	private ushort remainingDraws = 0;
	protected CardHand myHand { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		myHand = GetNode<CardHand>("CardHand");
		myHand.CardDrawn += on_Card_drawn;
		drawMovePath = this.FindChild<PathFollow2D>("PathFollow2D", owned: false);
		cardParent = FindChild("CardContainer", owned: false);
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		DrawCardMove(delta);
	}
	
	///
	/// Summary: Used to begin a side's turn
	///
	public virtual void Activate()
	{
		myHand.DrawCards(2);
	}
	
	///
	/// Summary: Used to end a side's turn
	///
	protected abstract void Deactivate();
	
	protected void on_Card_drawn(CardElement card, ushort remainingDraws)
	{
		this.remainingDraws = remainingDraws;
		drawnCard = card;
		cardStats = (card.Scale, card.Rotation, card.Position);
		card.Reparent(drawMovePath);
		card.Visible = true;
		card.Scale = new Vector2(0.5f, 0.5f);
		card.Rotation = 0.43f;
		card.Position = new Vector2((card.Size.X/2) * -1, (card.Size.Y/2) * -1);
		waitingOnCardDrawMove = true;
		drawMovePath.ProgressRatio = 0;
	}
	
	private void DrawCardMove(double delta)
	{
		if (waitingOnCardDrawMove)
		{
			if (drawMovePath.ProgressRatio >= 1)
			{
				waitingOnCardDrawMove = false;
				drawnCard.Reparent(cardParent);
				drawMovePath.ProgressRatio = 0;
			}
			else
			{
				//Debug.WriteLine("card draw progress: " + drawMovePath.Progress);
				drawMovePath.ProgressRatio += (float)delta;
			}
		}
		
		if (!waitingOnCardDrawMove && drawnCard != null)
		{
			var scaleDiff = cardStats.scale.X - drawnCard.Scale.X;
			var rotDiff = cardStats.rotation - drawnCard.Rotation;
			var tmpDiff = cardStats.scale.Y - drawnCard.Scale.Y;
			
			if (tmpDiff > scaleDiff) scaleDiff = tmpDiff;
			
			if (cardSettleTimer < 1)
			{
				drawnCard.Scale = drawnCard.Scale.Lerp(cardStats.scale, cardSettleTimer);
				drawnCard.Rotation = Mathf.Lerp(drawnCard.Rotation, cardStats.rotation, cardSettleTimer);
				cardSettleTimer += (float)delta * 0.8f;
			}
			
			if (cardSettleTimer >= 1 || (scaleDiff < 0.2f && rotDiff < 0.2f))
			{
				drawnCard.Rotation = cardStats.rotation;
				drawnCard.Scale = cardStats.scale;
				drawnCard = null;
				cardSettleTimer = 0;
				//Debug.WriteLine("card draw finished. remaining: " + remainingDraws);
				
				if (remainingDraws > 0) myHand.DrawCards(remainingDraws);
			}
		}
	}
}
