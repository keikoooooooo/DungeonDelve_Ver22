using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    public abstract class NavMeshMovement : Movement
    {
        [Tooltip("Thành phần NavMeshAgent")] 
        public SharedNavMeshAgent navMeshAgent;
        [Tooltip("Tốc độ di chuyển")]
        public SharedFloat speed = 10;
        [Tooltip("Tốc độ góc")]
        public SharedFloat angularSpeed = 120;
        [Tooltip("Đã đến nơi khi điểm đến ít hơn khoảng cách quy định. Khoảng cách này phải lớn hơn hoặc bằng NavMeshAgent StoppingDistance.")]
        public SharedFloat arriveDistance = 0.2f;
        [Tooltip("NavMeshAgent có dừng khi tác vụ kết thúc không?")]
        public SharedBool stopOnTaskEnd = true;
        // [Tooltip("Có cập nhật vòng xoay NavMeshAgent khi tác vụ kết thúc không?")]
        // public SharedBool updateRotation = true;
        [Tooltip("Sẽ áp dụng Rotation lên Object này.")] 
        public SharedGameObject objectRotation;
        [Tooltip("Object của mục tiêu cần thao tác")]
        public SharedGameObject target;
        
        [Space]
        [Tooltip("Thành phần phát animation")]
        public SharedAnimator animator;
        [Tooltip("Parameter để set tốc độ để blend Animation")]
        public SharedString animIDBlend;
        [Tooltip("Giá trị mục tiêu cần hòa trộn tới")]
        public SharedFloat targetBlend;
        [Tooltip("Tỉ lệ hòa trộn animation tối đa trong 1fr")]
        public float maxDeltaBlend = 3;
        
        
        // Component references
        private bool startUpdateRotation;
        private Quaternion _target;
        
        private float _animationBlend;
        
        
        /// <summary>
        /// Allow pathfinding to resume.
        /// </summary>
        public override void OnStart()
        {
            navMeshAgent.Value.speed = speed.Value;
            navMeshAgent.Value.angularSpeed = angularSpeed.Value;
            navMeshAgent.Value.isStopped = false;
            startUpdateRotation = navMeshAgent.Value.updateRotation;
            UpdateRotation(false);
            // UpdateRotation(updateRotation.Value);
        }

        /// <summary>
        /// Đặt 1 đích đến mới để di chuyển 
        /// </summary>
        /// <param name="destination"> Vị trí đích đến cần đặt.</param>
        /// <returns>True: Đúng nếu đích đến hợp lệ.</returns>
        protected override bool SetDestination(Vector3 destination)
        {
            navMeshAgent.Value.isStopped = false;
            return navMeshAgent.Value.SetDestination(destination);
        }

        /// <summary>
        /// Specifies if the rotation should be updated.
        /// </summary>
        /// <param name="update">Should the rotation be updated?</param>
        protected override void UpdateRotation(bool update)
        {
            navMeshAgent.Value.updateRotation = update;
            navMeshAgent.Value.updateUpAxis = update;
        }

        /// <summary>
        /// Có đường dẫn tìm đường không?
        /// </summary>
        /// <returns>True: Đúng nếu có đường dẫn tìm đường.</returns>
        protected override bool HasPath()
        {
            return navMeshAgent.Value.hasPath && navMeshAgent.Value.remainingDistance > arriveDistance.Value;
        }

        /// <summary>
        /// Trả về vận tốc.
        /// </summary>
        /// <returns> Trả về vận tốc hiện tại mà navMesh đang di chuyển.</returns>
        protected override Vector3 Velocity()
        {
            return navMeshAgent.Value.velocity;
        }

        /// <summary>
        /// Trả về True nếu vị trí là vị trí tìm đường hợp lệ.
        /// </summary>
        /// <param name="position"> Vị trí cần kiểm tra.</param>
        /// <returns> True: Đúng nếu vị trí tìm được là vị trí tìm đường hợp lệ.</returns>
        protected bool SamplePosition(Vector3 position)
        {
            NavMeshHit hit;
            return NavMesh.SamplePosition(position, out hit, navMeshAgent.Value.height * 2, NavMesh.AllAreas);
        }

        /// <summary>
        /// Đã di chuyển tới đích chưa ?
        /// </summary>
        /// <returns>True: Đúng nếu đã đến đích. </returns>
        protected override bool HasArrived()
        {
            // The path hasn't been computed yet if the path is pending.
            float remainingDistance;
            if (navMeshAgent.Value.pathPending) {
                remainingDistance = float.PositiveInfinity;
            } else {
                remainingDistance = navMeshAgent.Value.remainingDistance;
            }

            return remainingDistance <= arriveDistance.Value;
        }

        /// <summary>
        /// Dừng việc tìm đường.
        /// </summary>
        protected override void Stop()
        {
            UpdateRotation(startUpdateRotation);
            if (navMeshAgent.Value.hasPath) {
                navMeshAgent.Value.isStopped = true;
            }
        }

        /// <summary>
        /// Nhiệm vụ đã kết thúc. Không di chuyển nữa.
        /// </summary>
        public override void OnEnd()
        {
            if (stopOnTaskEnd.Value) {
                Stop();
            } else {
                UpdateRotation(startUpdateRotation);
            } 
        }

        /// <summary>
        /// Cây hành vi đã kết thúc. Không di chuyển nữa.
        /// </summary>
        public override void OnBehaviorComplete()
        {
            Stop();
        }

        /// <summary>
        /// Đặt lại các giá trị về mặc định của chúng.
        /// </summary>
        public override void OnReset()
        {
            speed = 10;
            angularSpeed = 120;
            arriveDistance = 1;
            stopOnTaskEnd = true;
        }

        /// <summary>
        /// Xoay về hướng được chỉ định bằng Quaternion, ở đây sẽ xoay Object Location chứ không xoay object đang giữ script
        /// </summary>
        /// <param name="targetPosition"> Vị trí của mục tiêu </param>
        protected override void RotationToTarget(Vector3 targetPosition)
        {
            _target = Quaternion.LookRotation(targetPosition - transform.position);

            var normalized = NormalizedRotation(_target.eulerAngles.y);
            var quaternion = Quaternion.Euler(0, normalized, 0);
            objectRotation.Value.transform.rotation = Quaternion.RotateTowards(objectRotation.Value.transform.rotation, quaternion, angularSpeed.Value);
        }
        private float NormalizedRotation(float rotation)
        {
            if (rotation < 0)    return rotation + 360;
            if (rotation >= 360) return rotation - 360;
            return rotation;
        }

        /// <summary>
        /// Hòa trộn giá trị parameter của animator
        /// NOTE: Trước khi Blend cần cập nhật giá trị lại của biến '_targetBlend' để blend tới giá trị đó
        /// </summary>
        protected void BlendAnimation()
        {
            if(animator.Value == null || animIDBlend.Value == null) return;
            _animationBlend = Mathf.MoveTowards(_animationBlend, targetBlend.Value, maxDeltaBlend * Time.deltaTime);
            animator.Value.SetFloat(animIDBlend.Value, _animationBlend);
        }
        
    }
}