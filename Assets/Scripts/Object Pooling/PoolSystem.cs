using System.Collections.Generic;
using UnityEngine;

public static class PoolSystem 
{
    private static readonly int _DEFAULT_POOL_SIZE = 5;

    private static Dictionary<int, List<PoolableObject>> _objectPools = new Dictionary<int, List<PoolableObject>>();

    public static void CreatePool(PoolableObject prefab, int capacity)
    {
        int poolId = prefab.GetInstanceID();
        _objectPools[poolId] = new List<PoolableObject>(capacity);
        prefab.gameObject.SetActive(false);
        for (int i = 0; i < capacity; i++)
        {
            CreateObjectInPool(poolId, prefab);
        }
    }

    private static PoolableObject CreateObjectInPool(int poolId, PoolableObject prefab)
    {
        var clone = GameObject.Instantiate(prefab);
        _objectPools[poolId].Add(clone);
        return clone;
    }

    public static PoolableObject GetNext(PoolableObject prefab, Vector3 position, Quaternion rotation, bool setActive = true)
    {
        var clone = GetNext(prefab);
        clone.transform.SetPositionAndRotation(position, rotation);
        clone.gameObject.SetActive(setActive);
        return clone;
    }

    public static PoolableObject GetNext(PoolableObject prefab)
    {
        int poolId = prefab.GetInstanceID();

        if (_objectPools.ContainsKey(poolId) == false)
        {
            CreatePool(prefab, _DEFAULT_POOL_SIZE);
        }

        var pool = _objectPools[poolId];
        for (int i = 0; i < pool.Count; i++)
        {
            var item = pool[i];
            if (item.gameObject.activeInHierarchy == false)
            {
                return item;
            }
        }

        prefab.gameObject.SetActive(false);
        return CreateObjectInPool(poolId, prefab);
    }
}
