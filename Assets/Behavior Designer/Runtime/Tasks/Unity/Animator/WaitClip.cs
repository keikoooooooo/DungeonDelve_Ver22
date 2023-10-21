using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Wait a specified amount of time. The task will return running until the task is done waiting. It will return success after the wait time has elapsed.")]
    [TaskIcon("{SkinColor}WaitIcon.png")]
    [TaskCategory("Unity/Animator")]
    public class WaitClip : Action
    {
        public AnimationClip clip;
        
        private float waitDuration;


        public override void OnStart()
        {
            // Remember the start time.
            waitDuration = clip.length;
        }
        
        public override TaskStatus OnUpdate()
        {
            if (waitDuration >= 0)
            {
                // Nếu chưa hoàn thành, node này sẽ luôn chạy
                waitDuration -= Time.deltaTime;
                return TaskStatus.Running;
            }
            // Tác vụ hoàn thành
            return TaskStatus.Success;
        }
        
        public override void OnReset()
        {
            // Reset the public properties back to their original values
            waitDuration = clip.length;
        }

    }
}

