using Godot;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

public record PlayingCard
{
	public CardFamily Family { get; set; } = CardFamily.None;
	public CardAffinity Affinity { get; set; } = CardAffinity.None;
	public ushort ResourceCost { get; set; } = 1;
	public ushort HealthMax { get; set; } = 1;
	public ushort CurrentHealth { get; set; } = 1;
	public ushort AttackValue { get; set; } = 1;
	public bool IsInStasis { get; set; } = false;
	public bool HasSecondaryAttackPhase { get; set; } = false;
	public bool IsSupportUnit { get; set; } = false;
	public string TexturePath { get; set; } = String.Empty;
	///
	/// Summary: For buff/action cards
	///
	public bool AppliesToLane { get; set; } = false;
	///
	/// Summary: For buff/action cards
	///
	public bool AppliesToUnit { get; set; } = false;
	///
	/// Summary: For action cards
	///
	public ushort DrawsCards { get; set; } = 0;
}

public class CardRoster
{
	///
	/// Summary: The player's active card roster, <numCards, Card>
	///
	public Dictionary<ushort, PlayingCard> Roster { get; init; } = new Dictionary<ushort, PlayingCard>();
	///
	/// Summary: The cards the user has obtained
	///
	public List<PlayingCard> CardLibrary { get; init; } = new List<PlayingCard>();
	///
	/// Summary: All cards in the game
	///
	public static List<PlayingCard> CardsRepository { get; set; } = new List<PlayingCard>();
	
	public void LoadCards()
	{
		throw new NotImplementedException();
		//TODO: get card definitions
	}
	
	public List<PlayingCard> GetAvailableCardsForFamilies(CardFamily families)
	{
		throw new NotImplementedException();
		//TODO: get cards to present to user based on hero selection/family and player's owned/unlocked cards
	}
}

///
/// Summary: includes both elements/classes and specific heroes
///
[Flags]
public enum CardFamily
{
	None
}

//
public enum CardAffinity
{
	None
}
