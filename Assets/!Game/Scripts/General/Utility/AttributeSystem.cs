using UnityEngine;


// this exists souly because i wanted to have more than one tag without touching layers because i'd rather save layer slots for more important functions.
public class AttributeSystem : MonoBehaviour
{
    public string[] attributes = new string[1];

    public bool GetAttribute(string attributeName)
    {
        foreach (string attribute in attributes)
        {
            if (attribute == attributeName)
                return true;
        }

        return false;
    }

    // was gonna make a get attribute func but thats redundant since you can just fetch the string variable
}
