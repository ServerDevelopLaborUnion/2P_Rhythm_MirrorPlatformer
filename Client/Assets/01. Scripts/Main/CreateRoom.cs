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
            Client.Instance.SendMessages("room", "create", field.text);
        }
    }
}
