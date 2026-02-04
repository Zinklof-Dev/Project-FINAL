using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public List<PlayableCharacter> partyMembers;
    public List<PlayableCharacter> recruits;
    public List<PlayableCharacter> deadPartyMembers;
    public List<Card> cards;
    public bool generateRecruits;

    public GameData()
    {
        partyMembers = new List<PlayableCharacter>();
        recruits = new List<PlayableCharacter>();
        deadPartyMembers = new List<PlayableCharacter>();

        cards = new List<Card>();

        // Temop starter card hand
        Card card1 = new Card(Card.ItemType.Sword);
        Card card2 = new Card(Card.ItemType.Axe);
        cards.Add(card1);
        cards.Add(card2);

        generateRecruits = true;
    }
}
