using Godot;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

public static class NodeHelper
{
	public static T[] FindChildren<T>(this Node theNode, string pattern, bool recursive = true, bool owned = true) where T : Node
	{
		var tmpVal = theNode.FindChildren(pattern, recursive: recursive, owned: owned);
		var retVal = new List<T>();
		
		Parallel.ForEach(tmpVal, val =>
		{
			retVal.Add((T)val);
		});
		
		return retVal.ToArray();
	}
	
	public static T FindParent<T>(this Node theNode, string pattern) where T : Node
	{
		return (T)theNode.FindParent(pattern);
	}
	
	public static T FindChild<T>(this Node theNode, string pattern, bool recursive = true, bool owned = true) where T : Node
	{
		return (T)theNode.FindChild(pattern, recursive: recursive, owned: owned);
	}
	
	public static bool IsInCenteredArea(this Vector2 thisCoord, Rect2? area)
	{
		if (area == null)
		{
			//Debug.WriteLine("Can't check object in area.  Area is null.");
			return false;
		}
		
		//Debug.WriteLine($"Check if: {thisCoord} is in: {area}, {new Vector2(area.Value.Position.X - (area.Value.Size.X / 2), area.Value.Position.Y - (area.Value.Size.Y / 2))}, {new Vector2(area.Value.Position.X + (area.Value.Size.X / 2), area.Value.Position.Y + (area.Value.Size.Y / 2))}");
		var isInside = (thisCoord.X >= (area?.Position.X - (area?.Size.X / 2)) && thisCoord.X <= (area?.Position.X + (area?.Size.X / 2 )));
		
		//Debug.WriteLine("Object is inside x: " + isInside);
		if (!isInside) return false;
		
		isInside = (thisCoord.Y >= (area?.Position.Y - (area?.Size.Y / 2 )) && thisCoord.Y <= (area?.Position.Y + (area?.Size.Y /  2)));
		
		//Debug.WriteLine("Object is inside y: " + isInside);
		return isInside;
	}
	
	public static Rect2? GetRectWithScale(Vector2 objectPosition, CollisionShape2D? shape, Vector2? additionalScaleFactor = null)
	{
		var shapeRectOriginal = shape?.Shape.GetRect();
		
		if (shapeRectOriginal != null)
		{
			additionalScaleFactor = additionalScaleFactor ?? new Vector2(1,1);
			
			var shapeRectSize = shapeRectOriginal.Value.Size;
			var shapeSize = new Vector2(shapeRectSize.X * additionalScaleFactor.Value.X, shapeRectSize.Y * additionalScaleFactor.Value.Y);
			var centerOffset = new Vector2(objectPosition.X + (shapeSize.X / 2), objectPosition.Y + (shapeSize.Y / 2));
			var retVal = new Rect2(centerOffset, shapeSize);
			
			//Debug.WriteLine("card world rect: " + retVal);
			
			return retVal;
		}
		else return null;
	}
}
