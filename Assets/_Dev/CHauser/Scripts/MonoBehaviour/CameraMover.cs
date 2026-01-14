using UnityEngine;
using ZinklofDev.Utils.MathZ;
using ZinklofDev.ConsoleV2;

public class CameraMover : MonoBehaviour
{
    private bool inMotion = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    private float t;

    private static CameraMover instance;

    [SerializeField] private float moveDuration;
    //[SerializeField] private AnimationCurve moveCurve;

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
        instance.startPosition = instance.transform.position;
        instance.startRotation = instance.transform.rotation;
        instance.t = 0;
    }

    public void Update()
    {
        if(!inMotion)
            return;

        t += Time.deltaTime / moveDuration;

        if (t >= 1)
        {
            transform.position = targetPosition;
            transform.rotation = targetRotation;
            t = 0;
            inMotion = false;
            return;
        }

        float smoothedT = Mathf.SmoothStep(0, 1, t);
        // smoothedT = moveCurve.Evaluate(t);

        transform.position = Vector3.Lerp(startPosition, targetPosition, smoothedT);
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, smoothedT);
    }
}
