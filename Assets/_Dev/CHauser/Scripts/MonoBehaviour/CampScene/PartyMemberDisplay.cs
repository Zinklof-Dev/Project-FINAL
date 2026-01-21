using TMPro;
using UnityEngine;

public class PartyMemberDisplay : MonoBehaviour
{
    public PlayableCharacter character;
    public TMP_Text characterTxt;

    public void DisplayInfo()
    {
        InfoDisplayer.instance.gameObject.SetActive(true);
        InfoDisplayer.UpdateDisplay(character);
    }

    public void Recruit()
    {
        PartyManager.recruits.Remove(character);
        PlayableCharacter.partyMembers.Add(character);
        PartyManager.instance.UpdatePartyDisplayContent();
        PartyManager.instance.UpdateRecruitDisplayContent();
    }
}
