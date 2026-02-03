/*using TMPro;
using UnityEngine;

public class PlayerInputDisplay : MonoBehaviour
{
    PlayerInputManager playerInputManager;
    [SerializeField] TurnManager turnManager;

    [SerializeField] TMP_Text selectedActorText;
    [SerializeField] TMP_Text selectedGoalPositionText;
    [SerializeField] TMP_Text selectedEnemyText;
    [SerializeField] TMP_Text currentTurnText;
    [SerializeField] TMP_Text actionsRemainingText;
    [SerializeField] TMP_Text apCostText;

    [SerializeField] GameObject attackOrNavigatePrompt;

    [SerializeField] GameObject confirmNavigationPrompt;

    [SerializeField] GameObject confirmEnemyPrompt;

    [SerializeField] GameObject cancelEnemySelectionPrompt;

    private void Start()
    {
        playerInputManager = FindFirstObjectByType<PlayerInputManager>();
        turnManager = FindFirstObjectByType<TurnManager>();

        SetActionsRemainingText();
        SetCurrentTurnText();
    }

    public void SetSelectedNameText(string name)
    {
        selectedActorText.text = name;
    }

    public void SetSelectedEnemyText(string name)
    {
        selectedEnemyText.text = name;
    }

    public void SetSelectedGoalPositionText(string position)
    {
        selectedGoalPositionText.text = position;
    }

    public void ConfirmNavigation()
    {
        if (playerInputManager.state == PlayerInputManager.InputState.ConfirmingNavigation)
        {
            playerInputManager.confirmButtonHit = true;
        }
    }

    public void Clear(bool resetWhileInactive)
    {
        if (playerInputManager.state == PlayerInputManager.InputState.Inactive && !resetWhileInactive)
            return;

        playerInputManager.currentSelectedActor = null;
        playerInputManager.currentEnemySelectedActor = null;
        playerInputManager.lineRenderer.enabled = false;

        if(turnManager.currentTurn == TurnManager.Turn.PlayerTurn) 
            playerInputManager.state = PlayerInputManager.InputState.SelectingPartyMember;

        selectedActorText.text = "None";
        selectedGoalPositionText.text = "None";
        selectedEnemyText.text = "None";
    }

    public void PromtIfAttackingOrNavigating()
    {
        attackOrNavigatePrompt.SetActive(true);
    }

    public void PromtIfAttackingOrNavigatingResponse(bool isAttacking)
    {
        attackOrNavigatePrompt.SetActive(false);

        if (isAttacking)
        {
            playerInputManager.state = PlayerInputManager.InputState.SelectingEnemyToAttack;
            cancelEnemySelectionPrompt.SetActive(true);
            return;
        }

        playerInputManager.state = PlayerInputManager.InputState.SelectingGoal;
    }


    public void ConfirmPathPrompt()
    {
        confirmNavigationPrompt.SetActive(true);
    }

    public void ConfirmPath()
    {
        playerInputManager.confirmButtonHit = true;
        confirmNavigationPrompt.SetActive(false);
    }

    public void ConfirmEnemyPrompt()
    {
        confirmEnemyPrompt.SetActive(true);
        cancelEnemySelectionPrompt.SetActive(false);
    }
    public void ConfirmEnemy()
    {
        playerInputManager.enemyConfirmButtonHit = true;
        confirmEnemyPrompt.SetActive(false);
    }

    public void SetCurrentTurnText()
    {
        if (turnManager.currentTurn == TurnManager.Turn.PlayerTurn)
            currentTurnText.text = "Player";

        else if (turnManager.currentTurn == TurnManager.Turn.EnemyTurn)
            currentTurnText.text = "Enemy";
    }

    public void SetActionsRemainingText()
    {
        if (turnManager.currentTurn == TurnManager.Turn.PlayerTurn)
            actionsRemainingText.text = turnManager.playerActionPoints.ToString();

        else if (turnManager.currentTurn == TurnManager.Turn.EnemyTurn)
            actionsRemainingText.text = turnManager.enemyActionPoints.ToString();
    }
    public void SetAPCostText(int cost)
    {
        apCostText.text = cost.ToString();
    }
}
*/