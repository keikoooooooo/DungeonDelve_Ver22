using System.Collections.Generic;
using UnityEngine;

public static class CollisionData
{
    private static readonly Dictionary<GameObject, IDamageable> _dictionary = new();
    
    
    public static void Add(GameObject _obj, IDamageable IDamage)
    {
        if (_dictionary.ContainsKey(_obj)) return;
        
        _dictionary.Add(_obj, IDamage);
    }
    
    public static void Remove(GameObject _obj)
    {
        if (!_dictionary.ContainsKey(_obj)) return;
        
        _dictionary.Remove(_obj);
    }
    
    public static bool Contains(GameObject _obj, out IDamageable collision)
    {
        return _dictionary.TryGetValue(_obj, out collision);
    }
    
    
}
