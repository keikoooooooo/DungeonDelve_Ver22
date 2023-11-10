
namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
    [TaskCategory("Unity/Animator")]
    public class ResetTrigger : Action
    {
        public SharedAnimator animator;
        [Tooltip("The name of the parameter")]
        public SharedString name;


        public override TaskStatus OnUpdate()
        {
            if (animator != null && animator.Value != null)
            {
                animator.Value.ResetTrigger(name.Value);
            }
            
            return TaskStatus.Success;
        }
        
        
    }

}
