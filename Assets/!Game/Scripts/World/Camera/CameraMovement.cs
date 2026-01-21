using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float speed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float[] minMaxDistance = new float[2];
    [Space(10)]
    [Header("Lerps")]
    [SerializeField] private float movementLerp;
    [SerializeField] private float rotLerp;
    [SerializeField] private float zoomLerp;
    [Header("Objective Transforms")]
    [SerializeField] private Transform cameraObjective;
    [SerializeField] private Transform cameraObjective2;
    [SerializeField] private Transform objObjective;
    [Space(10)]
    [Header("Debug")]
    [SerializeField] private bool drawGizmos;
    [SerializeField] private bool verbose;
    
    private float distFromCenter;

    private void Start()
    {
        cameraObjective.position = cameraTransform.position;
        cameraObjective.rotation = cameraTransform.rotation;
        cameraObjective.parent = objObjective;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;
            
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(objObjective.position, 0.25f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(cameraObjective.position, 0.25f);
        Gizmos.DrawLine(cameraObjective.position, cameraObjective.position + (1 * cameraObjective.forward));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(cameraObjective2.position, 0.25f);
        Gizmos.DrawLine(cameraObjective2.position, cameraObjective2.position + (1 * cameraObjective2.forward));
    }

    private void DoMovement()
    {
        Vector2 inputs = Vector2.zero;
        float rotation = 0f;
        float scroll = 0f;

        inputs.x = Input.GetAxis("Vertical");
        inputs.y = Input.GetAxis("Horizontal");

        rotation = Input.GetAxis("Lean");

        scroll = Input.GetAxis ("Mouse ScrollWheel");

        inputs.Normalize();

        objObjective.position += ((inputs.y * speed * Time.deltaTime) * objObjective.right) + ((inputs.x * speed * Time.deltaTime) * objObjective.forward);
        objObjective.rotation = Quaternion.Euler(objObjective.rotation.eulerAngles + new Vector3(0, rotation * rotSpeed * Time.deltaTime, 0));

        transform.position = Vector3.Lerp(transform.position, objObjective.position, movementLerp);
        transform.rotation = Quaternion.Slerp(transform.rotation, objObjective.rotation, rotLerp);

        distFromCenter = distFromCenter + (-scroll * scrollSpeed * Time.deltaTime);

        if (distFromCenter < minMaxDistance[0]/2)
            distFromCenter = minMaxDistance[0]/2;
        else if (distFromCenter > minMaxDistance[2]/2)
            distFromCenter = minMaxDistance[2]/2;

        cameraObjective.position = objObjective.position + (cameraObjective.forward * -distFromCenter);
        cameraObjective2.position = objObjective.position + (cameraObjective2.forward * -distFromCenter);

        if (distFromCenter < minMaxDistance[1]/2)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraObjective.position, zoomLerp);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraObjective.rotation, zoomLerp);
        }
        else
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraObjective2.position, zoomLerp);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraObjective2.rotation, zoomLerp);
        }
    }

    private void Verbose(string log);
    {
        if(verbose)
            Debug.Log("[CameraMovement.cs] " + log);
    }

    private void Update()
    {
        DoMovement();
    }
}
