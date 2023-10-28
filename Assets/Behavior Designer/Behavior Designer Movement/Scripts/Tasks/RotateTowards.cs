using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Rotates towards the specified rotation. The rotation can either be specified by a transform or rotation. If the transform "+
                     "is used then the rotation will not be used.")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer/Behavior Designer Movement/Editor/Icons/{SkinColor}RotateTowardsIcon.png")]
    public class RotateTowards : Action
    {
        [Tooltip("Should the 2D version be used?")]
        public bool usePhysics2D;
        [Tooltip("Được xoay khi góc nhỏ hơn giá trị này")]
        public SharedFloat rotationEpsilon = 0.5f;
        [Tooltip("Số góc tối đa mà tác nhân có thể xoay trong một tích tắc")]
        public SharedFloat maxLookAtRotationDelta = 1;
        [Tooltip("Chỉ xoay mỗi trục Y ?")]
        public SharedBool onlyY;
        [Tooltip("Đối tượng cần xoay")]
        public SharedGameObject thisRoration;
        
        [Tooltip("Mục tiêu xoay tới")]
        public SharedGameObject target;
        [Tooltip("Vector hướng xoay, nếu mục tiêu là null")]
        public SharedVector3 targetRotation;

        private Quaternion _rotation;

        public override TaskStatus OnUpdate()
        {
            var rotation = Target();
            // Return a task status of success once we are done rotating
            if (Quaternion.Angle(thisRoration.Value.transform.rotation, rotation) < rotationEpsilon.Value) {
                return TaskStatus.Success;
            }
            // We haven't reached the target yet so keep rotating towards it
            thisRoration.Value.transform.rotation = Quaternion.RotateTowards(thisRoration.Value.transform.rotation, rotation, maxLookAtRotationDelta.Value);
            return TaskStatus.Running;
        }

     

        // Return targetPosition if targetTransform is null
        private Quaternion Target()
        {
            if (target == null || target.Value == null) {
                return Quaternion.Euler(targetRotation.Value);
            }
            var position = target.Value.transform.position - transform.position;
            if (onlyY.Value) {
                position.y = 0;
            }
            if (usePhysics2D) {
                var angle = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
                return Quaternion.AngleAxis(angle, Vector3.forward);
            }
            return Quaternion.LookRotation(position);
        }

        // Reset the public variables
        public override void OnReset()
        {
            usePhysics2D = false;
            rotationEpsilon = 0.5f;
            maxLookAtRotationDelta = 1f;
            onlyY = false;
            target = null;
            targetRotation = Vector3.zero;
        }
    }
}