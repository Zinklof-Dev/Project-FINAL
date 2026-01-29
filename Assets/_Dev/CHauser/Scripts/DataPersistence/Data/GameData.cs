using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public List<PlayableCharacter> partyMembers;
    public List<PlayableCharacter> recruits;
    public List<Card> cards;
    public bool generateRecruits;

    public GameData()
    {
        partyMembers = new List<PlayableCharacter>();
        recruits = new List<PlayableCharacter>();

        cards = new List<Card>(); cards.Add(new Card(Card.ItemType.Sword)); // Temop starter card hand

        generateRecruits = true;
    }
}
