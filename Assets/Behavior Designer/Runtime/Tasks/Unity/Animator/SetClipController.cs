using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskCategory("Unity/Animator")]
    [TaskDescription("Đặt clip vào bộ OverrideController tại slotName.")]
    public class SetClipController : Action
    {
        [Tooltip("Bộ AnimatorOverrideController cần đổi Clip")]
        public AnimatorOverrideController controller;
        [Tooltip("Clip cần thay đổi")]
        public AnimationClip clip;
        [Tooltip("Slot trong OverrideController cần gán clip")]
        public SharedString slotName;

        [Tooltip("Có chờ clip chạy hết ?")] 
        public bool isWaitClip;

        private float waitDuration;

        public override void OnStart()
        {
            if(controller == null || clip == null) return;
            
            controller.SetClip(slotName.Value, clip);
            waitDuration = clip.length;
        }

        public override TaskStatus OnUpdate()
        {
            if (!isWaitClip) return TaskStatus.Success;
            
            waitDuration = waitDuration > 0 ? waitDuration - Time.deltaTime : 0;
            return waitDuration <= 0 ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnReset()
        {
            slotName = "";
            waitDuration = 0;
        }
        
    }
}
