using System.Collections;
using UnityEngine;

public sealed class GroundCheck : MonoBehaviour
{
    
    [Header("Grounded")]
    [Tooltip("Vị trí kiểm tra va chạm với mặt đất"), SerializeField]
    private Transform groundCheck;

    [Tooltip("Khoảng cách va chạm với mặt đất từ vị trí groundCheck"), SerializeField, Range(0, 1)]
    private float distance;
    
    [Tooltip("Kiểm tra va chạm với Layer nào?"), SerializeField]
    private LayerMask groundMask;

    
    public bool IsGrounded { get; private set; } // có đang trên mặt đất ?
    
    
    
    private Coroutine _groundCheckCoroutine;
    
    
    private void Start()
    {
        if(_groundCheckCoroutine != null) StopCoroutine(_groundCheckCoroutine);
        _groundCheckCoroutine = StartCoroutine(GroundCheckCoroutine());
    }

    private IEnumerator GroundCheckCoroutine()
    {
        while (true)
        {
            IsGrounded = Physics.Raycast(groundCheck.position, Vector3.down, distance, groundMask);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if(groundCheck == null) 
            return;
        
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(groundCheck.position, Vector3.down * distance);
    }
    
}



