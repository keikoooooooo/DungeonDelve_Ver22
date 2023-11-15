using UnityEngine;

public class ReaperEffects : MonoBehaviour
{
    public GameObject indicatorSkill;
    private Transform slotVFX;
    
    
    private void Start()
    {
        slotVFX = GameObject.FindGameObjectWithTag("SlotsVFX").transform;
        
        indicatorSkill.transform.SetParent(slotVFX);
    }
    
    
}
