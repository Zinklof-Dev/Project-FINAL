using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ZinklofDev.ConsoleV2;

public class PartyManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] RectTransform partyDisplayContent;
    [SerializeField] RectTransform recruitDisplayContent;
    [SerializeField] GameObject partyMemberDisplayPrefab;
    [SerializeField] GameObject recruitDisplayPrefab;
    [SerializeField] InfoDisplayer infoDisplayer;

    public static List<PlayableCharacter> recruits = new List<PlayableCharacter>();
    public static PartyManager instance;

    private List<GameObject> partyMemberDisplays = new List<GameObject>();
    private List<GameObject> recruitDisplays = new List<GameObject>();

    private void Start()
    {
        instance = this;

        // TEMP
        /*PlayableCharacter character1 = new PlayableCharacter("Tom", "Ur MOMMMMMM", 100, 1, 1, 1, true);
        PlayableCharacter character2 = new PlayableCharacter("Mary", "Ur MOMMMMMM", 100, 1, 1, 1, true);
        PlayableCharacter character3 = new PlayableCharacter("Bob", "Ur MOMMMMMM", 100, 1, 1, 1, true);
        PlayableCharacter character4 = new PlayableCharacter("Johnny", "Ur MOMMMMMM", 100, 1, 1, 1, true);*/

        // TEMP
        recruits.Add(new PlayableCharacter("Jerry", "Ur MOMMMMMM", 100, 1, 1, 1, false));
        recruits.Add(new PlayableCharacter("Rick", "Ur MOMMMMMM", 100, 1, 1, 1, false));
        recruits.Add(new PlayableCharacter("Morty", "Ur MOMMMMMM", 100, 1, 1, 1, false));
        recruits.Add(new PlayableCharacter("Beth", "Ur MOMMMMMM", 100, 1, 1, 1, false));

        InfoDisplayer.instance = infoDisplayer;
        UpdatePartyDisplayContent();
        UpdateRecruitDisplayContent();
    } 

    public void UpdatePartyDisplayContent()
    {
        for (int i = 0; i < 100000; i++)
        {
            if (partyMemberDisplays.Count == 0) break;
            GameObject display = partyMemberDisplays[0];
            partyMemberDisplays.Remove(display);
            Destroy(display);
        }

        foreach (PlayableCharacter character in PlayableCharacter.partyMembers)
        {
            GameObject display = Instantiate(partyMemberDisplayPrefab, partyDisplayContent);
            display.GetComponent<PartyMemberDisplay>().character = character;
            TMP_Text text = display.GetComponent<PartyMemberDisplay>().characterTxt;
            text.text = character.name;
            partyMemberDisplays.Add(display);
        }

        partyDisplayContent.GetComponent<RectTransform>().sizeDelta = new Vector2(partyDisplayContent.GetComponent<RectTransform>().sizeDelta.x, partyMemberDisplayPrefab.GetComponent<RectTransform>().sizeDelta.y * PlayableCharacter.partyMembers.Count);
    }

    public void UpdateRecruitDisplayContent()
    {
        for (int i = 0; i < 100000; i++)
        {
            if (recruitDisplays.Count == 0) break;
            GameObject display = recruitDisplays[0];
            recruitDisplays.Remove(display);
            Destroy(display);
        }

        foreach (PlayableCharacter recruit in recruits)
        {
            GameObject display = Instantiate(recruitDisplayPrefab, recruitDisplayContent);
            display.GetComponent<PartyMemberDisplay>().character = recruit;
            TMP_Text text = display.GetComponent<PartyMemberDisplay>().characterTxt;
            text.text = recruit.name;
            recruitDisplays.Add(display);
        }
        recruitDisplayContent.GetComponent<RectTransform>().sizeDelta = new Vector2(recruitDisplayContent.GetComponent<RectTransform>().sizeDelta.x, recruitDisplayPrefab.GetComponent<RectTransform>().sizeDelta.y * recruits.Count);
    }

    private void GenerateRecruits()
    {

    }

    public void LoadData(GameData data)
    {
        PlayableCharacter.partyMembers = data.partyMembers;
        UpdatePartyDisplayContent();
    }
    public void SaveData(ref GameData data)
    {
        data.partyMembers = PlayableCharacter.partyMembers;
    }
}