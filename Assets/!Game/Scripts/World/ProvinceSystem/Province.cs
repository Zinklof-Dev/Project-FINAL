/*
using UnityEngine;

public class Province : MonoBehaviour
{
    private byte ID;
    [SerializeField] private string name;
    [SerializeField] private byte type; // plains / forest / crossing / ocean
    [SerializeField] private bool evil = false;

    [SerializeField] private vector3 centerOffset;

    [SerializeField] bool drawGizmos;

    private Renderer renderer;
    private bool selected;
    private ProvinceManager pm;

    private Color selectedColor;
    private Color unselectedColor;

    public void Start()
    {
        ID = pm.RegisterProvince();
    
        pm = GameObject.FindGameObjectWithTag("Map_ProvinceManager").GetComponent<ProvinceManager>();

        unslectedColor = pm.GetUnselectedColor(evil);
        selectedColor = pm.GetSelectedColor(type);

        renderer = gameObject.GetComponent<Renderer>();

        selected = false;
    }

    private var[] GetInfo()
    {
        var[] returnInfo = new var[5];

        returnInfo[0] = ID;
        returnInfo[1] = type;
        returnInfo[2] = name;
        returnInfo[3] = centerOffset;
        returnInfo[4] = evil;

        return returnInfo;
    }

    public void ToggleSide()
    {
        evil = !evil;

        unslectedColor = pm.GetUnselectedColor(evil);
    }

    public void ToggleState()
    {
        selected = !selected;

        if (selected)
            renderer.material.SetColor("_Color", selectedColor);
        else
            renderer.material.SetColor("_Color", unselectedColor);
    }
}
*/
