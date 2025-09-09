using UnityEngine;

public class GridDweller : MonoBehaviour
{
    [SerializeField] GridSystem grid;
    [SerializeField] public int positionIndex;
    [SerializeField] Vector2 position;
    [SerializeField] public bool registerWithCommand;

    private void Start()
    {
        if(registerWithCommand)
            GridSystem.dwellers.Add(this);
    }

    private void Update()
    {
        positionIndex = Mathf.Clamp(positionIndex, 0, GridSystem.points.Count - 1);
        position = GridSystem.points[positionIndex];
        transform.position = new Vector3(position.x, 0, position.y);
    }
}
