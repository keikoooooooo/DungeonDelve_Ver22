using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Chờ một khoảng thời gian bằng thời gian của animation trong animator hiện tại. Sau khoảng thời gian chờ này node sẽ trả về thành công.")]
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
            if (waitDuration <= 0) 
                return TaskStatus.Success;  // Tác vụ hoàn thành
            
            waitDuration -= Time.deltaTime; // Nếu chưa hoàn thành, node này sẽ luôn chạy
            return TaskStatus.Running;
        }
        
        public override void OnReset()
        {
            // Reset the public properties back to their original values
            waitDuration = clip.length;
        }

    }
}

