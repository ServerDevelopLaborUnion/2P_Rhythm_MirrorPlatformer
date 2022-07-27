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

            foreach(string u in RoomManager.Instance.RoomList)
                CreateRoom(u);
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
