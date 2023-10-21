using UnityEngine;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedAnimator : SharedVariable<Animator>
    {
        public static implicit operator SharedAnimator(Animator value) { return new SharedAnimator { mValue = value }; }
    }
}

