using UnityEngine;

public class LookAt : MonoBehaviour
{
    Transform cameraTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraTransform = cameraTransform = GameObject.FindWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraTransform);
    }
}
