using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
    
    [TaskCategory("Unity/Animator")]
    [TaskDescription("Nếu animator đang chạy clip có tag = parameterName => return True")]
    [TaskIcon("Assets/Behavior Designer/Behavior Designer Movement/Editor/Icons/CheckIcon.png")]
    public class IsTag : Action
    {
        public SharedAnimator animator;
        [Tooltip("The name of the parameter")]
        public SharedString parameterName;
        [Tooltip("Giá trị muốn trả về")]
        public SharedBool boolValue;

        private bool _boolCheck;


        public override void OnStart()
        {
            _boolCheck = animator.Value.IsTag(parameterName.Value);
        }

        public override TaskStatus OnUpdate()
        {
            return boolValue.Value == _boolCheck ? TaskStatus.Success : TaskStatus.Failure;
        }
        
        public override void OnReset()
        {
            parameterName = "";
        }
        
    }
}

