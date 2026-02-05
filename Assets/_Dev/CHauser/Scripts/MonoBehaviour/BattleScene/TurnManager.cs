using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public Agent currentTurnAgent;
    public int actionPoints = 10;
    public int maxActionPoints = 10;
    private int index = 0;


    public enum TurnState
    {
        Setup,
        PlayerTurn,
        EnemyTurn,
        Victory,
        Defeat
    }

    public TurnState turnState = TurnState.Setup;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        switch (turnState)
        {
            case TurnState.PlayerTurn:

                if (actionPoints <= 0 && ((PlayerInput.instance.inputState == PlayerInput.InputState.Move && PlayerInput.instance.moveInput == PlayerInput.MoveInput.SelectingGoal) || (PlayerInput.instance.inputState == PlayerInput.InputState.Attack && PlayerInput.instance.attackInput == PlayerInput.AttackInput.Selecting)))
                    UpdateActorTurn();

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

    void UpdateActorTurn()
    {
        index++;

        if(index > ActorManager.instance.partyMemberActors.Count - 1)
        {
            index = 0;
            // Add logic here to change to enemy turn
        }

        currentTurnAgent = ActorManager.instance.partyMemberActors[index].agent;

        actionPoints = maxActionPoints;

        PlayerInputUIManager.instance.UpdateActionPointsDisplay(actionPoints);
        PlayerInputUIManager.instance.UpdateActionPointsDisplayCost(0);
        PlayerInput.instance.pathVisualizer.positionCount = 0;
    }
}