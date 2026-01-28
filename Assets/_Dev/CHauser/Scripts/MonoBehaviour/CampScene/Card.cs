using UnityEngine;

public class Card : MonoBehaviour
{
    public enum ItemType { Null, Sword };
    [SerializeField] public ItemType itemType;
}
