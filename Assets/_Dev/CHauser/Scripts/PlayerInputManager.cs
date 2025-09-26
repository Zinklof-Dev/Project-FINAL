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

    // Current Input State

    enum InputState { SelectingPartyMember, SelectingGoal, Confirming, Inactive };
    InputState state = InputState.SelectingPartyMember;

    // Current Actor and Goal Index
    Actor currentSelectedActor = null;
    int goalIndex = 0;

    private void Start()
    {
        allActors = FindObjectsByType<Actor>(FindObjectsSortMode.None).ToList();

        // Ensure that there is nly one in the scene

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
                SelectPartyMemberActor();
                break;

            case InputState.SelectingGoal:
                break;

            case InputState.Confirming:
                break;

            case InputState.Inactive:
                break;
        }
    }

    void SelectPartyMemberActor()
    {
        if(!Input.GetMouseButton(0)) 
            return;

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            return;

        float x = /*Numbers.*/RoundToMultiple(hit.point.x, GridSystem.staticTileSize, GridSystem.staticOffsetX);
        float y = /*Numbers.*/RoundToMultiple(hit.point.z, GridSystem.staticTileSize, GridSystem.staticOffsetY);

        Vector2 selectedPosition = new Vector2(x, y);

        foreach(Actor actor in partyMemberActors)
        {
            //if(actor.agent.currentIndex == )
        }
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
