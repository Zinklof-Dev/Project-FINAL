using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public int actionPoints = 10;
    public int maxActionPoints = 10;
    public int currentPlayerIndex = 0;
    private int currentEnemyTurnIndex = 0;


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
                    UpdatePlayerActorTurn();

                PlayerInput.instance.PlayerTurn();

                PlayerInputUIManager.instance.gameObject.SetActive(true);

                break;

            case TurnState.EnemyTurn:

                EnemyAI.instance.EnemyTurn();

                PlayerInputUIManager.instance.gameObject.SetActive(false);

                break;

            case TurnState.Victory:
                // Handle victory logic
                break;

            case TurnState.Defeat:
                // Handle defeat logic
                break;
        }
    }

    public void UpdatePlayerActorTurn()
    {
        if (ActorManager.instance.partyMemberActors.Count == 0)
        {
            turnState = TurnState.Defeat;
        }

        currentPlayerIndex++;
        if (PlayerInput.instance.currentTurnActor != null)
            PlayerInput.instance.currentTurnActor.healthBar.SetActive(true);

        if (currentPlayerIndex > ActorManager.instance.partyMemberActors.Count - 1)
        {
            currentPlayerIndex = 0;
            turnState = TurnState.EnemyTurn;
        }
        else
            PlayerInputUIManager.instance.MoveMode();

        PlayerInput.instance.currentTurnActor = ActorManager.instance.partyMemberActors[currentPlayerIndex];

        actionPoints = maxActionPoints;

        PlayerInputUIManager.instance.UpdateActionPointsDisplay(actionPoints);
        PlayerInputUIManager.instance.UpdateActionPointsDisplayCost(0);
        PlayerInput.instance.pathVisualizer.positionCount = 0;
    }

    public void UpdateEnemyActorTurn()
    {
        if (ActorManager.instance.enemyActors.Count == 0)
        {
            turnState = TurnState.Victory;
        }

        currentEnemyTurnIndex++;

        if (currentEnemyTurnIndex > ActorManager.instance.enemyActors.Count - 1)
        {
            currentEnemyTurnIndex = 0;
            turnState = TurnState.PlayerTurn;
            PlayerInputUIManager.instance.MoveMode();
        }

        actionPoints = maxActionPoints;
        EnemyAI.instance.currentTurnActor = ActorManager.instance.enemyActors[currentEnemyTurnIndex];

    }
}