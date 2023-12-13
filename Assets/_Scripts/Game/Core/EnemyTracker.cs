using System.Collections.Generic;
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
    public static Vector3 FindClosestEnemy(Transform _transfLocal) => SortClosestEnemy(_transfLocal);
    
    private static Vector3 SortClosestEnemy(Transform _transfLocal)
    {
        EnemiesTracker.RemoveAll(trans => trans == null);
        if (!DetectEnemy) return _transfLocal.position;
        EnemiesTracker.Sort((a, b)
            => Vector3.Distance(a.transform.position, _transfLocal.position).CompareTo(Vector3.Distance(b.transform.position, _transfLocal.position)));
        return EnemiesTracker[0].transform.position;
    }

}
