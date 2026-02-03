/*using UnityEngine;
using System.Collections.Generic;

public class InfoManager : MonoBehaviour
{
    static List<GameObject> infosPlayer = new List<GameObject>();
    static List<GameObject> infosEnemy = new List<GameObject>();
    [SerializeField] GameObject infoPrefab = null;
    [SerializeField] GameObject enemyInfoParent = null;
    [SerializeField] GameObject playerInfoParent = null;


    private void Start()
    {
        infosPlayer = new List<GameObject>();
        infosEnemy = new List<GameObject>();

        foreach (Actor playerActor in ActorManager.partyMemberActors)
        {
            GameObject info = Instantiate(infoPrefab, playerInfoParent.transform);
            Info information = info.GetComponent<Info>();
            information.assignedActor = playerActor;
            infosPlayer.Add(info);
        }
        foreach (Actor enemyActor in ActorManager.enemyActors)
        {
            GameObject info = Instantiate(infoPrefab, enemyInfoParent.transform);
            Info information = info.GetComponent<Info>();
            information.assignedActor = enemyActor;
            infosEnemy.Add(info);
        }

        UpdateInfos();
    }

    public static void UpdateInfos()
    {
        foreach (GameObject info in infosEnemy)
        {
            Info information = info.GetComponent<Info>();

            if (information.assignedActor == null)
                continue;

            information.name.text = "Name: " + information.assignedActor.name;
            information.attackPower.text = "Attack Power: " + information.assignedActor.attackPower;
            information.range.text = "Range: " + information.assignedActor.range;
            information.health.text = "Health: " + information.assignedActor.health + " / " + information.assignedActor.maxHealth;
        }

        foreach (GameObject info in infosPlayer)
        {
            Info information = info.GetComponent<Info>();

            if (information.assignedActor == null)
                continue;

            information.name.text = "Name: " + information.assignedActor.name;
            information.attackPower.text = "Attack Power: " + information.assignedActor.attackPower;
            information.range.text = "Range: " + information.assignedActor.range;
            information.health.text = "Health: " + information.assignedActor.health + " / " + information.assignedActor.maxHealth;
        }
    }

    private void Update()
    {
        UpdateInfos();
    }
}
*/