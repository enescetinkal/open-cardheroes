using Godot;
using System;
using System.Diagnostics;

public partial class Root : Node
{
	private bool _showLaneDebugGraphic = false;
	private Node2D field;
	private Sprite2D fieldBackdrop;
	private Rect2 fieldBackdropRect;
	private ushort GamePhase { get; set; } = 0;
	private FieldSidePlayer playerSide;
	private FieldSideOpponent opponentSide;
	private bool ready = false;
	private bool introCompleted = false;
	private bool introBegan = false;
	
	[Export]
	public bool ShowLaneDebugGraphic 
	{ 
		get => _showLaneDebugGraphic;
		set
		{
			_showLaneDebugGraphic = value;
			UpdateLaneDebug();
		}
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (ShowLaneDebugGraphic) UpdateLaneDebug();
		field = (Node2D)GetNode("PlayingField");
		playerSide = GetNode<FieldSidePlayer>("PlayingField/Backdrop/FieldSidePlayer");
		opponentSide = GetNode<FieldSideOpponent>("PlayingField/Backdrop/FieldSideOpponent");
		fieldBackdrop = GetNode<Sprite2D>("PlayingField/Backdrop");
		fieldBackdropRect = new Rect2(fieldBackdrop.GlobalPosition, fieldBackdrop.Texture.GetSize());
	
		ready = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		RepositionFieldBasedOnViewport();
		
		if (ready && !introCompleted) PlayIntroSequence(delta);
	}
	
	private void RepositionFieldBasedOnViewport()
	{
		var viewportRect = GetViewport().GetVisibleRect();
		var fieldSizeish = new Vector2(fieldBackdropRect.Size.X + (fieldBackdropRect.Position.X - (fieldBackdropRect.Size.X / 2)), fieldBackdropRect.Size.Y + 150);//TODO: just use fieldBackdropSize once we have a real backdrop graphic
		var fieldRect = new Rect2(field.GlobalPosition, fieldSizeish);
		Vector2 newCorner;
		
		//Debug.WriteLine($"vx: {viewportRect.Size.X}, vy: {viewportRect.Size.Y}, fx: {fieldRect.Size.X * field.Transform.Scale.X}, fy: {fieldRect.Size.Y * field.Transform.Scale.Y}");
		newCorner = new Vector2(viewportRect.Size.X / 2 - ((fieldRect.Size.X * field.Transform.Scale.X) / 2), viewportRect.Size.Y / 2 - ((fieldRect.Size.Y * field.Transform.Scale.Y) / 2) );
		
		field.SetGlobalPosition(newCorner);
	}
	
	private void PlayIntroSequence(double delta)
	{
		//TODO: play intro then activate player
		if (!introBegan)
		{
			introBegan = true;
			//effectively draw the card
			playerSide.Activate();
		}
		introCompleted = true;
	}
	
	private void UpdateLaneDebug()
	{
		var lanes = FindChildren("FieldLane?", owned: false);
		
		if (lanes.Count > 0)
		{
			//Debug.WriteLine("showing lane debug");
			foreach (FieldLane lane in lanes) { lane.ShowDebugGraphics = _showLaneDebugGraphic; }
		}
	}
}

public enum GamePhases
{
	Player,
	Opponent,
	Trick
}
