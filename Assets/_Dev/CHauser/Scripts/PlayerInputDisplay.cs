using TMPro;
using UnityEngine;

public class PlayerInputDisplay : MonoBehaviour
{
    PlayerInputManager playerInputManager;

    [SerializeField] TMP_Text selectedActorText;
    [SerializeField] TMP_Text selectedGoalPositionText;
    [SerializeField] TMP_Text selectedEnemyText;

    [SerializeField] GameObject attackOrNavigatePrompt;

    [SerializeField] GameObject confirmNavigationPrompt;

    [SerializeField] GameObject confirmEnemyPrompt;

    private void Start()
    {
        playerInputManager = FindFirstObjectByType<PlayerInputManager>();
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
    }
    public void ConfirmEnemy()
    {
        playerInputManager.enemyConfirmButtonHit = true;
        confirmEnemyPrompt.SetActive(false);
    }
}
