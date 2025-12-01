using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZinklofDev.Utils.MathZ;

public class PlayerInputManager : MonoBehaviour
{
    #region Display Manager Refrence
    // Display Manager Refrence

    [SerializeField] public PlayerInputDisplay display;

    #endregion

    #region Path Visualizer Refrences

    // Path Visualizer Refrence

    [SerializeField] public LineRenderer lineRenderer;
    [SerializeField] public Material lineRenedererMaterial;

    #endregion

    #region Input State

    // Current Input State

    public enum InputState { SelectingPartyMember, SelectingGoal, ConfirmingNavigation, Inactive, SelectingEnemyToAttack, ConfirmEnemy, EnemyTurn };
    public InputState state = InputState.SelectingPartyMember;

    #endregion

    #region Current Selections

    // Current Actor and Goal Index
    public Actor currentSelectedActor = null;
    public Actor currentEnemySelectedActor = null;
    int goalIndex = 0;
    int prevGoalIndex = 0; // for debgging, just to make sure that the line path doesn't change and recalculate if the path is already the same.

    #endregion

    #region Turn Manager Refrence

    public TurnManager turnManager;

    #endregion

    List<int> visualPath = new List<int>();

    public bool confirmButtonHit = false;
    public bool enemyConfirmButtonHit = false;

    float debugTick = 1f;

    public int actionPointCost = 0;

    private void Start()
    {
        // Ensure that there is only one in the scene
        PlayerInputManager manager = FindFirstObjectByType<PlayerInputManager>();
        if (manager != this)
            Destroy(gameObject);

        lineRenderer.enabled = false;

        turnManager = FindFirstObjectByType<TurnManager>();
    }

    private void Update()
    {
        switch (state)
        {
            case InputState.SelectingPartyMember:

                if(SelectPartyMemberActor())
                {
                    display.SetSelectedNameText(currentSelectedActor.actorName);
                    display.PromtIfAttackingOrNavigating();
                    state = InputState.Inactive;
                }

                break;

            case InputState.SelectingGoal:

                bool goalSelected = SelectingGoal();

                if (goalIndex == currentSelectedActor.agent.currentIndex)
                {
                    return;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    state = InputState.ConfirmingNavigation;
                    display.ConfirmPathPrompt();
                    debugTick = 1f;
                }

                if (debugTick < 0.1f)
                {
                    debugTick += Time.deltaTime;
                    return;
                }

                if (goalSelected)
                {
                    if (goalIndex == prevGoalIndex)
                        return;

                    debugTick = 0;

                    visualPath = PathFinding.AStarPath(currentSelectedActor.agent.currentIndex, goalIndex, 1);

                    if (visualPath == null)
                    {
                        goalIndex = prevGoalIndex;
                        return;
                    }

                    actionPointCost = visualPath.Count - 1;

                    if (actionPointCost > turnManager.playerActionPoints)
                    {
                        goalIndex = prevGoalIndex;
                        return;
                    }

                    display.SetAPCostText(visualPath.Count - 1);

                    visualPath = PathFinding.TrimPath(visualPath);

                    display.SetSelectedGoalPositionText(goalIndex.ToString());

                    lineRenderer.enabled = true;
                    lineRenderer.positionCount = visualPath.Count;
                    int i = 0;

                    foreach (int point in visualPath)
                    {
                        lineRenderer.SetPosition(i, new Vector3(GridSystem.points[point].x, 0.1f, GridSystem.points[point].y));
                        i++;
                    }
                }

                prevGoalIndex = goalIndex;

                break;

            case InputState.ConfirmingNavigation:

                if (!confirmButtonHit)
                    return;

                confirmButtonHit = false;
                currentSelectedActor.agent.goalIndex = goalIndex;
                currentSelectedActor.agent.StartNavigation();
                state = InputState.Inactive;
                lineRenderer.enabled = false;

                break;

            case InputState.SelectingEnemyToAttack:

                if(SelectEnemyActor())
                {
                    List<int> pathToEnemy = PathFinding.AStarPath(currentSelectedActor.agent.currentIndex, currentEnemySelectedActor.agent.currentIndex, 2);

                    if(pathToEnemy.Count - 1 <= currentSelectedActor.range)
                    {
                        display.SetSelectedEnemyText(currentEnemySelectedActor.name);
                        enemyConfirmButtonHit = false;
                        state = InputState.ConfirmEnemy;
                        display.ConfirmEnemyPrompt();
                        return;
                    }

                    currentEnemySelectedActor = null;
                }

                break;

            case InputState.ConfirmEnemy:
                
                if(!enemyConfirmButtonHit)
                    return;
                
                // Temp, currently just clears and resets system. Will eventually add another set of states for battle here
                state = InputState.SelectingPartyMember;
                turnManager.TakeAction(1);
                display.Clear(false);

                break;

            case InputState.Inactive:

                break;
        }
    }

    bool SelectPartyMemberActor()
    {
        if(!Input.GetMouseButtonDown(0)) 
            return false;

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            return false;

        float x = /*Numbers.*/RoundToMultiple(hit.point.x, GridSystem.staticTileSize, GridSystem.staticOffsetX);
        float y = /*Numbers.*/RoundToMultiple(hit.point.z, GridSystem.staticTileSize, GridSystem.staticOffsetY);

        Vector2 selectedPosition = new Vector2(x, y);

        bool actorFound = false;

        foreach(Actor actor in ActorManager.partyMemberActors)
        {
            if(actor.agent.currentIndex == GridSystem.points.IndexOf(selectedPosition))
            {
                currentSelectedActor = actor;
                actorFound = true;
                break;
            }
        }

        return actorFound;
    }

    bool SelectEnemyActor()
    {
        if (!Input.GetMouseButtonDown(0))
            return false;

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            return false;

        float x = /*Numbers.*/RoundToMultiple(hit.point.x, GridSystem.staticTileSize, GridSystem.staticOffsetX);
        float y = /*Numbers.*/RoundToMultiple(hit.point.z, GridSystem.staticTileSize, GridSystem.staticOffsetY);

        Vector2 selectedPosition = new Vector2(x, y);

        bool actorFound = false;

        foreach (Actor actor in ActorManager.enemyActors)
        {
            if (actor.agent.currentIndex == GridSystem.points.IndexOf(selectedPosition))
            {
                currentEnemySelectedActor = actor;
                actorFound = true;
                break;
            }
        }
        return actorFound;
    }

    bool SelectingGoal()
    {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            return false;

        float x = /*Numbers.*/RoundToMultiple(hit.point.x, GridSystem.staticTileSize, GridSystem.staticOffsetX);
        float y = /*Numbers.*/RoundToMultiple(hit.point.z, GridSystem.staticTileSize, GridSystem.staticOffsetY);

        Vector2 selectedPosition = new Vector2(x, y);

        bool indexFound = false;

        foreach(Vector2 position in GridSystem.points)
        {
            if(selectedPosition == position)
            {
                indexFound = true;
                goalIndex = GridSystem.points.IndexOf(position);
            }
        }

        return indexFound;
    }


    #region TEMP DLL FUNCTIONS

    // Cameron pushed an old dll to the project, so I am temp copy / pasting the functions - Cole


    /// <summary>
    /// Authored: Dgoyette, Cole Hauser
    /// Rounds to the nearest multiple
    /// </summary>
    /// <param name="inputValue">The Number to round</param>
    /// <param name="baseNumberOfMultiple">Multiple to round to</param>
    /// <returns>InputValue rounded to the nearest occurance of the Multiple as a float.</returns>
    static public float RoundToMultiple(float inputValue, float baseNumberOfMultiple)
    {
        return Mathf.Round(inputValue / baseNumberOfMultiple) * baseNumberOfMultiple;
    }
    /// <summary>
    /// Authored: Bunny83, Dgoyette, Cole Hauser
    /// Override made by Bunny83 that allows use of tOffset when rounding.
    /// </summary>
    /// <param name="inputValue">The Number to round</param>
    /// <param name="baseNumberOfMultiple">Multiple to round to</param>
    /// <param name="tOffset">I dunno... its an offset? but how does it apply what does it do!? Cole doesn't even remember</param>
    /// <returns>InputValue rounded to the nearest occurance of the Multiple as a float. Using tOffset to offset the result.</returns>
    static float RoundToMultiple(float inputValue, float baseNumberOfMultiple, float tOffset)
    {
        return Mathf.Round((inputValue - tOffset) / baseNumberOfMultiple) * baseNumberOfMultiple + tOffset;
    }
    #endregion
}
