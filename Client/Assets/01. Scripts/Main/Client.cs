using System.ComponentModel.Design.Serialization;
using System.Net.Sockets;
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
        [SerializeField] char prefix = ':';
        private object locker = new object();
        private Queue<Action> actions = new Queue<Action>();
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
                actions.Enqueue(() => Debug.Log($"{p.Type}"));
                switch (p.Type)
                {
                    case "slide":
                        actions.Enqueue(() => P2Control.Instance.SetEvent(P2Control.Events.Slide));
                        break;
                    case "jump":
                        actions.Enqueue(() => P2Control.Instance.SetEvent(P2Control.Events.Jump));
                        break;
                    case "error":
                        actions.Enqueue(() => Debug.Log($"{p.Payload}"));
                        break;
                }
            }
        }

        private void Update()
        {
            while (actions.Count > 0)
            {
                actions.Dequeue();
            }
        }

        public void SendMessages(string type, string payload)
        {
            Packet packet = new Packet(type, payload);
            string JSON = JsonConvert.SerializeObject(packet);
            server.Send(JSON);
        }

        private void OnApplicationQuit()
        {
            server.Close();
        }
    }
}
