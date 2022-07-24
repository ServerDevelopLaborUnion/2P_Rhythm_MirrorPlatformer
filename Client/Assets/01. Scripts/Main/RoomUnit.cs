using Core;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;

namespace Main
{
    public class RoomUnit : PoolableMono
    {
        private Button button = null;
        private TextMeshProUGUI infoText = null;
        private string roomName = null;

        private void Awake()
        {
            button = GetComponentInChildren<Button>();
            infoText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Init(string RoomName)
        {
            roomName = RoomName;
            infoText.SetText($"NAME : {roomName}");
        }

        public void EnterRoom()
        {
            Client.Instance.SendMessages("room", "join", roomName);
        }

        public override void Reset()
        {
            
        }
    }
}
