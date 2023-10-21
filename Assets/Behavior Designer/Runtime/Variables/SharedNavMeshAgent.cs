using UnityEngine.AI;

namespace BehaviorDesigner.Runtime
{
    public class SharedNavMeshAgent : SharedVariable<NavMeshAgent>
    {
        public static explicit operator SharedNavMeshAgent(NavMeshAgent value) { return new SharedNavMeshAgent { mValue = value }; }
    }
}

