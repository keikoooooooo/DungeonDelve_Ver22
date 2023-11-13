using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{

    [Category("Movement/Direct")]
    [Description("Nếu tùy chọn updateFrameByFrame = TRUE sẽ xoay đối tượng về phía mục tiêu trên mỗi khung hình, ngược lại sẽ cập nhật đến khi góc của " +
                 "(this) và (taget) <= angleDifference")]
    public class RotateTowards : ActionTask<Transform>
    {
        [RequiredField]
        public BBParameter<GameObject> target;
        public BBParameter<float> speed = 2;

        
        public bool updateFrameByFrame;
        
        [SliderField(0, 180)]
        public BBParameter<float> angleDifference = 5;
        public BBParameter<Vector3> upVector = Vector3.up;
        
        // [Tooltip("Nếu tùy chọn = TRUE node sẽ chờ khi góc xoay hoàn thành,")]
        // public bool waitActionFinish;
        
        [Space, Tooltip("Biến để lưu giá trị xoay được vào, nếu target bên trái sẽ trả về -1 ngược lại trả về 1"), RequiredField]
        public BBParameter<float> saveFoundParameter;

        private Quaternion currentRotate;
        private float deltaAngle;
        
        protected override void OnExecute()
        {
            currentRotate = Quaternion.Euler(Vector3.zero);
        }

        protected override void OnUpdate() 
        {
            if (!updateFrameByFrame && Vector3.Angle(target.value.transform.position - agent.position, agent.forward) <= angleDifference.value )
            {
                saveFoundParameter.value = 0;
                EndAction();
                return;
            }
             
            var dir = target.value.transform.position - agent.position;
            agent.rotation = Quaternion.LookRotation(Vector3.RotateTowards(agent.forward, dir, speed.value * Time.deltaTime, 0), upVector.value);

            deltaAngle = Mathf.DeltaAngle(currentRotate.eulerAngles.y, agent.eulerAngles.y);
            currentRotate = agent.rotation;
        
            saveFoundParameter.value = deltaAngle == 0 || Mathf.Abs(deltaAngle) <= angleDifference.value ? 0 : Mathf.Sign(deltaAngle);
            
        }
    }
}