using UnityEngine;
using ZinklofDev.Utils.MathZ;
using ZinklofDev.ConsoleV2;

public class CameraMover : MonoBehaviour
{
    private bool inMotion = false;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float t;

    private static CameraMover instance;

    [SerializeField] private float moveDuration;

    private void Start()
    {
        instance = this;
    }

    [Command("Moves the Camera to desired location and rotation smoothly over time.")]
    public static void MoveCamera(float targetPositionX, float targetPositionY, float targetPositionZ, float eulerTargetRotationX, float eulerTargetRotationY, float eulerTargetRotationZ)
    {
        if (instance.inMotion)
        {
            Debug.LogWarning("Camera already in motion!");
            return;
        }
        Vector3 targetPosition = new Vector3(targetPositionX, targetPositionY, targetPositionZ);
        Quaternion targetRotation = Quaternion.Euler(new Vector3(eulerTargetRotationX, eulerTargetRotationY, eulerTargetRotationZ));

        instance.inMotion = true;
        instance.targetPosition = targetPosition;
        instance.targetRotation = targetRotation;
    }

    public void Update()
    {
        if(!inMotion)
            return;

        bool moveDone = false;
        t += Time.deltaTime / moveDuration;
        float smoothedT = Mathf.SmoothStep(0, 1, t);

        if (Vectors.SqrDist3f(transform.position, targetPosition) <= 0.01f * 0.01f)
        {
            transform.position = targetPosition;
            transform.rotation = targetRotation;
            moveDone = true;
        }
        else
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothedT);

        if (Quaternion.Angle(transform.rotation, targetRotation) == 0.01f)
        {
            transform.rotation = targetRotation;
        }
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothedT);

        if (moveDone)
        {
            inMotion = false;
            t = 0;
        }
    }
}
