using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Move towards the specified position. The position can either be specified by a transform or position. If the transform " +
                     "is used then the position will not be used.")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer/Behavior Designer Movement/Editor/Icons/{SkinColor}MoveTowardsIcon.png")]
    public class MoveTowards : Action
    {
        [Tooltip("Tốc độ di chuyển")]
        public SharedFloat speed;
        [Tooltip("Node sẽ hoàn thành khi khoảng cách từ transform để target < giá trị này")]
        public SharedFloat arriveDistance = 0.1f;
        [Tooltip("Có luôn xoay về phía mục tiêu khi lùi lại không?")]
        public SharedBool lookAtTarget = true;
        [Tooltip("Delta xoay tối đa nếu LookAtTarget được bật")]
        public SharedFloat maxLookAtRotationDelta;
        [Tooltip("Object mục tiêu")]
        public SharedGameObject target;
        [Tooltip("Nếu target == null thì sẽ áp dụng Vector này làm mục tiêu")]
        public SharedVector3 targetPosition;
        
        
        private Vector3 position;
        
        public override void OnStart()
        {
            position = Target();
        }
        public override TaskStatus OnUpdate()
        {
            // Nếu khoảng cách từ transform đến target <= giá trị (arriveDistance) được cho -> Return: SUCCESS
            if (Vector3.Magnitude(transform.position - position) < arriveDistance.Value) {
                return TaskStatus.Success;
            }
            
            // We haven't reached the target yet so keep moving towards it
            transform.position = Vector3.MoveTowards(transform.position, position, speed.Value * Time.deltaTime);
            if (lookAtTarget.Value && (position - transform.position).sqrMagnitude > 0.01f) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(position - transform.position), maxLookAtRotationDelta.Value);
            }
            return TaskStatus.Running;
        }

        
        private Vector3 Target() // Return targetPosition if targetTransform is null 
        {
            if (target == null || target.Value == null) {
                return targetPosition.Value;
            }
            return target.Value.transform.position;
        }

       
        public override void OnReset() // Reset the public variables 
        {
            arriveDistance = 0.1f;
            lookAtTarget = true;
        }
    }
}