using TMPro;
using UnityEngine;
using Newtonsoft.Json;

namespace Main
{
    public class CreateRoom : MonoBehaviour
    {
        private TMP_InputField field = null;

        private void Awake()
        {
            field = transform.parent.GetComponentInChildren<TMP_InputField>();
        }

        public void RequestCreate()
        {
            Client.RoomPacket rp = new Client.RoomPacket(field.text, "자기 id");
            string data = JsonConvert.SerializeObject(rp);

            Client.Instance.SendMessages("room", "create", data);
        }
    }
}
