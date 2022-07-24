using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main
{
    public class Test : MonoBehaviour
    {
        public static Test Instance = null;

        private void Awake()
        {
            if(Instance == null) Instance = this;
            if(Instance != null) { Debug.Log($"Multiple Test Instance is Running, Destroy This"); Destroy(gameObject); }
            DontDestroyOnLoad(transform.root.gameObject);
        }

        public void LoadMessage(string sceneName)
        {
            Client.Instance.SendMessages("game", "loadScene", sceneName);
        }

        public void UnloadMessage(string sceneName)
        {
            Client.Instance.SendMessages("game", "unLoadScene", sceneName);
        }

        public void Load(string sceneName)
        {
            Debug.Log($"lets load the scene");
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        public void UnLoad(string sceneName)
        {
            Debug.Log($"lets unload the scene");
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}
