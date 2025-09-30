using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// using ZinklofDev.Utils.MathZ;

public class PlayerInputManager : MonoBehaviour
{
    // Actor Refrences
    [SerializeField] List<Actor> allActors = new List<Actor>();
    [SerializeField] List<Actor> partyMemberActors = new List<Actor>();
    [SerializeField] List<Actor> enemyActors = new List<Actor>();

    // Display Manager Refrence

    [SerializeField] public PlayerInputDisplay display;

    // Path Visaulizer Refrence

    [SerializeField] public LineRenderer lineRenderer;

    // Current Input State

    public enum InputState { SelectingPartyMember, SelectingGoal, Confirming, Inactive };
    public InputState state = InputState.SelectingPartyMember;

    // Current Actor and Goal Index
    public Actor currentSelectedActor = null;
    int goalIndex = 0;
    int prevGoalIndex = 0; // for debgging, just to make sure that the line path doesn't change and recalculate if the path is already the same.

    List<int> visualPath = new List<int>();

    public bool confirmButtonHit = false;

    float debugTick = 0f;

    private void Start()
    {
        lineRenderer.enabled = false;

        allActors = FindObjectsByType<Actor>(FindObjectsSortMode.None).ToList();

        // Ensure that there is only one in the scene

        PlayerInputManager manager = FindFirstObjectByType<PlayerInputManager>();

        if (manager != this)
            Destroy(gameObject);

        foreach (Actor actor in allActors)
        {
            switch (actor.type)
            {
                case Actor.ActorType.Enemy:
                    enemyActors.Add(actor);
                    break;

                case Actor.ActorType.PartyMember:
                    partyMemberActors.Add(actor);
                    break;
            }
        }
    }

    private void Update()
    {
        switch (state)
        {
            case InputState.SelectingPartyMember:

                if(SelectPartyMemberActor())
                {
                    display.SetSelectedNameText(currentSelectedActor.actorName);
                    state = InputState.SelectingGoal;
                }

                break;

            case InputState.SelectingGoal:

                if(SelectingGoal())
                {
                    display.SetSelectedGoalPositionText(goalIndex.ToString());


                    if (Input.GetMouseButtonDown(0))
                        state = InputState.Confirming;

                    if (debugTick < 0.1f)
                    {
                        debugTick += Time.deltaTime;
                        return;
                    }

                    if (goalIndex == prevGoalIndex)
                        return;

                    debugTick = 0;
                    visualPath = PathFinding.AStarPath(currentSelectedActor.agent.currentIndex, goalIndex);
                    visualPath = PathFinding.TrimPath(visualPath);

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

            case InputState.Confirming:

                if (!confirmButtonHit)
                    return;

                confirmButtonHit = false;
                currentSelectedActor.agent.goalIndex = goalIndex;
                currentSelectedActor.agent.StartNavigation();
                state = InputState.Inactive;
                lineRenderer.enabled = false;

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

        foreach(Actor actor in partyMemberActors)
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



    // Cameron pushed an old dll to the project, so I am temp copy / pasting the functions 


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
}
