using UnityEngine;

public class Sword : EffectBase
{
    public override void FIRE()
    {
        Flash(ActiveType.Enable);
        Projectile(ActiveType.Enable);
    }

}
