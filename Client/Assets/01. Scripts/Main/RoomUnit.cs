using UnityEngine;
using Core;
using TMPro;

namespace Main
{
    public class RoomUnit : PoolableMono
    {
        private TextMeshProUGUI infoText = null;
        private TMP_InputField field = null;
        private Transform passwordPanel = null;
        public string roomName { get; set; }
        public string password { get; set; }

        private void Awake()
        {
            passwordPanel = transform.Find("PasswordPanel");
            infoText = GetComponentInChildren<TextMeshProUGUI>();
            field = passwordPanel.GetComponentInChildren<TMP_InputField>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
                CloseFolder();
        }

        public void Init(string RoomName, string Password)
        {
            password = Password;
            roomName = RoomName;
            infoText.SetText(roomName);
        }

        public void CloseFolder()
        {
            if(!passwordPanel.gameObject.activeSelf) return;

            passwordPanel.gameObject.SetActive(false);
        }

        public void EnterRoom()
        {
            if(field.text != password)
            {
                TextSpawner.Instance.SpawnText("Wrong Password!");
                return;
            }

            Client.Instance.SendMessages("room", "joinReq", roomName);
        }

        public override void Reset()
        {
            
        }
    }
}
