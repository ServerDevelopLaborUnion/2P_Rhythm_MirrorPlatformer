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
            infoText.SetText($"HOST ID : {id} \nNAME : {roomName}");
        }

        public void EnterRoom()
        {
            Client.RoomPacket rp = new Client.RoomPacket(roomName, "자기 id");
            string data = JsonConvert.SerializeObject(rp);

            Client.Instance.SendMessages("room", "join", data);
        }

        public override void Reset()
        {
            
        }
    }
}
