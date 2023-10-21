using UnityEngine;

public class EffectBase : MonoBehaviour
{    
    [Header("Effect Prefab")]
    public ParticleSystem flash;
    public ParticleSystem projectile;
    public ParticleSystem hit;

    public enum ActiveType
    {
        Enable,
        Disable
    }

    /// <summary>
    /// Set Active của Effect Flash theo Parameter
    /// </summary>
    public void Flash(ActiveType _type)
    {
        if(flash == null) 
            return;
        
        switch (_type)
        {
            case ActiveType.Enable:
                flash.gameObject.SetActive(true);
                flash.Play();
                break;
            
            case ActiveType.Disable:
                flash.gameObject.SetActive(false);
                flash.Stop();
                break;
            
            default: Debug.Log("There is no activity type"); break;
        }
    }

    
    /// <summary>
    /// Set Active của Effect Projectile theo Parameter
    /// </summary>
    public void Projectile(ActiveType _type)
    {
        if(projectile == null) 
            return;
        
        switch (_type)
        {
            case ActiveType.Enable:
                projectile.gameObject.SetActive(true);
                projectile.Play();
                break;
            
            case ActiveType.Disable:
                projectile.gameObject.SetActive(false);
                projectile.Stop();
                break;
            
            default: Debug.Log("There is no activity type"); break;
        }
    }


    /// <summary>
    /// Set Active của Effect Hit theo Parameter tại vị trí Pos
    /// </summary>
    public void Hit(ActiveType _type, Vector3 _pos)
    {
        if(hit == null) 
            return;
        
        switch (_type)
        {
            case ActiveType.Enable:
                hit.transform.position = _pos;
                hit.gameObject.SetActive(true);
                hit.Play();
                break;
            
            case ActiveType.Disable:
                hit.gameObject.SetActive(false);
                hit.Stop();
                break;
            
            default: Debug.Log("There is no activity type"); break;
        }
    }
    
}
