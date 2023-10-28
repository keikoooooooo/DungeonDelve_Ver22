using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
    [TaskDescription("Check to see if the any object specified by the object list or tag is within the distance specified of the current agent.")]
    [TaskCategory("Unity/Math")]
    [TaskIcon("Assets/Behavior Designer/Behavior Designer Movement/Editor/Icons/{SkinColor}WithinDistanceIcon.png")]
    public class DistanceComparison : Conditional
    {
        [Tooltip("Đối tượng muốn kiểm tra khoảng cách")]
        public SharedGameObject targetObject;
        
        [Tooltip("Khoảng cách tối đa mà đối tượng cần phải nằm trong để trả về thành công")]
        public SharedFloat magnitude = 5;

        [Tooltip("Giá trị cần kiểm tra")]
        public SharedBool valueCompare;
        
        private float _sqrMagnitude;
        private Vector3 _targetPosition;
        private Vector3 _direction;
        private bool _valueComare;
        
        
        public override void OnStart()
        {
            _sqrMagnitude = magnitude.Value * magnitude.Value;
        }

        public override TaskStatus OnUpdate()
        {
            if (targetObject.Value == null) return TaskStatus.Failure;

            _targetPosition = targetObject.Value.transform.position;
            _direction = _targetPosition - transform.position;
            _valueComare = Vector3.SqrMagnitude(_direction) < _sqrMagnitude;
            
            return valueCompare.Value == _valueComare ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            magnitude = 5;
        }


        public override void OnDrawGizmos()
        {
            if (Owner == null || magnitude == null) {
                return;
            }
            #if UNITY_EDITOR
            var oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(Owner.transform.position, Owner.transform.up, magnitude.Value);
            UnityEditor.Handles.color = oldColor;
            #endif
        }
    }
}

