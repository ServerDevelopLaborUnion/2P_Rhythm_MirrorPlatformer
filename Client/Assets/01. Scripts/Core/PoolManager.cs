using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class PoolManager : MonoBehaviour
    {
        private Dictionary<string, Pools<PoolableMono>> pools = new Dictionary<string, Pools<PoolableMono>>();
        public static PoolManager Instance = null;

        private void Awake()
        {
            if(Instance == null) Instance = this;
        }

        public void CreatePool(PoolableMono Prefab, Transform Parent)
        {
            Pools<PoolableMono> pooler = new Pools<PoolableMono>(Prefab, Parent);
            pools.Add(Prefab.name, pooler);
        }

        public PoolableMono Pop(PoolableMono prefab)
        {
            return pools[prefab.name].Pop();
        }
        
        public void Push(PoolableMono obj)
        {
            pools[obj.name].Push(obj);
        }
    }
}
