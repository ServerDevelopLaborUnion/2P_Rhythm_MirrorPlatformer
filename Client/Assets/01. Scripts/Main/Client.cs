using System;
using WebSocketSharp;
using UnityEngine;
using Newtonsoft.Json;
using Core;
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
            [JsonProperty("t")] public string Type;
            [JsonProperty("v")] public string Value;

            public Packet(string locate, string type, string value)
            {
                Locate = locate;
                Type = type;
                Value = value;
            }
        }

        private void Awake()
        {
            if(Instance != null) { Debug.Log($"Multiple Client Instance is Running, Destroy This"); Destroy(gameObject); }
            if(Instance == null) { Instance = this; DontDestroyOnLoad(transform.root.gameObject); }
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

            if(!server.IsAlive) Debug.Log($"Server is Not Connected");
        }

        private void GetMessages(object sender, MessageEventArgs e)
        {
            lock (locker)
            {
                if (e.Data.Length == 0 || e.Data == null) return;
                Packet p = JsonConvert.DeserializeObject<Packet>(e.Data);
                actions.Enqueue(() => Debug.Log($"Type:{p.Type}"));
                switch (p.Locate)
                {
                    case "game":
                        actions.Enqueue(() => GameData(p) );
                        break;
                    case "room":
                        actions.Enqueue(() => RoomData(p) );
                        break;
                    case "error":
                        actions.Enqueue(() => Debug.Log($"{p.Locate}"));
                        break;
                }
            }
        }
#region 룸
        private void RoomData(Packet p)
        {
            switch(p.Type)
            {
                case "create":
                    actions.Enqueue(() => RoomManager.Instance.AddRoom(p.Value) );
                    break;
                case "createErr":
                    actions.Enqueue(() => TextSpawner.Instance.SpawnText(p.Value) );
                    break;
                case "createRes":
                    actions.Enqueue(() => SceneLoader.Instance.LoadScene("INGAME") );
                    break;
                case "join":
                    actions.Enqueue(() => InGameManager.Instance.WaitingPanel.SetActive(false) );
                    break;
                case "joinErr":
                    actions.Enqueue(() => TextSpawner.Instance.SpawnText(p.Value) );
                    break;
                case "joinRes":
                    actions.Enqueue(() => SceneLoader.Instance.LoadScene("INGAME") );
                    break;
                case "quit":
                    actions.Enqueue(() => {
                        SceneLoader.Instance.LoadScene("INTRO");
                        TextSpawner.Instance.SpawnText("Partner is Disconnected With Room");
                    });
                    break;
                case "quitRes":
                    actions.Enqueue(() => {
                        SceneLoader.Instance.LoadScene("INTRO");
                        TextSpawner.Instance.SpawnText("Disconnecting With Room");
                    });
                    break;
                case "init":
                    actions.Enqueue(() => RoomManager.Instance.AddRoom(p.Value) );
                    break;
                case "roomDel":
                    actions.Enqueue(() => RoomManager.Instance.RemoveRoom(p.Value) );
                    break;
                case "error":
                    actions.Enqueue(() => Debug.Log($"{p.Type}") );
                    break;
            }
        }
#endregion
#region 인게임
        private void GameData(Packet p)
        {
            switch (p.Type)
            {
                case "input":
                    actions.Enqueue(() => InputData(p));
                    break;
                case "start":
                    actions.Enqueue(() => InGameManager.Instance.LoadStage(p.Value) );
                    break;
                case "clear":
                    actions.Enqueue(() => InGameManager.Instance.StageClear() );
                    break;
                case "return":
                    actions.Enqueue(() => InGameManager.Instance.UnloadStage() );
                    break;
                case "dead":
                    actions.Enqueue(() => InGameManager.Instance.currentStage.Reset() );
                    break;
                case "error":
                    actions.Enqueue(() => Debug.Log($"{p.Type}") );
                    break;
            }
        }

        private void InputData(Packet p)
        {
            switch (p.Value)
            {

                case "slide":
                    actions.Enqueue(() => P2Control.Instance.SetEvent(P2Control.Events.Slide));
                    break;
                case "jump":
                    actions.Enqueue(() => P2Control.Instance.SetEvent(P2Control.Events.Jump));
                    break;
                case "error":
                    actions.Enqueue(() => Debug.Log($"{p.Value}") );
                    break;
            }
        }
#endregion
        private void Update()
        {
            while (actions.Count > 0)
            {
                Debug.Log($"Action gonna be dequeuing");
                actions.Dequeue()?.Invoke();
            }
        }

        public void SendMessages(string locate, string type, string value)
        {
            if(!server.IsAlive) { Debug.Log($"Server is Not Connected"); return; }

            Packet packet = new Packet(locate, type, value);
            string JSON = JsonConvert.SerializeObject(packet);
            server.Send(JSON);
        }

        private void OnApplicationQuit()
        {
            server.Close();
        }
    }
}
