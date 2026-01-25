using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public List<PlayableCharacter> partyMembers;
    public List<PlayableCharacter> recruits;
    public bool generateRecruits;

    public GameData()
    {
        partyMembers = new List<PlayableCharacter>();
        recruits = new List<PlayableCharacter>();
        generateRecruits = true;
    }
}
