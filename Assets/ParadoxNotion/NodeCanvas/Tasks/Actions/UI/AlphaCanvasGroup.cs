using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using CanvasGroup = UnityEngine.CanvasGroup;

namespace NodeCanvas.Tasks.Actions
{
    [Name("Set Alpha CanvasGroup")]
    [Category("UI")]
    [Description("Set giá trị Alpha theo thời gian chỉ định")]
    public class AlphaCanvasGroup : ActionTask
    {
        [RequiredField]
        public BBParameter<CanvasGroup> canvasGroup;
        [SliderField(0, 1)]
        public BBParameter<float> setTo;
        [SliderField(0, 1)]
        public float transitTime = 0.25f;

        [Tooltip("Có cập nhật giá trị mỗi Frame")]
        public BBParameter<bool> updateFrameByFrame;
        
        private float currentValue;
        
        protected override string info => $"Set Alpha CanvasGroup to {setTo.value}";

        protected override void OnExecute()
        {
            if (transitTime == 0)
            {
                Set(setTo.value);
                EndAction();
                return;
            }

            currentValue = Get();
        }

        protected override void OnUpdate()
        {
            Set(Mathf.Lerp(currentValue, setTo.value, elapsedTime / transitTime));
            if (elapsedTime >= transitTime && !updateFrameByFrame.value)
            {
                EndAction(true);
            }
                
        }

        private float Get() => canvasGroup.value.alpha;
        private void Set(float _value) => canvasGroup.value.alpha = _value;

    }
}

