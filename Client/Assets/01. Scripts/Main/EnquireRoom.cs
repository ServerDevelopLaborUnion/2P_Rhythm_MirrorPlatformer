using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Main
{
    public class EnquireRoom : MonoBehaviour
    {
        [SerializeField] RoomUnit roomUnit;

        private Transform canvas = null;
        private Transform content = null;

        private void Awake()
        {
            canvas = GameObject.Find("Canvas").transform;
            content = canvas.Find("MainPanel/Second/Panels/RoomPanel/ScrollView/Viewport/Content");
        }

        public void DoEnquire()
        {
            foreach(RoomUnit unit in content.GetComponentsInChildren<RoomUnit>() )
                PoolManager.Instance.Push(unit);

            foreach(KeyValuePair<string, string> rp in RoomManager.Instance.RoomList)
                CreateRoom(rp.Key, rp.Value);
        }

        public void CreateRoom(string RoomName, string Password)
        {
            RoomUnit temp = PoolManager.Instance.Pop(roomUnit) as RoomUnit;
            temp.Init(RoomName, Password);
            temp.transform.SetParent(content);
            temp.transform.localScale = Vector3.one;
            temp.transform.rotation = content.rotation;
        }   
    }
}
