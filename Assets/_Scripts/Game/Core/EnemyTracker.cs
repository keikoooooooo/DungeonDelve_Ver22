using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EnemyTracker
{
    private static readonly List<Transform> EnemiesTracker = new();
    public static bool DetectEnemy => EnemiesTracker.Count != 0;
    
    public static void Add(Transform _transform)
    {
        if(EnemiesTracker.Contains(_transform)) return;
        EnemiesTracker.Add(_transform);
    }
    public static void Remove(Transform _transform)
    {
        if(!EnemiesTracker.Contains(_transform)) return;
        EnemiesTracker.Remove(_transform);
    }
    public static Transform FindClosestEnemy(Transform _transfLocal) => SortClosestEnemy(_transfLocal);
    
    private static Transform SortClosestEnemy(Transform _transfLocal)
    {
        EnemiesTracker.Sort((a, b)
            => Vector3.Distance(a.transform.position, _transfLocal.position).CompareTo(Vector3.Distance(b.transform.position, _transfLocal.position)));
        return EnemiesTracker[0].transform;
    }

}
