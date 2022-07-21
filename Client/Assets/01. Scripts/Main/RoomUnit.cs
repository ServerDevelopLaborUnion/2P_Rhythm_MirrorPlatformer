using Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Main
{
    public class RoomUnit : PoolableMono
    {
        private Button button = null;
        private TextMeshProUGUI infoText = null;
        private string roomName = null;
        private string id = null;

        private void Awake()
        {
            button = GetComponentInChildren<Button>();
            infoText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Init(string ID, string RoomName)
        {
            id = ID;
            roomName = RoomName;
        }

        public void EnterRoom()
        {
            Client.Instance.SendMessages("room", "join", id + "," + roomName);
        }

        public override void Reset()
        {
            
        }
    }
}
