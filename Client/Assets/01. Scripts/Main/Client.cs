using WebSocketSharp;
using UnityEngine;
using Newtonsoft.Json;

namespace Main
{
    public class Client : MonoBehaviour
    {
        public static Client Instance = null;

        [SerializeField] string IP = "127.0.0.1";
        [SerializeField] string Port = "8080";
        [SerializeField] char prefix = ':';
        private object locker = new object();
        public WebSocket server;

        public class Packet
        {
            [JsonProperty("type")] public string Type;
            [JsonProperty("payload")] public string Payload;

            public Packet(string type, string payload)
            {
                Type = type;
                Payload = payload;
            }
        }

        private void Awake()
        {
            if(Instance == null) Instance = this;
        }

        private void Start()
        {
            server = new WebSocket("ws://" + IP + ":" + Port);

            server.OnOpen += (sender, e) => {
                Debug.Log($"client connected on port ${server.Url}");
            };

            server.OnMessage += GetMessages;

            server.Connect();
        }

        private void GetMessages(object sender, MessageEventArgs e)
        {
            lock(locker)
            {
                if(e.Data.Length == 0 || e.Data == null) return;

                Packet p = JsonConvert.DeserializeObject<Packet>(e.Data);
                
                switch(p.Type)
                {
                    case "slide":
                        P2Control.Instance.SetEvent(P2Control.Events.Slide);
                        break;
                    case "jump":
                        P2Control.Instance.SetEvent(P2Control.Events.Jump);
                        break;
                    case "error":
                        Debug.Log($"{p.Payload}");
                        break;
                }
            }
        }

        public void SendMessages(string type, string payload)
        {
            Packet packet = new Packet(type, payload);
            string JSON =  JsonConvert.SerializeObject(packet);
            server.Send(JsonUtility.ToJson(JSON));
        }

        private void OnApplicationQuit()
        {
            server.Close();
        }
    }
}
