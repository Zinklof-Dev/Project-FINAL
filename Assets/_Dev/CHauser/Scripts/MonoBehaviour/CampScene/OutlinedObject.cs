using UnityEngine;

public class OutlinedObject : MonoBehaviour
{
    public bool isOutlineActive = false;
    Material m;

    private void Start()
    {
        m = GetComponent<MeshRenderer>().materials[0];
    }

    void Update()
    {
        if(isOutlineActive)
        {
           m.SetColor("_Outline_Color", Color.yellow);
        }
        else
        {
            m.SetColor("_Outline_Color", Color.black);
        }

        isOutlineActive = false;
    }
}
