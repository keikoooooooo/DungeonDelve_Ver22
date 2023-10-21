using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
    // Di chuyển giá trị Float đến giá trị Target
    [TaskCategory("Unity/Math")]
    [TaskDescription("Moves a value current towards target.")]
    public class FloatMoveTowards : Action
    {
        [Tooltip("Giá trị hiện tại")]
        public SharedFloat floatVariable;
        
        [Tooltip("Giá trị cần hướng tới")]
        public SharedFloat targetValue;

        [Tooltip("Giới hạn tối đa cho sự thay đổi giá trị trong một lần gọi")]
        public SharedFloat maxDelta;


        public override TaskStatus OnUpdate()
        {
            floatVariable.Value = Mathf.MoveTowards(floatVariable.Value, targetValue.Value, maxDelta.Value * Time.deltaTime);
            return TaskStatus.Success;
        }
        
        public override void OnReset()
        {
            floatVariable = 0;
            targetValue = 0;
        }
        
    }
}

