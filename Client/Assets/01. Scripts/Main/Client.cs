using System;
using WebSocketSharp;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Main
{
    public class Client : MonoBehaviour
    {
        public static Client Instance = null;

        [SerializeField] string IP = "127.0.0.1";
        [SerializeField] string Port = "8080";
        private object locker = new object();
        private Queue<Action> actions = new Queue<Action>();
        public WebSocket server;

        public class Packet
        {
            [JsonProperty("l")] public string Locate;
            [JsonProperty("")] public string Payload;
            [JsonProperty("v")] public string Value;

            public Packet(string locate, string payload, string value)
            {
                Locate = locate;
                Payload = payload;
                Value = value;
            }
        }

        public class Lobby
        {
            [JsonProperty("n")] public string Name;
            [JsonProperty("i")] public string ID;
            [JsonProperty("p")] public string PassWord;

            public Lobby(string name, string id, string passWord)
            {
                Name = name;
                ID = id;
                PassWord = passWord;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(transform.root.gameObject);

            if (Instance == null) Instance = this;
        }

        private void Start()
        {
            server = new WebSocket("ws://" + IP + ":" + Port);

            server.OnOpen += (sender, e) =>
            {
                Debug.Log($"client connected on port ${server.Url}");
            };

            server.OnMessage += GetMessages;

            server.Connect();
        }

        private void GetMessages(object sender, MessageEventArgs e)
        {
            lock (locker)
            {
                if (e.Data.Length == 0 || e.Data == null) return;
                Packet p = JsonConvert.DeserializeObject<Packet>(e.Data);
                actions.Enqueue(() => Debug.Log($"{p.Locate}"));
                switch (p.Locate)
                {
                    case "game":
                        actions.Enqueue(() => GameData(p.Value) );
                        break;
                    case "room":
                        actions.Enqueue(() => RoomData(p.Value) );
                        break;
                    case "error":
                        actions.Enqueue(() => Debug.Log($"{p.Payload}"));
                        break;
                }
            }
        }

        private void RoomData(string value)
        {
            
        }

        private void GameData(string value)
        {
            switch (value)
            {
                case "loadScene":
                    actions.Enqueue(() => Test.Instance.Load());
                    break;
                case "unLoadScene":
                    actions.Enqueue(() => Test.Instance.UnLoad());
                    break;
                case "slide":
                    actions.Enqueue(() => P2Control.Instance.SetEvent(P2Control.Events.Slide));
                    break;
                case "jump":
                    actions.Enqueue(() => P2Control.Instance.SetEvent(P2Control.Events.Jump));
                    break;
            }
        }

        private void Update()
        {
            while (actions.Count > 0)
            {
                Debug.Log($"Action gonna be dequeuing");
                actions.Dequeue()?.Invoke();
            }
        }

        public void SendMessages(string locate, string payload, string value)
        {
            Packet packet = new Packet(locate, payload, value);
            string JSON = JsonConvert.SerializeObject(packet);
            server.Send(JSON);
        }

        private void OnApplicationQuit()
        {
            server.Close();
        }
    }
}
