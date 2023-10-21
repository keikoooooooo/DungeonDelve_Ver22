using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public EnemyController enemy;
    public GameObject waypoints;
    
    private void Start()
    {
        var e = Instantiate(enemy, transform);
    }

    private void OnDrawGizmos()
    {
        if(waypoints == null || waypoints.transform.childCount == 0) return;
        Gizmos.color = Color.red;
        for (int i = 0; i < waypoints.transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(waypoints.transform.GetChild(i).position, waypoints.transform.GetChild(i + 1).position);
        }
    }
}
