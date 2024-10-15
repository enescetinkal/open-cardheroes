using Godot;
using System;

public partial class ElementArea : Area2D
{
	delegate void enteredHandler(Area2D area);
	delegate void exitedHandler(Area2D area);
	event enteredHandler entered;
	event exitedHandler exited;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void _on_area_entered(Area2D area)
	{
		if (entered != null) entered(area);
	}
	
	public void _on_area_exited(Area2D area)
	{
		if (exited != null) exited(area);
	}
}
