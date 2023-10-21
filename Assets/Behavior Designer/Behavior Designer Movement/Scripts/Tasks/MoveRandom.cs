using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    
    [TaskCategory("Movement")]
    [TaskIcon("Assets/Behavior Designer/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
    public class MoveRandom : NavMeshMovement
    {
        [Tooltip("Bán kính hình tròn tối đa của điểm đích ngẫu nhiên.")]
        public SharedFloat radius;
        
        private readonly List<Vector3> _directions = new List<Vector3>();

        
        public override void OnStart()
        {
            base.OnStart();
            
            if(Velocity().magnitude != 0) 
                return;
            
            _directions.Clear(); // Xóa danh sách cũ (nếu có)
            _directions.Add(location.Value.transform.right.normalized);   // Hướng bên trái
            _directions.Add(-location.Value.transform.right.normalized);    // Hướng bên phải
            
            SetDestination(Target());
        }
        public override TaskStatus OnUpdate()
        {
            if (HasArrived()) {
                return TaskStatus.Success;
            }

            BlendAnimation();
            RotationToTarget(target.Value.transform.position);
            
            return TaskStatus.Running;
        }
        
        private Vector3 Target()
        {
            var currentPosition = transform.position;
            var randomDirection = _directions[Random.Range(0, _directions.Count)];
            
            randomDirection.x = randomDirection.x == 0 ? 0 : Mathf.Sign(randomDirection.x);
            var valueTemp = randomDirection.x;
            randomDirection *= radius.Value;
            
            var randomPosition = currentPosition + randomDirection;

            targetBlend.Value = valueTemp;
            if (location.Value.eulerAngles.y >= 90 && location.Value.eulerAngles.y <= 225)  targetBlend.Value *= -1;
            
            return !NavMesh.SamplePosition(randomPosition, out var hit, radius.Value, NavMesh.AllAreas) ? currentPosition : hit.position;
        }
        
       

        
        
    }
}

