using UnityEngine;

public enum PushDirectionEnums
{
    Forward,
    Behind
}

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Tùy chỉnh di chuyển của đối tượng ra trước hoặc sau bằng Vector3.MoveTowards theo trục Forward của objectRotation")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer/Behavior Designer Movement/Editor/Icons/{SkinColor}MoveTowardsIcon.png")]
    public class MoveCustom : Action
    {
        
        [Tooltip("Tốc độ di chuyển")]
        public SharedFloat speed;           
        
        [Tooltip("Có luôn xoay về phía mục tiêu khi lùi lại không?")]
        public SharedBool lookAtTarget = true;
        
        [Tooltip("Khoảng cách di chuyển tối đa")]
        public SharedFloat maxDistance = .1f;
        
        [Tooltip("Delta xoay tối đa nếu LookAtTarget được bật")]
        public SharedFloat maxLookAtRotationDelta;

        [Tooltip("Object cần áp dụng Rotation")] 
        public SharedGameObject objectRotation;
        
        [Tooltip("Gameobject mục tiêu")]
        public SharedGameObject target;

        [Tooltip("Hướng đẩy: Forward di chuyển về phía trước và Behind là về sau")]
        public PushDirectionEnum PushDirectionEnum;

        private int _getDirection;
        private float _currentSpeed;
        
        private Vector3 _targetPosition;
        private Vector3 _moveDirection;
        
        
        public override void OnStart()
        {
            _getDirection = PushDirectionEnum == PushDirectionEnum.Forward ? 1 : -1;
            _currentSpeed = 0;
        }

        public override TaskStatus OnUpdate()
        {
            if (target.Value == null) 
                return TaskStatus.Failure;

            _currentSpeed += speed.Value * Time.deltaTime;
            _currentSpeed = Mathf.Clamp(_currentSpeed, 0, speed.Value);
            
            _moveDirection = objectRotation.Value.transform.forward * _getDirection;
            _targetPosition = transform.position + _moveDirection * maxDistance.Value;
            
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _currentSpeed * Time.deltaTime);
            
            if (lookAtTarget.Value)
            {
                objectRotation.Value.transform.rotation = Quaternion.RotateTowards(objectRotation.Value.transform.rotation,
                    Quaternion.LookRotation(target.Value.transform.position - transform.position), maxLookAtRotationDelta.Value);
            }
            
            return TaskStatus.Running;
        }
        

        public override void OnReset()
        {
            lookAtTarget = true;
        }

        
    }   
    
    
}
