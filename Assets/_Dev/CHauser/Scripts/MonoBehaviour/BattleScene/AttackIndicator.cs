using UnityEngine;

public class AttackIndicator : MonoBehaviour
{
    Actor parent;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        parent = GetComponentInParent<Actor>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (PlayerInput.instance.targetEnemyActor == parent && PlayerInput.instance.attackInput == PlayerInput.AttackInput.Selecting && PlayerInput.instance.inputState == PlayerInput.InputState.Attack && TurnManager.instance.turnState == TurnManager.TurnState.PlayerTurn)
        {
            if (spriteRenderer.color.a != 255)
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 255);
        }

        else
        {
            if (spriteRenderer.color.a != 0)
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        }
    }
}
