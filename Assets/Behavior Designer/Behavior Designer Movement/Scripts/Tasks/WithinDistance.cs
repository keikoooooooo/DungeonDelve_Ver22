using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Check to see if the any object specified by the object list or tag is within the distance specified of the current agent.")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer/Behavior Designer Movement/Editor/Icons/{SkinColor}WithinDistanceIcon.png")]
    public class WithinDistance : Conditional
    {
        [Tooltip("Đối tượng cụ thể muốn kiểm tra khoảng cách đến")]
        public SharedGameObject targetObject;
        
        [Tooltip("Tag của đối tượng muốn kiểm tra khoảng cách đến")]
        public SharedString targetTag;
        
        [Tooltip("LayerMask cho các đối tượng bạn muốn kiểm tra khoảng cách đến")]
        public LayerMask objectLayerMask;
        
        [Tooltip("Nếu sử dụng mặt nạ lớp đối tượng, hãy chỉ định số lượng máy va chạm tối đa mà vật lý có thể va chạm với")]
        public int maxCollisionCount = 200;
        
        [Tooltip("Khoảng cách tối đa mà đối tượng cần phải nằm trong để trả về thành công")]
        public SharedFloat magnitude = 5;
        
        [Tooltip("Nếu true, đối tượng cần phải có đường nhìn trực tiếp đến đối tượng hiện tại mới trả về thành công.")]
        public SharedBool lineOfSight;
        
        [Tooltip("LayerMask cho các đối tượng bạn muốn bỏ qua khi kiểm tra đường nhìn (nếu lineOfSight là true)")]
        public LayerMask ignoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        
        [Tooltip("Vị trí Offset của đối tượng hiện tại")]
        public SharedVector3 offset;
        
        [Tooltip("Vị trí Offset của đối tượng mục tiêu")]
        public SharedVector3 targetOffset;
        
        [Tooltip("Có vẽ ra trong Scene View để kiểm tra đường nhìn?")]
        public SharedBool drawDebugRay;
        
        [Tooltip("Sẽ chứa đối tượng đã được kiểm tra thành công nếu có.")]
        public SharedGameObject returnedObject;

        private List<GameObject> objects;
        // distance * distance, optimization so we don't have to take the square root
        private float sqrMagnitude;
        private bool overlapCast = false;
        private Collider[] overlapColliders;
        private Collider2D[] overlap2DColliders;

        public override void OnStart()
        {
            sqrMagnitude = magnitude.Value * magnitude.Value;

            if (objects != null) {
                objects.Clear();
            } else {
                objects = new List<GameObject>();
            }
            // if objects is null then find all of the objects using the layer mask or tag
            if (targetObject.Value == null) {
                if (!string.IsNullOrEmpty(targetTag.Value)) {
                    var gameObjects = GameObject.FindGameObjectsWithTag(targetTag.Value);
                    for (int i = 0; i < gameObjects.Length; ++i) {
                        objects.Add(gameObjects[i]);
                    }
                } else {
                    overlapCast = true;
                }
            } else {
                objects.Add(targetObject.Value);
            }
        }

        // Trả về thành công nếu bất kỳ đối tượng nào nằm trong khoảng cách của đối tượng hiện tại. Nếu không nó sẽ trả về thất bại
        public override TaskStatus OnUpdate()
        {
            if (transform == null || objects == null)
                return TaskStatus.Failure;

            if (overlapCast) {
                objects.Clear();
                if (overlapColliders == null) {
                    overlapColliders = new Collider[maxCollisionCount];
                }
                var count = Physics.OverlapSphereNonAlloc(transform.position, magnitude.Value, overlapColliders, objectLayerMask.value);
                for (int i = 0; i < count; ++i) {
                    objects.Add(overlapColliders[i].gameObject);
                }
            }

            Vector3 direction;
            // check each object. All it takes is one object to be able to return success
            for (int i = 0; i < objects.Count; ++i) {
                if (objects[i] == null || objects[i] == gameObject) {
                    continue;
                }
                direction = objects[i].transform.position - (transform.position + offset.Value);
                // check to see if the square magnitude is less than what is specified
                if (Vector3.SqrMagnitude(direction) < sqrMagnitude) {
                    // the magnitude is less. If lineOfSight is true do one more check
                    if (lineOfSight.Value) {
                        var hitTransform = MovementUtility.LineOfSight(transform, offset.Value, objects[i], targetOffset.Value, false, ignoreLayerMask.value, drawDebugRay.Value);
                        if (hitTransform != null && MovementUtility.IsAncestor(hitTransform, objects[i].transform)) {
                            // the object has a magnitude less than the specified magnitude and is within sight. Set the object and return success
                            returnedObject.Value = objects[i];
                            return TaskStatus.Success;
                        }
                    } else {
                        // the object has a magnitude less than the specified magnitude. Set the object and return success
                        returnedObject.Value = objects[i];
                        return TaskStatus.Success;
                    }
                }
            }
            // no objects are within distance. Return failure
            return TaskStatus.Failure;
        }

        public override void OnReset()
        {
            targetObject = null;
            targetTag = string.Empty;
            objectLayerMask = 0;
            magnitude = 5;
            lineOfSight = true;
            ignoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
            offset = Vector3.zero;
            targetOffset = Vector3.zero;
        }

        // Draw the seeing radius
        public override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (Owner == null || magnitude == null) {
                return;
            }
            var oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(Owner.transform.position, Owner.transform.up, magnitude.Value);
            UnityEditor.Handles.color = oldColor;
#endif
        }
    }
}