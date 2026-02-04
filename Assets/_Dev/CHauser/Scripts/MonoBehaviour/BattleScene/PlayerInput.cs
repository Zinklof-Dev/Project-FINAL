using System.Collections.Generic;
using UnityEngine;
using ZinklofDev.Utils.MathZ;


public class PlayerInput : MonoBehaviour
{ 
	public static PlayerInput instance;

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

    [SerializeField] LineRenderer pathVisualizer;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if(TurnManager.instance.turnState != TurnManager.TurnState.PlayerTurn || inputState != InputState.Move)
        {
            pathVisualizer.gameObject.SetActive(false);
        }
    }

    public void PlayerTurn()
    {
        switch(inputState)
        {
            case InputState.Move:
                HandleMoveInput();
                break;

            case InputState.Attack:
                // Handle player attack input
                break;

            case InputState.EnemyTurn:

                break;
        }
    }

    public void HandleMoveInput()
    {
        switch(moveInput)
        {
            case MoveInput.SelectingGoal:
                SelectingGoal();
                break;
            case MoveInput.ConfirmingMove:
                // Handle confirming move logic
                break;
            case MoveInput.Moving:
                // Handle moving logic
                break;
        }
    }

    int tentativeGoalIndex = -1;
    int prevTentativeGoalIndex;

    public void SelectingGoal()
    {
        pathVisualizer.gameObject.SetActive(true);

        RaycastHit hit;

        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Vector2 closestPoint = new Vector2(Numbers.RoundToMultiple(hit.point.x, GridSystem.instance.tileSize, GridSystem.instance.offsetX), Numbers.RoundToMultiple(hit.point.z, GridSystem.instance.tileSize, GridSystem.instance.offsetY));
            int hitIndex = GridSystem.instance.points.IndexOf(closestPoint);

            //Debug.Log("Closest Point: " + closestPoint + " | Hit Index: " + hitIndex);

            if (hitIndex != -1)
            {
                tentativeGoalIndex = hitIndex;
            }

            else
                return;
        }

        if(prevTentativeGoalIndex == tentativeGoalIndex)
            return;

        prevTentativeGoalIndex = tentativeGoalIndex;

        List<int> visualPath = PathFinding.AStarPath(TurnManager.instance.currentTurnAgent.currentIndex, tentativeGoalIndex, 1);

        if (visualPath == null)
        {
            pathVisualizer.positionCount = 0;
            Debug.Log("No Path Found");
            return;
        }

        visualPath = PathFinding.TrimPath(visualPath);

        pathVisualizer.positionCount = visualPath.Count;

        foreach (int index in visualPath)
        {
            pathVisualizer.SetPosition(visualPath.IndexOf(index), new Vector3(GridSystem.instance.points[index].x, 0.1f, GridSystem.instance.points[index].y));
        }
    }

    public void ConfirmingMove()
    {

    }

    public void MovingGoal()
    {

    }

}
