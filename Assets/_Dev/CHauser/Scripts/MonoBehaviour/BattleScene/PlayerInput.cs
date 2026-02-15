using System.Collections.Generic;
using UnityEngine;
using ZinklofDev.Utils.MathZ;


public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;

    public bool moveConfirmed = false;

    public Actor targetEnemyActor;

    [SerializeField] public LineRenderer pathVisualizer;

    public int attackAPCost = 5;

    int tentativeGoalIndex = -1;
    int prevTentativeGoalIndex;

    public Actor currentTurnActor;

    public enum InputState
    {
        Move,
        Attack,
        EnemyTurn
    }

    public InputState inputState = InputState.Move;

    public enum MoveInput
    {
        SelectingGoal,
        ConfirmingMove,
        Moving
    }

    public MoveInput moveInput = MoveInput.SelectingGoal;

    public enum AttackInput
    {
        Selecting,
        Attacking,
        AttackAnimation
    }

    public AttackInput attackInput = AttackInput.Selecting;


    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (TurnManager.instance.turnState != TurnManager.TurnState.PlayerTurn || inputState != InputState.Move)
        {
            pathVisualizer.gameObject.SetActive(false);
        }
    }

    int currentActionPointCost = 0;

    public void PlayerTurn()
    {
        switch (inputState)
        {
            case InputState.Move:
                HandleMoveInput();
                break;

            case InputState.Attack:
                HandleAttackInput();
                break;

            case InputState.EnemyTurn:

                break;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////

    public void HandleMoveInput()
    {
        switch (moveInput)
        {
            case MoveInput.SelectingGoal:
                SelectingGoal();
                break;
            case MoveInput.ConfirmingMove:
                ConfirmingMove();
                break;
            case MoveInput.Moving:
                MovingGoal();
                break;
        }
    }

    public void SelectingGoal()
    {

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Vector2 closestPoint = new Vector2(Numbers.RoundToMultiple(hit.point.x, GridSystem.instance.tileSize, GridSystem.instance.offsetX), Numbers.RoundToMultiple(hit.point.z, GridSystem.instance.tileSize, GridSystem.instance.offsetY));
            int hitIndex = GridSystem.instance.points.IndexOf(closestPoint);

            //Debug.Log("Closest Point: " + closestPoint + " | Hit Index: " + hitIndex);

            if (hitIndex != -1)
            {
                tentativeGoalIndex = hitIndex;
            }

            else
            {
                PlayerInputUIManager.instance.UpdateActionPointsDisplayCost(0);
                pathVisualizer.positionCount = 0;
                return;
            }
        }

        pathVisualizer.gameObject.SetActive(true);

        // Temp input for testing, will replace with Input System later

        if (Input.GetMouseButtonDown(0))
        {
            moveInput = MoveInput.ConfirmingMove;
            PlayerInputUIManager.instance.ConfirmMovePrompt();
        }

        if (prevTentativeGoalIndex == tentativeGoalIndex && moveInput != MoveInput.ConfirmingMove)
            return;

        prevTentativeGoalIndex = tentativeGoalIndex;

        List<int> visualPath = PathFinding.AStarPath(currentTurnActor.agent.currentIndex, tentativeGoalIndex, 0.5f);

        if (visualPath == null)
        {
            pathVisualizer.positionCount = 0;
            moveInput = MoveInput.SelectingGoal;
            PlayerInputUIManager.instance.CancelMove();
            PlayerInputUIManager.instance.UpdateActionPointsDisplayCost(0);
            return;
        }

        int speed = Mathf.RoundToInt(currentTurnActor.speed);
        currentActionPointCost = (visualPath.Count - 1) / speed;
        PlayerInputUIManager.instance.UpdateActionPointsDisplayCost(currentActionPointCost);

        if (currentActionPointCost > TurnManager.instance.actionPoints)
        {
            pathVisualizer.positionCount = 0;
            moveInput = MoveInput.SelectingGoal;
            PlayerInputUIManager.instance.UpdateActionPointsDisplayCost(0);
            PlayerInputUIManager.instance.CancelMove();
            return;
        }

        visualPath = PathFinding.TrimPath(visualPath);

        pathVisualizer.positionCount = visualPath.Count;

        foreach (int index in visualPath)
        {
            pathVisualizer.SetPosition(visualPath.IndexOf(index), new Vector3(GridSystem.instance.points[index].x, 0.2f, GridSystem.instance.points[index].y));
        }
    }

    public void ConfirmingMove()
    { 
        if (moveConfirmed)
        {
            moveConfirmed = false;
            currentTurnActor.agent.goalIndex = tentativeGoalIndex;
            currentTurnActor.agent.StartNavigation();
            moveInput = MoveInput.Moving;
            TurnManager.instance.actionPoints -= currentActionPointCost;
            PlayerInputUIManager.instance.UpdateActionPointsDisplay(TurnManager.instance.actionPoints);
            PlayerInputUIManager.instance.UpdateActionPointsDisplayCost(0);
            pathVisualizer.gameObject.SetActive(false);
        }
    }

    public void MovingGoal()
    {
        if (currentTurnActor.agent.currentState == Agent.State.Moving)
            return;

        moveInput = MoveInput.SelectingGoal;
        BattleCameraMover.instance.MoveCameraToMoveSelectionPosition();
    }

    ///////////////////////////////////////////////////////////////////////////////////

    void HandleAttackInput()
    {
        switch(attackInput)
        {
            case AttackInput.Selecting:
                AttackSelecting();
                break;
            
            case AttackInput.Attacking:
                AttackWithWeapon(currentTurnActor.character.GetWeapon());
                break;
        }
    }

    void AttackSelecting()
    {
        if (attackAPCost > TurnManager.instance.actionPoints)
        {
            inputState = InputState.Move;
            moveInput = MoveInput.SelectingGoal;
            PlayerInputUIManager.instance.MoveMode();
        }

        PlayerInputUIManager.instance.selectedEnemyTargetTxt.text = targetEnemyActor.actorName;
    }

    void AttackWithWeapon(Card card)
    {
        if (card == null)
        {
            PlayerInputUIManager.instance.UpdateActionPointsDisplay(TurnManager.instance.actionPoints);
            PlayerInputUIManager.instance.UpdateActionPointsDisplayCost(0);
            Debug.Log("No weapon!");
            attackInput = AttackInput.Selecting;
            return;
        }

        switch (card.cardSubClass)
        {
            case Card.CardSubClass.Two_Handed:
                AttackTwoHanded();
                break;

            case Card.CardSubClass.Polearm:
                AttackPolearm();
                break;

            case Card.CardSubClass.One_Handed:
                AttackOneHanded();
                break;

            case Card.CardSubClass.Bow:
                AttackBow();
                break;

            case Card.CardSubClass.Crossbow:
                AttackCrossbow();
                break;

            case Card.CardSubClass.Special:
                AttackSpecial();
                break;
        }
    }

    void AttackTwoHanded()
    {

    }

    void AttackPolearm()
    {

    }

    void AttackOneHanded()
    {
        if (!InLineOfSight())
        {
            Miss();
            Debug.Log("Not in line of sight!");
            return;
        }

        List<int> pathToEnemy = PathFinding.AStarPath(currentTurnActor.agent.currentIndex, targetEnemyActor.agent.currentIndex, 100);

        if (pathToEnemy == null)
        {
            Debug.Log("Path is null, something went wrong");
            return;
        }

        if (pathToEnemy.Count - 1 > currentTurnActor.agent.GetComponent<Actor>().range)
        {
            Miss();
            Debug.Log("Too far!");
            return;
        }

        Hit(currentTurnActor.agent.GetComponent<Actor>().attackPower);
    }

    void AttackBow()
    {

    }

    void AttackCrossbow()
    {

    }

    void AttackSpecial()
    {

    }

    private void Hit(float damage)
    {
        targetEnemyActor.TakeDamage(damage);
        TurnManager.instance.actionPoints -= attackAPCost;
        PlayerInputUIManager.instance.UpdateActionPointsDisplay(TurnManager.instance.actionPoints);
        PlayerInputUIManager.instance.UpdateActionPointsDisplayCost(0);
        Debug.Log("Hit!");

        // Temp, going to send to an animation of the attack state later
        attackInput = AttackInput.Selecting;
    }

    private void Miss()
    {
        TurnManager.instance.actionPoints -= attackAPCost;
        PlayerInputUIManager.instance.UpdateActionPointsDisplay(TurnManager.instance.actionPoints);
        PlayerInputUIManager.instance.UpdateActionPointsDisplayCost(0);
        Debug.Log("Miss!");

        // Temp, going to send to an animation of the attack state later
        attackInput = AttackInput.Selecting;
    }

    bool InLineOfSight()
    {
        RaycastHit hit;
        Vector3 currentTurnAgentPosition = currentTurnActor.gameObject.transform.position;
        Vector3 lineOfSightDirection =  targetEnemyActor.transform.position - currentTurnAgentPosition;

        if(Physics.Raycast(currentTurnAgentPosition, lineOfSightDirection.normalized, out hit))
        {
            if(hit.collider.gameObject.GetComponent<Actor>() != targetEnemyActor)
            {
                return false;
            }

            return true;
        }

        return false;
    }
}