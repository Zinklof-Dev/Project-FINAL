using UnityEngine;

public class CampScenePlayerInputManager : MonoBehaviour
{
    [Header("Clickable Enviorment Refrences")]
    [SerializeField] private GameObject mapTable;
    [SerializeField] private GameObject playerEditor;

    [Header("UI Refrences")]
    [SerializeField] private GameObject mapTableUI;
    [SerializeField] private GameObject playerEditorUI;

    [Header("Camera Positions")]
    [SerializeField] private Vector3 cameraDefaultPosition = new Vector3(0, 5, -4);
    [SerializeField] private Vector3 cameraMapTablePosition = new Vector3(2, 3, 2);
    [SerializeField] private Vector3 cameraPlayerEditorPosition = new Vector3(-3, 3, 2);

    [Header("Camera Euler Rotations")]
    [SerializeField] private Vector3 cameraDefaultEulerRotation = new Vector3(15, 0, 0);

    public enum State { Default, MapTable, PlayerEditor }

    private State state = State.Default;

    private void Update()
    {
        switch(state)
        {
            case(State.Default):
                HandleDefaultStateInput();
                break;
            case State.MapTable:
                if(CameraMover.inMotion || mapTableUI.activeInHierarchy)
                    return;
                mapTableUI.SetActive(true);
                break;
            case State.PlayerEditor:
                if (CameraMover.inMotion || playerEditorUI.activeInHierarchy)
                    return;
                playerEditorUI.SetActive(true);
                break;
        }
    }

    private void HandleDefaultStateInput()
    {
        if (CameraMover.inMotion)
            return;

        if (!Input.GetMouseButtonDown(0))
            return;

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            return;

        if (hit.collider.gameObject == mapTable)
        {

            Vector3 directionToTarget = hit.transform.position - cameraMapTablePosition;
            Quaternion goalRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            Vector3 eulerGoalRotation = goalRotation.eulerAngles;

            CameraMover.MoveCamera(cameraMapTablePosition.x, cameraMapTablePosition.y, cameraMapTablePosition.z, eulerGoalRotation.x, eulerGoalRotation.y, eulerGoalRotation.z);

            state = State.MapTable;
        }
        else if (hit.collider.gameObject == playerEditor)
        {

            Vector3 directionToTarget = hit.transform.position - cameraPlayerEditorPosition;
            Quaternion goalRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            Vector3 eulerGoalRotation = goalRotation.eulerAngles;

            CameraMover.MoveCamera(cameraPlayerEditorPosition.x, cameraPlayerEditorPosition.y, cameraPlayerEditorPosition.z, eulerGoalRotation.x, eulerGoalRotation.y, eulerGoalRotation.z);

            state = State.PlayerEditor;
        }
    }

    public void BackToDefault()
    {
        state = State.Default;
        CameraMover.MoveCamera(cameraDefaultPosition.x, cameraDefaultPosition.y, cameraDefaultPosition.z, cameraDefaultEulerRotation.x, cameraDefaultEulerRotation.y, cameraDefaultEulerRotation.z);
        mapTableUI.SetActive(false);
        playerEditorUI.SetActive(false);
    }
}
