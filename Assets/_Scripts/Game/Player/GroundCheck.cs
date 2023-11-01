using System.Collections;
using UnityEngine;

public sealed class GroundCheck : MonoBehaviour
{
    
    [Header("Grounded")]
    [Tooltip("Vị trí kiểm tra va chạm với mặt đất"), SerializeField]
    private Transform groundCheck;

    [Tooltip("Bán kính kiểm tra va chạm với mặt đất"), SerializeField, Range(0, 1)]
    private float groundRadius;
    
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
            IsGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
            //IsGrounded = Physics.CheckBox(groundCheck.position, size);
            yield return null;
        }
    }

    public Vector3 size;
    private void OnDrawGizmos()
    {
        if(groundCheck == null) return;
        
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundRadius);
        
        //Gizmos.DrawWireCube(groundCheck.position, size);
    }
    
}



