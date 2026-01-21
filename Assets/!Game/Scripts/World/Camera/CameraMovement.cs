using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraObjective;
    [SerializeField] private Transform cameraObjective2;
    [SerializeField] private Transform objObjective;
    [Space(10)]
    [Header("Movement Variables")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float speed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float shiftMult;
    [SerializeField] private float ctrlMult;
    [SerializeField] private float[] minMaxDistance = new float[2];
    [SerializeField] private AnimationCurve scrollCurve;
    [Space(5)]
    [Header("Zoom To Object Variables")]
    [SerializeField] private float zoomDist;
    [SerializeField] private bool copyRotation;
    [Space(10)]
    [Header("Lerp Variables")]
    [SerializeField] private float movementLerp;
    [SerializeField] private float rotLerp;
    [SerializeField] private float zoomLerp;
    [Space(10)]
    [Header("Debug")]
    [SerializeField] private bool drawLerps;
    [SerializeField] private bool verbose;
    [SerializeField] private bool drawHits;

    private float distFromCenter;

    private list<Vector3> hits = new list<Vector3>();
    private Vector3 ?latestHit = null;

    private void Start()
    {
        cameraObjective.position = cameraTransform.position;
        cameraObjective.rotation = cameraTransform.rotation;
        cameraObjective.parent = objObjective;

        latestHit = null;
    }

    private void OnDrawGizmos()
    {
        if (drawLerps)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(objObjective.position, 0.25f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(cameraObjective.position, 0.25f);
            Gizmos.DrawLine(cameraObjective.position, cameraObjective.position + (1 * cameraObjective.forward));
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(cameraObjective2.position, 0.25f);
            Gizmos.DrawLine(cameraObjective2.position, cameraObjective2.position + (1 * cameraObjective2.forward));
        }

        if (drawHits)
        {
            Gizmos.color = Color.red;
            foreach (Vector3 pos in hits)
            {
                Gizmos.drawSphere(pos, 0.25f)
            }

            Gizmos.color = Color.blue;
            if (latestHit != null)
                Gizmos.drawSphere(latestHit, 0.3f);
        }
    }

    private void DoMovement()
    {
        float mult = (shiftMult * Input.GetAxis("Sprint") + (ctrlMult * Input.GetAxis("Crouch")));
        if (mult == 0)
            mult = 1;   

        Vector2 inputs = Vector2.zero;
        float rotation = 0f;
        float scroll = 0f;

        inputs.x = Input.GetAxis("Vertical");
        inputs.y = Input.GetAxis("Horizontal");

        rotation = Input.GetAxis("Lean");

        scroll = Input.GetAxis("Mouse ScrollWheel");

        inputs.Normalize();

        objObjective.position += ((inputs.y * speed * mult * Time.deltaTime) * objObjective.right) + ((inputs.x * speed * Time.deltaTime) * objObjective.forward);
        objObjective.rotation = Quaternion.Euler(objObjective.rotation.eulerAngles + new Vector3(0, rotation * rotSpeed * mult * Time.deltaTime, 0));

        transform.position = Vector3.Lerp(transform.position, objObjective.position, movementLerp);
        transform.rotation = Quaternion.Slerp(transform.rotation, objObjective.rotation, rotLerp);

        distFromCenter = distFromCenter + (-scroll * scrollSpeed * mult * scrollCurve.Evaluate(distFromCenter / minMaxDistance[2]) * Time.deltaTime);

        if (distFromCenter < minMaxDistance[0] / 2)
            distFromCenter = minMaxDistance[0] / 2;
        else if (distFromCenter > minMaxDistance[2] / 2)
            distFromCenter = minMaxDistance[2] / 2;

        cameraObjective.position = objObjective.position + (cameraObjective.forward * -distFromCenter);
        cameraObjective2.position = objObjective.position + (cameraObjective2.forward * -distFromCenter);

        if (distFromCenter < minMaxDistance[1] / 2)
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

    private void HandleMouseInput()
    {
        gameobject hitObject = null;
    
        if (Input.GetAxis("Fire") > 0)
        {
            hitobject = FireRay();
        }

        if (hitObject == null)
            return;

        AttributeSystem objAttributes = hitObject.GetComponent<AttributeSystem>();
            
        if (objAttributes.GetAttribute("Map_Zoomable"))
        {
            objObjective.position = hitObject.transform.position;
            distFromCenter = zoomDist;

            if (copyRotation)
                objObjective.rotation = hitObject.transform.rotation;
        }
        if (objAttributes.GetAttribute("Map_Party"))
        {
        
        }
    }

    private gameobject FireRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 250f)
        {   
            if (!drawHits)
                return hit.collider.gameobject;
            
            if (latestHit != null)
                hits.add(latestHit);
            latestHit = hit.point;

            return hit.collider.gameobject;
        }
    }

    private void Verbose(string log)
    {
        if(verbose)
            Debug.Log("[CameraMovement.cs] " + log);
    }

    private void Update()
    {
        DoMovement();
        HandleMouseInput();
    }
}
