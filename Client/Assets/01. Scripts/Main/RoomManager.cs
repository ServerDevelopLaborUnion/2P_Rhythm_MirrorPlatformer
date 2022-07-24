using Core;
using UnityEngine;

namespace Main
{
    public class RoomManager : MonoBehaviour
    {
        public static RoomManager Instance = null;

        [SerializeField] PoolableMono roomUnit;
        private Transform canvas = null;
        private Transform content = null;

        private void Awake()
        {
            if(Instance != null) { Debug.Log($"Multiple RoomManager Instance is Running, Destroy This"); Destroy(gameObject); }
            if(Instance == null) { Instance = this; DontDestroyOnLoad(transform.root.gameObject); }

            canvas = GameObject.Find("Canvas").transform;
            content = canvas.Find("MainPanel/Second/Panels/RoomPanel/ScrollView/Viewport/Content");
        }

        public void CreateRoom(string RoomName)
        {
            RoomUnit temp = PoolManager.Instance.Pop(roomUnit) as RoomUnit;
            temp.Init(RoomName);
            temp.transform.SetParent(content);
            temp.transform.localScale = Vector3.one;
        }
    }
}
