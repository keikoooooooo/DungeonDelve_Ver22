using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
    [TaskCategory("Unity/Animator")]
    [TaskDescription("Sets the float parameter on an animator. Returns Success.")]
    public class SetFloatParameter : Action
    {
        public SharedAnimator animator;
        [Tooltip("The name of the parameter")]
        public SharedString parameterName;
        [Tooltip("Giá trị cần set")] 
        public SharedFloat floatValue;
        
        [Tooltip("Có blend giá trị thay vì gán lập tức ?")] 
        public bool isLerp;
        [Tooltip("Giá trị hòa trộn tối đa trong 1fr")]
        public float maxDelta;
        
        private float animationBlend;
        
        public override void OnStart()
        {
            if (animator == null) {
                Debug.LogWarning("Animator is null");
                return;
            }
            
            animationBlend = animator.Value.GetFloat(parameterName.Value);
        }

        public override TaskStatus OnUpdate()
        {
            if (!isLerp)
            {
                animator.Value.SetFloat(parameterName.Value, floatValue.Value);
                return TaskStatus.Success;
            }
            
            if (System.Math.Abs(animationBlend - floatValue.Value) > 0)
            {
                animationBlend = Mathf.MoveTowards(animationBlend, floatValue.Value, maxDelta * Time.deltaTime); 
                animator.Value.SetFloat(parameterName.Value, animationBlend);
                return TaskStatus.Running;
            }
            
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            parameterName = "";
            floatValue = 0;
        }
        
    }
}