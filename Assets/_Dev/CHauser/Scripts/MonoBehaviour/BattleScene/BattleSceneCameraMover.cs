using UnityEngine;

public class BattleSceneCameraMover : MonoBehaviour
{
    // Basic for now, will refine later

    [SerializeField] private float moveSpeed = 10f;

    private void Update()
    {
        if(Input.GetKey(KeyCode.W) && !(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.S) && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            transform.position += Vector3.back * moveSpeed * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.A) && !(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D)))
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D) && !(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W)))
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }
}
