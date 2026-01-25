using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using ZinklofDev.ConsoleV2;

public class PartyManager : MonoBehaviour, IDatapersistence
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

        // TEMP
        /*PlayableCharacter character1 = new PlayableCharacter("Tom", "Ur MOMMMMMM", 100, 1, 1, 1, true);
        PlayableCharacter character2 = new PlayableCharacter("Mary", "Ur MOMMMMMM", 100, 1, 1, 1, true);
        PlayableCharacter character3 = new PlayableCharacter("Bob", "Ur MOMMMMMM", 100, 1, 1, 1, true);
        PlayableCharacter character4 = new PlayableCharacter("Johnny", "Ur MOMMMMMM", 100, 1, 1, 1, true);*/

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
        System.Random random = new System.Random();

        for(int i = 0; i < numberToGenerate; i++)
        {
            if(random.Next(0, 100) <= 25) // 25% chance to generate a combo name recruit
            {
                string comboName = DataPersistenceManager.instance.namesList.comboNames[random.Next(0, DataPersistenceManager.instance.namesList.comboNames.Count)];
                foreach(PlayableCharacter existingRecruit in recruits)
                {
                    if(existingRecruit.name == comboName)
                    {
                        comboName += " Jr."; // simple way to avoid duplicates for now
                        break;
                    }
                }
                PlayableCharacter recruit = new PlayableCharacter(comboName, "A special recruit.", 150, 1, 1, 1, false); // placeholder stats other than name
                recruits.Add(recruit);
                continue;
            }

            string firstName = DataPersistenceManager.instance.namesList.firstNames[random.Next(0, DataPersistenceManager.instance.namesList.firstNames.Count)];
            string lastName = DataPersistenceManager.instance.namesList.lastNames[random.Next(0, DataPersistenceManager.instance.namesList.lastNames.Count)];
            string fullName = firstName + " " + lastName;

            foreach (PlayableCharacter existingRecruit in recruits)
            {
                if (existingRecruit.name == fullName)
                {
                    fullName += " Jr."; // simple way to avoid duplicates for now
                    break;
                }
            }

            PlayableCharacter newRecruit = new PlayableCharacter(fullName, "A new recruit.", 100, 1, 1, 1, false); // placeholder stats other than name
            recruits.Add(newRecruit);
        }
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