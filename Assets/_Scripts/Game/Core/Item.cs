using UnityEngine;


public enum ItemType 
{
    // Potion (PO)
    POHealth,
    POStamina,
    PODamage,
    PODefense,
    POIceResist,
    
    
    // Jade (JA)
    JARed1,
    JARed2,
    JARed3,
    JARed4,
    
    JABlue1,
    JABlue2,
    JABlue3,
    JABlue4,
     
    JAYellow1,
    JAYellow2,
    JAYellow3,
    JAYellow4,
    
    
    // Experience (EXP)
    EXPSmall,
    EXPMedium,
    EXPBig,
    
    
    // Others (O)
    OCommonSword,
    OCommonBow,
}

public class Item : MonoBehaviour
{
    public Sprite icon;
    public ItemType type;
    public int value;
}
