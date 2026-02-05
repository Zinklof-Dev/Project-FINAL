using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Transform cameraTransform;
    float fullWidth;

    [SerializeField] Actor actor;
    [SerializeField] SpriteRenderer spriteRenderer;


    private void Start()
    {
        cameraTransform = GameObject.FindWithTag("MainCamera").transform;

        fullWidth = spriteRenderer.size.x;
        //Debug.Log("fullWidth: " + fullWidth);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraTransform);

        //Debug.Log("Health: " + (actor.health / actor.maxHealth));
        spriteRenderer.size = new Vector2(fullWidth * (actor.health / actor.maxHealth), spriteRenderer.size.y);
    }
}
