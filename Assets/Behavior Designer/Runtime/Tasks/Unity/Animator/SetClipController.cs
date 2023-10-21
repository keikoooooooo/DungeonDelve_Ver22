using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskCategory("Unity/Animator")]
    [TaskDescription("Đặt clip vào bộ OverrideController tại slotName.")]
    public class SetClipController : Action
    {
        public AnimatorOverrideController controller;
        public AnimationClip clip;
        public SharedString slotName;


        public override void OnStart()
        {
            if(controller == null || clip == null || controller[slotName.Value] == clip) return;
            
            controller.SetClip(slotName.Value, clip);
        }

        public override void OnReset()
        {
            slotName = "";
        }
        
    }
}
