using System.Numerics;
using Core;
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
            DataManager.Instance.ud.isHost = false;
            SceneLoader.Instance.LoadScene("INGAME");
        }

        public override void Reset()
        {
            
        }
    }
}
