using Godot;
using System;
using System.Diagnostics;

public partial class CardElement : Control
{
	[Export]
	public bool CardInputEnabled { get; set; } = false;
	
	public delegate void CardPickedHandler(CardElement card, Vector2 offset);
	public delegate void CardMouseUpHandler(CardElement card);
	public event CardPickedHandler CardPicked;
	public event CardMouseUpHandler CardMouseUp;
	public CollisionShape2D CollisionArea { get; set; }
	
	/// Where the card is in the player's hand
	private short initialPosition = -1;
	private static Vector2? _size;
	private PlayingCard _cardStats;
	private FieldSide playerField = null;
	private FieldSide opponentField = null;
	private FieldSide fieldSide = null;
	private CardHand cardHand = null;
	
	public PlayingCard CardStats 
	{ 
		get => _cardStats; 
		set
		{
			_cardStats = value;
			//TODO: set card texture based on stats object
		}
	}
	
	public new Vector2 Size
	{
		get
		{
			if (_size == null) GetCardWorldRect();
			
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
		//Debug.WriteLine("card ready");
		CollisionArea = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");
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
						(tmpMouseEvent.Pressed && CardPicked != null && tmpMouseEvent.Position.IsInCenteredArea(cardRect))
						|| (!tmpMouseEvent.Pressed && CardMouseUp != null)
					)
				) 
				|| (
					eventAction is InputEventScreenTouch tmpTouchEvent
					&& (
						(tmpTouchEvent.Pressed && CardPicked != null && tmpTouchEvent.Position.IsInCenteredArea(cardRect))
						|| (!tmpTouchEvent.Pressed && CardMouseUp != null)
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
					CardMouseUp(this);
				}
			}
		}
	}
	
	public Rect2? GetCardWorldRect()
	{
		if (playerField == null) playerField = this.FindParent<FieldSidePlayer>("FieldSidePlayer");
		if (opponentField == null) opponentField = this.FindParent<FieldSideOpponent>("FieldSideOpponent");
		if (fieldSide == null) fieldSide = playerField ?? opponentField;
		if (cardHand == null) cardHand = fieldSide.GetNode<CardHand>("CardHand");

		var retVal = NodeHelper.GetRectWithScale(GlobalPosition, CollisionArea, this.Scale * fieldSide.Scale * cardHand.Transform.Scale);
		
		if (_size == null && retVal != null)
		{
			//Debug.WriteLine("card size: " + retVal.Value.Size);
			_size = retVal.Value.Size;
		}
		
		return retVal;
	}
}
