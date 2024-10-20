using Godot;
using System;

public partial class Unit : Element
{
	public PlayingCard UnitInfo { get; set; }
	public AnimatedSprite2D Placeholder { get; set; }
	public CollisionShape2D CollisionArea { get; set; }
	public AnimatedSprite2D Sprite { get; set; }
	public delegate void OnDestroyedHandler();
	public event OnDestroyedHandler OnDestroyed;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		Placeholder = GetNode<AnimatedSprite2D>("UnitPlaceholder");
		CollisionArea = GetNode<CollisionShape2D>("Element/ElementBounds/ElementBoundsShape");
		Sprite = GetNode<AnimatedSprite2D>("Element/ElementSprite");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}
	
	public void ReceiveBuffDebuff(short? healthAmount, short? damageAmount)
	{
		if (UnitInfo != null)
		{
			if (healthAmount != null)
			{
				UnitInfo.MaxHealth += healthAmount;
				UnitInfo.CurrentHealth += healthAmount;
			}
			
			if (damageAmount != null) UnitInfo.AttackValue += damageAmount;
			
			//TODO: animate changes??
		}
		else ;//TODO: do something
	}
	
	public void ReceiveAttack(ushort damageAmount)
	{
		if (UnitInfo != null)
		{
			if (UnitInfo.CurrentHealth > damageAmount) UnitInfo.CurrentHealth -= damageAmount;
			else
			{
				UnitInfo.CurrentHealth = 0;
				if (OnDestroyed != null) OnDestroyed(this);
			}
		}
		else ; //TODO: do something
	}
}
