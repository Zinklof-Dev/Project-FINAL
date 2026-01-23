using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public List<PlayableCharacter> partyMembers;

    public GameData()
    {
        partyMembers = new List<PlayableCharacter>(PlayableCharacter.partyMembers);
    }
}
