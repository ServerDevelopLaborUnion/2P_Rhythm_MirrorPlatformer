using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using Core;

namespace Main
{
    public class CreateRoom : MonoBehaviour
    {
        [SerializeField] int nameL = 10, pwL = 5;
        private TMP_InputField nameField = null, pwField = null;

        private void Awake()
        {
            nameField = transform.Find("NameField").GetComponent<TMP_InputField>();
            pwField = transform.Find("PasswordField").GetComponent<TMP_InputField>();
        }

        public void RequestCreate()
        {
            if(nameField.text.Length == 0 || nameField.text.Length > nameL) { TextSpawner.Instance.SpawnText($"Enter Between 1 and {nameL} Characters of Name!"); return; }
            else if(pwField.text.Length == 0 || pwField.text.Length > pwL) { TextSpawner.Instance.SpawnText($"Enter Between 1 and {pwL} Characters of Password!"); return; }
            else if(RoomManager.Instance.RoomList.ContainsKey(nameField.text)) { TextSpawner.Instance.SpawnText("Room With Current Name Already Exists"); return; }

            Client.RoomPacket rp = new Client.RoomPacket(nameField.text, pwField.text);

            Client.Instance.SendMessages("room", "createReq", rp);
        }
    }
}
