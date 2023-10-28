using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent
{
    [TaskCategory("Unity/NavMeshAgent")]
    [TaskDescription("Clears the current path. Returns Success.")]
    public class ResetPath : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedNavMeshAgent navMeshAgent;
        

        public override TaskStatus OnUpdate()
        {
            if (navMeshAgent == null) {
                Debug.LogWarning("NavMeshAgent is null");
                return TaskStatus.Failure;
            }

            navMeshAgent.Value.ResetPath();

            return TaskStatus.Success;
        }
    }
}