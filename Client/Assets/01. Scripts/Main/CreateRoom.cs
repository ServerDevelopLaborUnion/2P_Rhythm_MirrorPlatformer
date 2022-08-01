using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using Core;

namespace Main
{
    public class CreateRoom : MonoBehaviour
    {
        private TMP_InputField nameField = null, pwField = null;

        private void Awake()
        {
            nameField = transform.Find("NameField").GetComponent<TMP_InputField>();
            pwField = transform.Find("PasswordField").GetComponent<TMP_InputField>();
        }

        public void RequestCreate()
        {
            if(nameField.text.Length == 0 || nameField.text.Length > 11) { TextSpawner.Instance.SpawnText("Enter Between 1 and 10 Characters of Name!"); return; }
            else if(pwField.text.Length == 0 || pwField.text.Length > 6) { TextSpawner.Instance.SpawnText("Enter Between 1 and 5 Characters of Password!"); return; }
            else if(RoomManager.Instance.RoomList.ContainsKey(nameField.text)) { TextSpawner.Instance.SpawnText("Room With Current Name Already Exists"); return; }

            Client.RoomPacket rp = new Client.RoomPacket(nameField.text, pwField.text);
            string JSON = JsonConvert.SerializeObject(rp);

            Client.Instance.SendMessages("room", "createReq", JSON);
        }
    }
}
