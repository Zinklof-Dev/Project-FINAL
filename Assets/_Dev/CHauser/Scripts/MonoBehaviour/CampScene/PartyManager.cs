using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartyManager : MonoBehaviour, IDataPersistence
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
    private bool generateRecriuts = false;

    private void Start()
    {
        instance = this;
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

    private void GenerateRecruits(int numberToGenerate)
    {
        DataPersistenceManager.LoadNamesList();
        DataPersistenceManager.LoadBackgroundsList();
        System.Random random = new System.Random();
        string background;
        string name;

        for (int i = 0; i < numberToGenerate; i++)
        {
            background = DataPersistenceManager.instance.backgroundsList.backgrounds[random.Next(0, DataPersistenceManager.instance.backgroundsList.backgrounds.Count)];
            name = "";

            if (random.Next(0, 100) <= 25) // 25% chance to generate a combo name recruit
            {
                name = DataPersistenceManager.instance.namesList.comboNames[random.Next(0, DataPersistenceManager.instance.namesList.comboNames.Count)];
            }
            else
            {
                string firstName = DataPersistenceManager.instance.namesList.firstNames[random.Next(0, DataPersistenceManager.instance.namesList.firstNames.Count)];
                string lastName = DataPersistenceManager.instance.namesList.lastNames[random.Next(0, DataPersistenceManager.instance.namesList.lastNames.Count)];
                name = firstName + " " + lastName;
            }

            name = AvoidDuplicateNames(name);

            PlayableCharacter newRecruit = new PlayableCharacter(name, background, 100, 1, 1, 1, false); // placeholder stats other than name + description
            recruits.Add(newRecruit);
        }
    }

    public string AvoidDuplicateNames(string name)
    {
        foreach (PlayableCharacter existingRecruit in recruits)
        {
            if (existingRecruit.name == name)
            {
                name += " Jr."; // simple way to avoid duplicates for now
                break;
            }
        }

        return name;
    }

    public void LoadData(GameData data)
    {
        PlayableCharacter.partyMembers = data.partyMembers;
        recruits = data.recruits;
        generateRecriuts = data.generateRecruits;

        if (generateRecriuts && recruits.Count == 0)
        {
            GenerateRecruits(4); // TEMP number
            generateRecriuts = false;
        }

        UpdatePartyDisplayContent();
        UpdateRecruitDisplayContent();
    }
    public void SaveData(ref GameData data)
    {
        data.partyMembers = PlayableCharacter.partyMembers;
        data.recruits = recruits;
        data.generateRecruits = generateRecriuts;
    }
}