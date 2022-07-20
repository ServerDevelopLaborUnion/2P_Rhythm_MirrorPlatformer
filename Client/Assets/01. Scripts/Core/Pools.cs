using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Pools <T> where T : PoolableMono
    {
        private Stack<T> pool = new Stack<T>();
        private T prefab = null;
        private Transform parent = null;

        public Pools(T Prefab, Transform Parent)
        {
            prefab = Prefab;
            parent = Parent;
        }

        public T Pop()
        {
            T temp = null;

            if(pool.Count > 0)
            {
                temp = pool.Pop();
                temp.gameObject.SetActive(true);
            }
            else
            {
                temp = GameObject.Instantiate(prefab, parent);
                temp.name = temp.name.Replace("(Clone)", "");
            }

            temp.Reset();
            return temp;
        }

        public void Push(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(parent);
            pool.Push(obj);
        }
    }
}
