using System.Data;
using UnityEngine;
using Bastion.Utils.MathZ;

public class CameraTrack : MonoBehaviour
{
    [SerializeField] Vector3[] positions = new Vector3[4];
    [SerializeField] float slerp;
    [SerializeField] float moveSpeed;

    int cur = 0;

    private void OnDrawGizmos()
    {
        Vector3 lastPosition = new Vector3(0,3,-10);

        Gizmos.color = Color.green;
        foreach (Vector3 position in positions)
        {
            Gizmos.DrawWireSphere(position, 0.25f);
            Gizmos.DrawLine(position, lastPosition);
            lastPosition = position;
        }
    }

    private void Update()
    {
        if (Vectors.SqrDist3f(transform.position, positions[cur]) < 0.5f * 0.5f)
        {
            cur++;

            if (cur >= positions.Length)
            {
                Destroy(this);
                return;
            }
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(positions[cur] - transform.position), slerp);

        transform.position = transform.position + ((positions[cur] - transform.position).normalized * moveSpeed) * Time.deltaTime;
    }
}
