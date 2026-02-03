using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour, IDataPersistence
{
    List<PlayableCharacter> squad = new List<PlayableCharacter>();
    List<Agent> agents = new List<Agent>();
    

    [SerializeField] private GameObject agentPrefab;
    [SerializeField] private List<int> playerStartIndicies;


    private void SpawnAgents()
    {
        foreach (PlayableCharacter character in squad)
        {
            GameObject agentGameObject = Instantiate(agentPrefab);
            Agent agent = agentGameObject.GetComponent<Agent>();
            agent.playableCharacter = character;
            agent.currentIndex = playerStartIndicies[squad.IndexOf(character)];
            agents.Add(agent);
        }
    }
    public void LoadData(GameData data)
    {
        squad = data.squad;

        // TEMP
        squad.Add(new PlayableCharacter("Hero", "Hero", 10, 5, 3, false));
        squad.Add(new PlayableCharacter("Hero2", "Hero2", 10, 5, 3, false));
        squad.Add(new PlayableCharacter("Hero3", "Hero3", 10, 5, 3, false));
        squad.Add(new PlayableCharacter("Hero4", "Hero4", 10, 5, 3, false));

        SpawnAgents();
    }

    public void SaveData(ref GameData data)
    {
        data.squad = squad;
    }
}
