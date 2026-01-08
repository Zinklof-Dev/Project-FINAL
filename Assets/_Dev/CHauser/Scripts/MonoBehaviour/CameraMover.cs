using UnityEngine;
using ZinklofDev.Utils.MathZ;
using ZinklofDev.ConsoleV2;

public class CameraMover : MonoBehaviour
{
    private bool inMotion = false;
    private float moveSpeed;
    private Vector3 targetPosition;
    private float rotateSpeed;
    private Quaternion targetRotation;
    private static CameraMover instance;

    [SerializeField] private float moveRate;

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
        instance.moveSpeed = Vector3.Distance(instance.transform.position, targetPosition) / instance.moveRate;
        instance.rotateSpeed = Quaternion.Angle(instance.transform.rotation, targetRotation) / instance.moveRate;
    }

    public void Update()
    {
        if(!inMotion)
            return;

        bool moveDone = false;

        if (Vectors.SqrDist3f(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            transform.rotation = targetRotation;
            moveDone = true;
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.01f)
        {
            transform.rotation = targetRotation;
        }
        else
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        if (moveDone)
            inMotion = false;
    }
}
