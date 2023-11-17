using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{

    [Category("Movement/Direct")]
    [Description("Di chuyển Object tới mục tiêu nhưng không tìm đường")]
    public class MoveTowards : ActionTask<Transform>
    {

        [RequiredField]
        public BBParameter<GameObject> target;
        public BBParameter<float> speed = 2;
        public BBParameter<float> stopDistance = 0.1f;
        public bool updateFrameByFrame;
        
        [Space, Tooltip("Biến để lưu giá trị xoay được vào, nếu target bên trái sẽ trả về -1 ngược lại trả về 1"), RequiredField]
        public BBParameter<float> saveFoundParameter;
        
        public bool waitActionFinish;
       
        private float CurrentDistance => (agent.position - target.value.transform.position).magnitude;

        
        protected override void OnUpdate() 
        {
            if (CurrentDistance <= stopDistance.value )
            {
                saveFoundParameter.value = 0;
                if(!updateFrameByFrame)
                    EndAction();
                return;
            }
            
            saveFoundParameter.value = 1;
            agent.position = Vector3.MoveTowards(agent.position, target.value.transform.position, speed.value * Time.deltaTime);

            if ( !waitActionFinish ) 
            {
                EndAction();
            }
        }


    }
}