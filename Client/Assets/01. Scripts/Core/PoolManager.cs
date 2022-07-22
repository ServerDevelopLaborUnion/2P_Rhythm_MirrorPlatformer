using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] List<PoolableMono> poolingList = new List<PoolableMono>();
        public static PoolManager Instance = null;
        private Dictionary<string, Pools<PoolableMono>> pools = new Dictionary<string, Pools<PoolableMono>>();
        private Transform pooler = null;

        private void Awake()
        {
            if(Instance != null) { Debug.Log($"Multiple PoolManager Instance is Running, Destroy This"); Destroy(gameObject); }
            if(Instance == null) Instance = this;
            //DontDestroyOnLoad(transform.root.gameObject);

            pooler = transform.GetChild(0);

            foreach(PoolableMono prefab in poolingList)
                CreatePool(prefab, pooler);
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
