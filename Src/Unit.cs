using Godot;
using System;

public partial class Unit : Element
{
	public PlayingCard UnitInfo { get; set; }
	public AnimatedSprite2D Placeholder { get; set; }
	public CollisionShape2D CollisionArea { get; set; }
	public AnimatedSprite2D Sprite { get; set; }
	public delegate void OnDestroyedHandler(Unit destroyedUnit);
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
				if (healthAmount > 0)
				{
					UnitInfo.HealthMax += (ushort)healthAmount.Value;
					UnitInfo.CurrentHealth += (ushort)healthAmount.Value;
				}
				else
				{
					short tmpMax = (short)UnitInfo.HealthMax;
					short tmpHealth = (short)UnitInfo.CurrentHealth;
					
					tmpMax += healthAmount.Value;
					tmpHealth += healthAmount.Value;
					
					if (tmpMax < 0) tmpMax = 0;
					if (tmpHealth < 0) tmpHealth = 0;
					
					UnitInfo.HealthMax = (ushort)tmpMax;
					UnitInfo.CurrentHealth = (ushort)tmpHealth;
				}
			}
			
			if (damageAmount != null)
			{
				if (damageAmount > 0)
				{
					UnitInfo.AttackValue += (ushort)damageAmount;
				}
				else
				{
					short tmpDmg = (short)UnitInfo.AttackValue;
					
					tmpDmg += damageAmount.Value;
					
					if (tmpDmg < 0) tmpDmg = 0;
					
					UnitInfo.AttackValue = (ushort)tmpDmg;
				}
			}
			
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
