using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Patrol around the specified waypoints using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer/Behavior Designer Movement/Editor/Icons/{SkinColor}PatrolIcon.png")]
    public class Patrol : NavMeshMovement
    {
       [Space]
        [Tooltip("Có tuần tra các điểm tham chiếu một cách ngẫu nhiên không?")]
        public SharedBool randomPatrol = false;
        [Tooltip("Khoảng thời gian tạm dừng khi đến điểm tham chiếu")]
        public SharedFloat waypointPauseDuration = 0;
        [Tooltip("Các điểm cần di chuyển tới")]
        public SharedTransform waypoints;
        
        // The current index that we are heading towards within the waypoints array
        private int waypointIndex;
        private float waypointReachedTime;

        
        public override void OnStart()
        {
            base.OnStart();

            if(waypoints.Value == null) 
                return;
            
            // initially move towards the closest waypoint
            float distance = Mathf.Infinity;
            float localDistance;
            for (int i = 0; i < waypoints.Value.childCount; ++i) {
                if ((localDistance = Vector3.Magnitude(transform.position - waypoints.Value.GetChild(i).transform.position)) < distance) {
                    distance = localDistance;
                    waypointIndex = i;
                }
            }
            targetBlend.Value = 0.5f;
            waypointReachedTime = -1;
            SetDestination(Target());
        }
        // Patrol around the different waypoints specified in the waypoint array. Always return a task status of running. 
        public override TaskStatus OnUpdate()
        {
            if (waypoints.Value == null || waypoints.Value.childCount == 0) { return TaskStatus.Failure; }
            
            if (HasArrived()) {
                if (waypointReachedTime == -1)
                {
                    targetBlend.Value = 0f;
                    waypointReachedTime = Time.time;
                }
                // wait the required duration before switching waypoints.
                if (waypointReachedTime + waypointPauseDuration.Value <= Time.time) {
                    if (randomPatrol.Value) {
                        if (waypoints.Value.childCount == 1) {
                            waypointIndex = 0;
                        } else {
                            // prevent the same waypoint from being selected
                            var newWaypointIndex = waypointIndex;
                            while (newWaypointIndex == waypointIndex) {
                                newWaypointIndex = Random.Range(0, waypoints.Value.childCount);
                            }
                            waypointIndex = newWaypointIndex;
                        }
                    } else {
                        waypointIndex = (waypointIndex + 1) % waypoints.Value.childCount;
                    }
                    targetBlend.Value = 0.5f;
                    SetDestination(Target());
                    waypointReachedTime = -1;
                }
            }
            
            BlendAnimation();
            RotationToTarget(Target());
            return TaskStatus.Running;
        }

        
        
        // Return the current waypoint index position
        private Vector3 Target()
        {
            if (waypointIndex >= waypoints.Value.childCount) {
                return transform.position;
            }
            return waypoints.Value.GetChild(waypointIndex).transform.position;
        }

        // Reset the public variables
        public override void OnReset()
        {
            base.OnReset();

            randomPatrol = false;
            waypointPauseDuration = 0;
            waypoints = null;
        }
        
        
        
        // Draw a gizmo indicating a patrol 
        public override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (waypoints == null || waypoints.Value == null) {
                return;
            }
            var oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.yellow;
            for (int i = 0; i < waypoints.Value.childCount; ++i) {
                if (waypoints.Value.GetChild(i) != null) {
                    UnityEditor.Handles.SphereHandleCap(0, waypoints.Value.GetChild(i).transform.position, waypoints.Value.GetChild(i).transform.rotation, .1f, EventType.Repaint);
                }
            }
            UnityEditor.Handles.color = oldColor;
#endif
        }

    }
}