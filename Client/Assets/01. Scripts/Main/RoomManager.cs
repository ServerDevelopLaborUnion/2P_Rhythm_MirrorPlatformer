using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Main
{
    public class RoomManager : MonoBehaviour
    {
        public static RoomManager Instance = null;

        [SerializeField] PoolableMono roomUnit;
        public List<string> RoomList { get; set; } = new List<string>();

        private void Awake()
        {
            if(Instance != null) { Debug.Log($"Multiple RoomManager Instance is Running, Destroy This"); Destroy(gameObject); }
            if(Instance == null) { Instance = this; DontDestroyOnLoad(transform.root.gameObject); }
        }

        public void AddRoom(string Name)
        {
            RoomList.Add(Name);
        }

        public void RemoveRoom(string Name)
        {
            RoomList.Remove(Name);
        }
    }
}
