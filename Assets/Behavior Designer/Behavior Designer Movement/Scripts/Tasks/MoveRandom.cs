using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    
    [TaskCategory("Movement")]
    [TaskIcon("Assets/Behavior Designer/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
    public class MoveRandom : NavMeshMovement
    {
        
        [Space, Tooltip("Bán kính hình tròn tối đa của điểm đích ngẫu nhiên.")]
        public SharedFloat maxDistance;

        private readonly List<Vector3> _directions = new();

        private Vector3 randomPosition;
        
        
        public override void OnStart()
        {
            base.OnStart();
            
            _directions.Clear(); // Xóa danh sách cũ (nếu có)
            _directions.Add(objectRotation.Value.transform.right.normalized);   // Hướng bên trái
            _directions.Add(-objectRotation.Value.transform.right.normalized);  // Hướng bên phải
            SetDestination(Target());
        }
        public override TaskStatus OnUpdate()
        {
            if (HasArrived()) 
            {
                //targetBlend = 0;
            }
            
            //BlendAnimation();
            RotationToTarget(target.Value.transform.position);
            
            return TaskStatus.Running;
        }
        
        private Vector3 Target()
        {
            var currentPosition = transform.position;
            var randomDirection = _directions[Random.Range(0, _directions.Count)];
            randomDirection.x = Mathf.Sign(randomDirection.x);
            
            //targetBlend.Value =  randomDirection.x;
            randomPosition = currentPosition + randomDirection * maxDistance.Value;
            
            // if (objectRotation.Value.transform.eulerAngles.y is >= 90 and <= 225)
            //     targetBlend.Value *= -1;
            
            return !NavMesh.SamplePosition(randomPosition, out var hit, maxDistance.Value, NavMesh.AllAreas) ? currentPosition : hit.position; 
        }

        public override void OnEnd()
        {
            navMeshAgent.Value.ResetPath();
        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(randomPosition, .2f);
            
            Gizmos.color = Color.magenta;
            _directions.ForEach(x =>  Gizmos.DrawLine(transform.position, transform.position + x));
        }
        
    }
}

