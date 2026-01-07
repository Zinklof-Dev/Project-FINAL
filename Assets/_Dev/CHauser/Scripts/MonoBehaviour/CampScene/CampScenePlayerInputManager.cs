using UnityEngine;

public class CampScenePlayerInputManager : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private GameObject mapTable;

    [Header("Camera Positions")]
    [SerializeField] private Vector3 cameraDefaultPosition = new Vector3(0, 5, -4);
    [SerializeField] private Vector3 cameraMapTablePosition = new Vector3(2, 3, 2);

    [Header("Camera Euler Rotations")]
    [SerializeField] private Vector3 cameraDefaultEulerRotation = new Vector3(15, 0, 0);

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if(!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            return;

        if(hit.collider.gameObject == mapTable)
        {

            Vector3 directionToTarget = hit.transform.position - cameraMapTablePosition;
            Quaternion goalRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            Vector3 eulerGoalRotation = Quaternion.Euler(goalRotation);

            CameraMover.MoveCamera(cameraMapTablePosition.x, cameraMapTablePosition.y, cameraMapTablePosition.z, hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
        }
    }
}
