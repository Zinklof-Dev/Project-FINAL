using UnityEngine;

public class CampSceneOutliner : MonoBehaviour
{
    private void Update()
    {
        FireRay();
    }

    void FireRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
    
        if (Physics.Raycast(ray, out hit))
        {
            OutlinedObject obj = hit.collider.gameObject.GetComponent<OutlinedObject>();

            if(obj == null)
                return;

            obj.isOutlineActive = true;
        }
    }
}
