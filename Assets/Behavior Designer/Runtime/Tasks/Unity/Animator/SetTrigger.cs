using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
    [TaskCategory("Unity/Animator")]
    [TaskDescription("Sets a trigger parameter to active or inactive. Returns Success.")]
    public class SetTrigger : Action
    {
        public SharedAnimator animator;
        [Tooltip("The name of the parameter")]
        public SharedString parameterName;
        
        public override void OnStart()
        {
            if (animator == null) {
                Debug.LogWarning("Animator is null");
                return;
            }
            animator.Value.SetTrigger(parameterName.Value);
        }
        
        public override void OnReset()
        {
            parameterName = "";
        }
    }
}
