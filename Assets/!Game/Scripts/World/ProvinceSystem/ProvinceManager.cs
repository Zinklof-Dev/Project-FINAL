/*
using UnityEngine;
using System;
using System.Collections.Generic;

public class ProvinceManager : MonoBehaviour
{
    [SerializeField] public List<Province> Provinces = new List<Province>();
    [SerializeField] List<Color> colors = new List<Color>(); // fallback, selected land, selected evil, unselected land, unselected evil, crossing, ocean, selected misc    
    byte nextID;

    static private ProvinceManager pm;

    private void Start()
    {
        pm = this.ProvinceManager;
    }

    public byte RegisterProvince(Province p)
    {
        Provinces.add(p);

        return nextID++;
    }

    public Color GetUnselectedColor(bool evil, byte type)
    {
        if (type = 3)
            return colors(5);
        else if (type == 4)
            return colors(6);
        else if (evil)
            return colors(4);
        else if (!evil)
            return colors(3);
        else
            return colors(0);
    }

    public Color GetSelectedColor(bool evil, byte type)
    {
        if (type == 3 || type == 4)
            return colors(7);
        else if (evil)
            return colors(2);
        else if (!evil)
            return colors(1);
        else
            return colors(0);
    }

    [Command("Switches a provinces side on the map")]
    static private ToggleSide(byte id)
    {
        if (pm == null)
        {
            //log
            return;
        }
        
        pm.Provinces(id).ToggleSide();
    }
}
*/
