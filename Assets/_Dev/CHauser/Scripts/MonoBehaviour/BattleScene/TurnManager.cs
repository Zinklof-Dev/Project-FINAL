using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public Agent currentTurnAgent;


    public enum TurnState
    {
        PlayerTurn,
        EnemyTurn,
        Victory,
        Defeat
    }

    public TurnState turnState = TurnState.PlayerTurn;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        switch(turnState)
        {
            case TurnState.PlayerTurn:
                PlayerInput.instance.PlayerTurn();
                break;

            case TurnState.EnemyTurn:
                // Handle enemy turn logic
                break;

            case TurnState.Victory:
                // Handle victory logic
                break;

            case TurnState.Defeat:
                // Handle defeat logic
                break;
        }
    }
}
