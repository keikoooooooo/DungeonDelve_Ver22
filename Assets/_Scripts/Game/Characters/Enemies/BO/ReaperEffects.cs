using UnityEngine;

public class ReaperEffects : MonoBehaviour
{
    public GameObject indicatorSkill;
    public GameObject indicatorNormalAttack;
    private Transform slotVFX;
    
    
    private void Start()
    {
        slotVFX = GameObject.FindGameObjectWithTag("SlotsVFX").transform;
        
        indicatorSkill.transform.SetParent(slotVFX);
        indicatorNormalAttack.transform.SetParent(slotVFX);
    }
    
    
}
