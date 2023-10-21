using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
    [TaskCategory("Unity/Animator")]
    [TaskDescription("Sets the bool parameter on an animator. Returns Success.")]
    public class SetBoolParameter : Action
    {
        public SharedAnimator animator;
        [Tooltip("The name of the parameter")]
        public SharedString parameterName;
        [Tooltip("The value of the bool parameter")]
        public SharedBool boolValue;
        
        public override void OnStart()
        {
            if (animator == null) {
                Debug.LogWarning("Animator is null");
                return;
            }
            animator.Value.SetBool(parameterName.Value, boolValue.Value);
        }
        public override void OnReset()
        {
            parameterName = "";
        }
    }
    
}