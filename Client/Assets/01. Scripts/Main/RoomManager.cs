using Core;
using UnityEngine;

namespace Main
{
    public class RoomManager : MonoBehaviour
    {
        public static RoomManager Instance = null;

        [SerializeField] PoolableMono roomUnit;
        private Transform canvas = null;

        private void Awake()
        {
            if(Instance == null) Instance = this;
            if(Instance != null) { Debug.Log($"Multiple RoomManager Instance is Running, Destroy This"); Destroy(gameObject); }
            DontDestroyOnLoad(transform.root.gameObject);

            canvas = GameObject.Find("Canvas").transform;
        }

        public void CreateRoom(string ID, string RoomName)
        {
            RoomUnit temp = PoolManager.Instance.Pop(roomUnit) as RoomUnit;
            temp.Init(ID, RoomName);
        }
    }
}
