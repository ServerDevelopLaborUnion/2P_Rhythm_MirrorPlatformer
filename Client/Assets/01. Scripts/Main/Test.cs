using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main
{
    public class Test : MonoBehaviour
    {
        public static Test Instance = null;
        [SerializeField] string sceneName = null;

        private void Awake()
        {
            if(Instance == null) Instance = this;
            if(Instance != null) { Debug.Log($"Multiple Test Instance is Running, Destroy This"); Destroy(gameObject); }
            DontDestroyOnLoad(transform.root.gameObject);
        }

        public void LoadMessage()
        {
            Client.Instance.SendMessages("game", "system", "loadScene");
        }

        public void UnloadMessage()
        {
            Client.Instance.SendMessages("game", "system", "unLoadScene");
        }

        public void Load()
        {
            Debug.Log($"lets load the scene");
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        public void UnLoad()
        {
            Debug.Log($"lets unload the scene");
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}
