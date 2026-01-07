using TMPro;
using UnityEngine;

public class Info : MonoBehaviour
{
    public Actor assignedActor = null;
    [SerializeField] public TMP_Text name = null;
    [SerializeField] public TMP_Text attackPower = null;
    [SerializeField] public TMP_Text range = null;
    [SerializeField] public TMP_Text health = null;
}
