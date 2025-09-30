using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputDisplay : MonoBehaviour
{
    PlayerInputManager playerInputManager;

    [SerializeField] TMP_Text selectedActorText;
    [SerializeField] TMP_Text selectedGoalPositionText;

    private void Start()
    {
        playerInputManager = FindFirstObjectByType<PlayerInputManager>();
    }

    public void SetSelectedNameText(string name)
    {
        selectedActorText.text = name;
    }

    public void SetSelectedGoalPositionText(string position)
    {
        selectedGoalPositionText.text = position;
    }

    public void Confirm()
    {
        if (playerInputManager.state == PlayerInputManager.InputState.Confirming)
        {
            playerInputManager.confirmButtonHit = true;
        }
    }
}
