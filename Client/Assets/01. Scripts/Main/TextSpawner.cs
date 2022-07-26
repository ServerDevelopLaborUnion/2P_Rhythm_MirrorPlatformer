using UnityEngine;
using Core;

namespace Main
{
    public class TextSpawner : MonoBehaviour
    {
        public static TextSpawner Instance = null;

        [SerializeField] PoolableMono textPrefab;
        public Transform Canvas { get; set; } = null;

        private void Awake()
        {
            if (Instance != null) { Debug.Log($"Multiple TextSpawner Instance is Running, Destroy This"); Destroy(gameObject); }
            if (Instance == null) { Instance = this; DontDestroyOnLoad(transform.root.gameObject); }

            Canvas = GameObject.Find("TextCanvas").transform;
            DontDestroyOnLoad(Canvas.root.gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
                SpawnText("Host is Disconnecting With Game");
        }

        public void SpawnText(string content)
        {
            TextPrefab temp = PoolManager.Instance.Pop(textPrefab) as TextPrefab;
            temp.Init(content);
        }
    }
}
