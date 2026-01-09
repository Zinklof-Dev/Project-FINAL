using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float speed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float[] minMaxDistance = new float[2];

    private float distanceFromCenter;

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

        transform.position += ((inputs.y * speed * Time.deltaTime) * transform.right) + ((inputs.x * speed * Time.deltaTime) * transform.forward);
        transform.rotation *= Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, rotation * rotSpeed * Time.deltaTime, 0));

        //cameraTransform.localPosition.z
    }
}
