using UnityEngine;

public class Agent : MonoBehaviour
{
    public PlayableCharacter playableCharacter;

    public int actionPoints;
    public int currentIndex;
    public int goalIndex;

    public enum AgentState
    {
        Idle,
        Moving,
        Attacking,
        Defending
    }

    public AgentState state = AgentState.Idle;

    private void Update()
    {
        switch (state)
        {
            case AgentState.Idle:
                IdleBehavior();
                break;

            case AgentState.Moving:
                // Moving behavior
                break;

            case AgentState.Attacking:
                // Attacking behavior
                break;

            case AgentState.Defending:
                // Defending behavior
                break;
        }
    }

    private void IdleBehavior()
    {
        transform.position = new Vector3(GridSystem.instance.points[currentIndex].x, 0, GridSystem.instance.points[currentIndex].y);
    }
}
