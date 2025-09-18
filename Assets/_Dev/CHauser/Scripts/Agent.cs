using UnityEngine;

public class Agent : MonoBehaviour
{
  private enum State { Idle, Moving }
  [Header("Agent State")]
  [SerializeField] private State currentState = State.Idle;
  [Header("Indicies Of The Grid")]
  [SerializeField] private int startIndex;
  [SerializeField] private int goalIndex;
  [SerializeField] private int currentIndex;
  [SerializeField] private int nextIndex;
  private List<int> path = new List<int>();
  private int step = 1;

  private void OnDrawGizmos()
  {
    if(!currentState == State.Moving)
      return;
    
  }

  private void Start()
  {
    
  }
  
  private void Update()
  {
    switch (currentState)
    {
      case State.Idle:
        transform.position = 
        break;
        
      case State.Moving:
        Move();
        break;
    }
  }

  /*private*/ public void StartNavigation()
  {
    path = PathFinding.AStarPath(startIndex, goalIndex);
    if(path == null)
    {
      Debug.Log("Can't Move! No path found.");
      return;
    }
    currentState = State.Moving;
    currentIndex = startIndex;
    step = 1;
  }

  private void Move()
  {
    nextIndex = path[step];
    Vector3 start = new Vector3(GridSystem.points[currentIndex].x, transform.position.y, GridSystem.points[currentIndex].y);
    
  }
}
