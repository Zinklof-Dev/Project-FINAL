using UnityEngine;

public class TurnManager : MonoBehaviour
{
  public enum Turn = { EnemyTurn, PlayerTurn };
  Turn currentTurn = Turn.PlayerTurn;

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
        }
        
        break;
        
      case Turn.EnemyTurn:
      
        enemyActionsCount--;
        if(enemyActionsCount == 0)
        {
          currentTurn = Turn.PlayerTurn;
          playerActionsCount = maxPlayerActionsCount;
          enemyActionsCount = maxEnemyActionsCount;
        }
        
        break;
    }
  }
}
