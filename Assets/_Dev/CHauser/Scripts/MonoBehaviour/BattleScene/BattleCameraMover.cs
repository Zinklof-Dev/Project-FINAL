using UnityEngine;

public class BattleCameraMover : MonoBehaviour
{
    [SerializeField] public Transform moveSelectionPositn;
    [SerializeField] private GameObject cameraGameObject;
    [SerializeField] private float moveDuration;

    private float t = 0;
    public bool moving = false;
    private Vector3 startPosition = Vector3.zero;
    private Quaternion startRotation = Quaternion.identity;

    public static BattleCameraMover instance;

    private float clampedXMin = 0;
    private float clampedZMin = 0;
    private float clampedXMax = 0;
    private float clampedZMax = 0;

    private void Start()
    {
        instance = this; 
        MoveCameraToMoveSelectionPosition();
    }

    private void Update()
    {
        if (moving)
            Move();
    }

    public void MoveCameraToMoveSelectionPosition()
    {
        moving = true;
        cameraGameObject.transform.parent = moveSelectionPositn;
        startPosition = cameraGameObject.transform.localPosition;
        startRotation = cameraGameObject.transform.localRotation;
        t = 0;
    }

    public void MoveCameraTo(Transform parent)
    {
        moving = true;
        cameraGameObject.transform.parent = parent;
        startPosition = cameraGameObject.transform.localPosition;
        startRotation = cameraGameObject.transform.localRotation;
        t = 0;
    }

    private void Move()
    {
        if (t >= 1)
        {
            moving = false;
            return;
        }

        t += Time.deltaTime / moveDuration;
        float smoothedT = Mathf.SmoothStep(0, 1, t);

        cameraGameObject.transform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, smoothedT);
        cameraGameObject.transform.localRotation = Quaternion.Slerp(startRotation, Quaternion.identity, smoothedT);
    }
}
