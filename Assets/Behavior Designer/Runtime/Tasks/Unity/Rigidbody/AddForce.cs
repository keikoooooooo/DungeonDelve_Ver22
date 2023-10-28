using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody
{
    [RequiredComponent(typeof(Rigidbody))]
    [TaskCategory("Unity/Rigidbody")]
    [TaskDescription("Applies a force to the rigidbody. Returns Success.")]
    public class AddForce : Action
    {
        [Tooltip("Rigibody của đối tượng")]
        public Rigidbody rigidbody;
        
        [Tooltip("Object mà Rigibody cần tác động")]
        public SharedGameObject targetGameObject;
        
        [Tooltip("Lực áp dụng vào")]
        public SharedVector3 force;
        
        [Tooltip("Loại lực")]
        public ForceMode forceMode = ForceMode.Force;
        
        public override TaskStatus OnUpdate()
        {
            if (rigidbody == null) {
                Debug.LogWarning("Rigidbody is null");
                return TaskStatus.Failure;
            }

            rigidbody.AddForce(force.Value, forceMode);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            if (force != null) {
                force.Value = Vector3.zero;
            }
            forceMode = ForceMode.Force;
        }
    }
}