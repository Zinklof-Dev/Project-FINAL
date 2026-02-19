using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public List<PlayableCharacter> partyMembers;
    public List<PlayableCharacter> recruits;
    public List<PlayableCharacter> deadPartyMembers;
    public List<Card> cards;
    public List<PlayableCharacter> squad;
    public bool generateRecruits;

    public GameData()
    {
        partyMembers = new List<PlayableCharacter>();
        recruits = new List<PlayableCharacter>();
        deadPartyMembers = new List<PlayableCharacter>();
        squad = new List<PlayableCharacter>();

        cards = new List<Card>();

        // Temop starter card hand
        Card card1 = new Card(Card.ItemType.Sword);
        Card card2 = new Card(Card.ItemType.Axe);
        Card card3 = new Card(Card.ItemType.Health_Supporter);
        Card card4 = new Card(Card.ItemType.Action_Points_Supporter);

        cards.Add(card1);
        cards.Add(card2);
        cards.Add(card1);
        cards.Add(card2);
        cards.Add(card3);
        cards.Add(card4);

        generateRecruits = true;
    }
}
