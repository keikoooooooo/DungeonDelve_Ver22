using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayerMask;
    private void OnTriggerEnter(Collider other)
    {
        if(!playerLayerMask.Contains(other.gameObject)) return;
        GameManager.Instance.Player.Health.Decreases(int.MaxValue);
    }
}
