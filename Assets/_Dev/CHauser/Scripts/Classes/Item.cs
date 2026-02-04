using UnityEngine;

// Going to rename class and file, goingto do so tomorrow in Flower's though bc of compiler and .meta errors.
// Going to delete "Weapon" class or maybe scrap "Tool" and move it to there, still need to decide though.

public class Item // Tool // Tool is new name for this class and file
{
    // Needed maybe
    public enum Type { Axe, Sword, Default }
    private Type type;
    // Following going to stay unused probably for EP
   // private int id;
    //private string name;
    //private string description;
    // These will be needed essential variables
    private float attackPower; 
    private float reach;

    public Item(Type type, float attackPower, float reach)
    {
        this.type = type;
        this.attackPower = attackPower;
        this.reach = reach;
        // TEMP
        //id = 69420;
        //name = "Pizza Maker";
        //description = "KIKI ZIZI KINKY ZINKY";
    }
}
