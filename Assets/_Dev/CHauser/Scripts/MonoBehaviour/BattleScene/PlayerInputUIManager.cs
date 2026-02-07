using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInputUIManager : MonoBehaviour
{
    [SerializeField] TMP_Text actionPointDisplayTxt;
    [SerializeField] TMP_Text actionPointDisplayCostTxt;
    [SerializeField] TMP_Text currentSquadMemberTxt;
    [SerializeField] public TMP_Text selectedEnemyTargetTxt;
    [SerializeField] Button attackModeButton;
    [SerializeField] Button moveModeButton;
    [SerializeField] Button endTurnButton;
    [SerializeField] GameObject confirmMovePrompt;

    public static PlayerInputUIManager instance;

    private void Awake()
    {
        instance = this;
        UpdateActionPointsDisplay(TurnManager.instance.actionPoints);
    }

    private void Update()
    {
        if(PlayerInput.instance.moveInput == PlayerInput.MoveInput.SelectingGoal)
            attackModeButton.interactable = true;
        else
            attackModeButton.interactable = false;

        if ((PlayerInput.instance.inputState == PlayerInput.InputState.Move && PlayerInput.instance.moveInput == PlayerInput.MoveInput.SelectingGoal) || (PlayerInput.instance.inputState == PlayerInput.InputState.Attack && PlayerInput.instance.attackInput == PlayerInput.AttackInput.Selecting))
            endTurnButton.interactable = true;
        else
            endTurnButton.interactable = false;

        if (PlayerInput.instance.attackAPCost <= TurnManager.instance.actionPoints)
            attackModeButton.interactable = true;
        else
            attackModeButton.interactable = false;

        currentSquadMemberTxt.text = "Current Squad Member: " + PlayerInput.instance.currentTurnActor.actorName; 
    }


    public void UpdateActionPointsDisplay(int currentAP)
    {
        actionPointDisplayTxt.text = "Action Points: " + currentAP.ToString();
    }

    public void UpdateActionPointsDisplayCost(int costAP)
    {
        actionPointDisplayCostTxt.text = "Cost: " + costAP.ToString();
    }

    public void AttackMode()
    {
        if(TurnManager.instance.turnState != TurnManager.TurnState.PlayerTurn || PlayerInput.instance.moveInput != PlayerInput.MoveInput.SelectingGoal)
            return;

        PlayerInput.instance.inputState = PlayerInput.InputState.Attack;
        PlayerInput.instance.targetEnemyActor = ActorManager.instance.enemyActors[0];
    }

    public void MoveMode()
    {
        if (TurnManager.instance.turnState != TurnManager.TurnState.PlayerTurn)
            return;

        PlayerInput.instance.inputState = PlayerInput.InputState.Move;
    }

    public void ConfirmMovePrompt()
    {
        confirmMovePrompt.SetActive(true);
    }

    public void CancelMove()
    {
        PlayerInput.instance.moveInput = PlayerInput.MoveInput.SelectingGoal;
        confirmMovePrompt.SetActive(false);
    }

    public void ConfirmMove()
    {
        PlayerInput.instance.moveConfirmed = true;
        confirmMovePrompt.SetActive(false);
    }

    public void ChangeSelectedEnemyTarget(int incriment)
    {
        int index = ActorManager.instance.enemyActors.IndexOf(PlayerInput.instance.targetEnemyActor);

        index += incriment;

        if (index < 0)
        {
            index = ActorManager.instance.enemyActors.Count - 1;
        }
        else if (index > ActorManager.instance.enemyActors.Count - 1)
        {
            index = 0;
        }

        PlayerInput.instance.targetEnemyActor = ActorManager.instance.enemyActors[index];
    }

    public void EndTurn()
    {
        PlayerInput.instance.moveInput = PlayerInput.MoveInput.SelectingGoal;
        PlayerInput.instance.attackInput = PlayerInput.AttackInput.Selecting;
        TurnManager.instance.actionPoints = 0;
    }

    public void AttackButtonHit()
    {
        PlayerInput.instance.attackInput = PlayerInput.AttackInput.Attacking;
    }
}
