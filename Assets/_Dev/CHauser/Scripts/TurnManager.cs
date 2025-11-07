using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private PlayerInputDisplay playerInputDisplay;
    [SerializeField] private PlayerInputManager playerInputManager;

    public enum Turn { EnemyTurn, PlayerTurn };
    public Turn currentTurn = Turn.PlayerTurn;

    public int maxEnemyActionsCount = 1;
    public int enemyActionsCount = 1;
    public int maxPlayerActionsCount = 1;
    public int playerActionsCount = 1;

    public void TakeAction()
    {
        switch(currentTurn)
        {
            case Turn.PlayerTurn:
      
                playerActionsCount--;
                if(playerActionsCount == 0)
                {
                    currentTurn = Turn.EnemyTurn;
                    playerActionsCount = maxPlayerActionsCount;
                    enemyActionsCount = maxEnemyActionsCount;
                    playerInputManager.state = PlayerInputManager.InputState.EnemyTurn;
                }
                playerInputDisplay.SetCurrentTurnText();
                playerInputDisplay.SetActionsRemainingText();

                break;
        
            case Turn.EnemyTurn:
      
                enemyActionsCount--;
                if(enemyActionsCount == 0)
                {
                    currentTurn = Turn.PlayerTurn;
                    playerActionsCount = maxPlayerActionsCount;
                    enemyActionsCount = maxEnemyActionsCount;
                    playerInputManager.state = PlayerInputManager.InputState.SelectingPartyMember;
                }
                playerInputDisplay.SetCurrentTurnText();
                playerInputDisplay.SetActionsRemainingText();

                break;
        }
    }
}
