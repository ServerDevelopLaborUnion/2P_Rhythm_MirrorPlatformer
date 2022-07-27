using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Main
{
    public class RoomManager : MonoBehaviour
    {
        public static RoomManager Instance = null;

        [SerializeField] PoolableMono roomUnit;
        private Transform canvas = null;
        private Transform content = null;
        private List<string> roomList = new List<string>();

        private void Awake()
        {
            if(Instance != null) { Debug.Log($"Multiple RoomManager Instance is Running, Destroy This"); Destroy(gameObject); }
            if(Instance == null) { Instance = this; DontDestroyOnLoad(transform.root.gameObject); }

            canvas = GameObject.Find("Canvas").transform;
            content = canvas.Find("MainPanel/Second/Panels/RoomPanel/ScrollView/Viewport/Content");
        }

        public void RoomUpdate()
        {
            foreach(RoomUnit unit in content.GetComponentsInChildren<RoomUnit>() )
                PoolManager.Instance.Push(unit);

            foreach(string u in roomList)
                CreateRoom(u);
        }

        public void AddRoom(string Name)
        {
            roomList.Add(Name);
        }

        public void RemoveRoom(string Name)
        {
            roomList.Remove(Name);
        }

        public void CreateRoom(string RoomName)
        {
            RoomUnit temp = PoolManager.Instance.Pop(roomUnit) as RoomUnit;
            temp.Init(RoomName);
            temp.transform.SetParent(content);
            temp.transform.localScale = Vector3.one;
            temp.transform.rotation = content.rotation;
        }
    }
}
