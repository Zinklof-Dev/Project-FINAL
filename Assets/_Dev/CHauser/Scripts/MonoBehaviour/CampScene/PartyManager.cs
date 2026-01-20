using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PartyManager : MonoBehaviour
{
    [SerializeField] RectTransform partyDisplayContent;
    [SerializeField] RectTransform recruitDisplayContent;
    [SerializeField] GameObject partyMemberDisplayPrefab;
    [SerializeField] GameObject recruitDisplayPrefab;

    List<PlayableCharacter> recruits = new List<PlayableCharacter>();

    private void Start()
    {
        // TEMP
        PlayableCharacter character1 = new PlayableCharacter("Tom", "Ur MOMMMMMM", 100, 1, 1, 1, true);
        PlayableCharacter character2 = new PlayableCharacter("Mary", "Ur MOMMMMMM", 100, 1, 1, 1, true);
        PlayableCharacter character3 = new PlayableCharacter("Bob", "Ur MOMMMMMM", 100, 1, 1, 1, true);
        PlayableCharacter character4 = new PlayableCharacter("Johnny", "Ur MOMMMMMM", 100, 1, 1, 1, true);

        // TEMP
        recruits.Add(new PlayableCharacter("Jerry", "Ur MOMMMMMM", 100, 1, 1, 1, false));
        recruits.Add(new PlayableCharacter("Rick", "Ur MOMMMMMM", 100, 1, 1, 1, false));
        recruits.Add(new PlayableCharacter("Morty", "Ur MOMMMMMM", 100, 1, 1, 1, false));
        recruits.Add(new PlayableCharacter("Beth", "Ur MOMMMMMM", 100, 1, 1, 1, false));

        UpdatePartyDisplayContent();
        UpdateRecruitDisplayContent();
    }

    private void UpdatePartyDisplayContent()
    {

        foreach (PlayableCharacter character in PlayableCharacter.partyMembers)
        {
            GameObject display = Instantiate(partyMemberDisplayPrefab, partyDisplayContent);
            display.GetComponent<PartyMemberDisplay>().character = character;
            TMP_Text text = display.GetComponentInChildren<TMP_Text>();
            text.text = character.name;
        }

        partyDisplayContent.GetComponent<RectTransform>().sizeDelta = new Vector2(partyDisplayContent.GetComponent<RectTransform>().sizeDelta.x, partyMemberDisplayPrefab.GetComponent<RectTransform>().sizeDelta.y * PlayableCharacter.partyMembers.Count);
    }

    private void UpdateRecruitDisplayContent()
    {
        foreach (PlayableCharacter recruit in recruits)
        {
            GameObject display = Instantiate(recruitDisplayPrefab, recruitDisplayContent);
            display.GetComponent<PartyMemberDisplay>().character = recruit;
            TMP_Text text = display.GetComponentInChildren<TMP_Text>();
            text.text = recruit.name;
        }
        recruitDisplayContent.GetComponent<RectTransform>().sizeDelta = new Vector2(recruitDisplayContent.GetComponent<RectTransform>().sizeDelta.x, recruitDisplayPrefab.GetComponent<RectTransform>().sizeDelta.y * recruits.Count);
    }
}