using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Seek the target specified using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
    public class Seek : NavMeshMovement
    {
        [Tooltip("If target is null then use the target position")]
        public SharedVector3 targetPosition;
        
        public override void OnStart()
        {
            base.OnStart();
            
            SetDestination(Target());
        }

        // Seek the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        public override TaskStatus OnUpdate()
        {
            if (HasArrived())
            {
                return TaskStatus.Success;
            }
            
            BlendAnimation();
            SetDestination(Target());
            RotationToTarget(Target());
            
            return TaskStatus.Running;
        }
        
        // Return targetPosition if target is null
        private Vector3 Target() => target.Value != null ? target.Value.transform.position : targetPosition.Value;

        public override void OnReset()
        {
            base.OnReset();
            
            target = null;
            targetPosition = Vector3.zero;
        }
        
    }
}