using UnityEngine;
using ZinklofDev.Utils.MathZ;
using ZinklofDev.ConsoleV2;

public class CameraMover : MonoBehaviour
{
    private bool inMotion = false;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private static CameraMover instance;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

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

        bool rotDone = false;
        bool moveDone = false;

        if (Vectors.SqrDist3f(transform.position, targetPosition) < 0.1f)
        {
            transform.position = targetPosition;
            moveDone = true;
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            transform.rotation = targetRotation;
            rotDone = true;
        }
        else
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (rotDone && moveDone)
            inMotion = false;
    }
}
