/*using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private PlayerInputDisplay playerInputDisplay;
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private EnemyAI enemyAI;

    public enum Turn { EnemyTurn, PlayerTurn };
    public Turn currentTurn = Turn.PlayerTurn;

    public int maxEnemyActionPoints = 1;
    public int enemyActionPoints = 1;
    public int maxPlayerActionPoints = 1;
    public int playerActionPoints = 1;

    public void TakeAction(int pointsToDeduct)
    {
        switch(currentTurn)
        {
            case Turn.PlayerTurn:

                playerActionPoints -= pointsToDeduct;
                if(playerActionPoints <= 0)
                {
                    currentTurn = Turn.EnemyTurn;
                    playerActionPoints = maxPlayerActionPoints;
                    enemyActionPoints = maxEnemyActionPoints;
                    playerInputManager.state = PlayerInputManager.InputState.EnemyTurn;
                    enemyAI.currentState = EnemyAI.State.SelectingEnemyAndPlayer;
                }
                playerInputDisplay.SetCurrentTurnText();
                playerInputDisplay.SetActionsRemainingText();

                break;
        
            case Turn.EnemyTurn:

                enemyActionPoints -= pointsToDeduct;
                if(enemyActionPoints <= 0)
                {
                    currentTurn = Turn.PlayerTurn;
                    playerActionPoints = maxPlayerActionPoints;
                    enemyActionPoints = maxEnemyActionPoints;
                    playerInputManager.state = PlayerInputManager.InputState.SelectingPartyMember;
                    enemyAI.currentState = EnemyAI.State.PlayerTurn;
                }
                playerInputDisplay.SetCurrentTurnText();
                playerInputDisplay.SetActionsRemainingText();

                break;
        }
    }
}
*/