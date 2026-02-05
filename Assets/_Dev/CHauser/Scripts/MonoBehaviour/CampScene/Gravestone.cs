using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gravestone : MonoBehaviour, IDataPersistence
{
    List<PlayableCharacter> deadPartyMembers;

    [SerializeField] GameObject deadDisplayPrefab;
    [SerializeField] RectTransform contentTransform;

    public void LoadData(GameData data)
    {
        deadPartyMembers = data.deadPartyMembers;
        CreateDisplay();
    }

    public void SaveData(ref GameData data)
    {
        data.deadPartyMembers = deadPartyMembers;
    }

    private void CreateDisplay()
    {
        foreach (PlayableCharacter character in deadPartyMembers)
        {
            GameObject display = Instantiate(deadDisplayPrefab, contentTransform);
            display.GetComponent<TMP_Text>().text = character.name;
        }

        contentTransform.sizeDelta = new Vector2(contentTransform.GetComponent<RectTransform>().sizeDelta.x, deadDisplayPrefab.GetComponent<RectTransform>().sizeDelta.y * deadPartyMembers.Count);
    }
}
