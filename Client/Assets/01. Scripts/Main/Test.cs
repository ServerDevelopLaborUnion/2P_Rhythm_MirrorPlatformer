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

        public void AddMessage(string sceneName)
        {
            Client.Instance.SendMessages("game", "addScene", sceneName);
        }

        public void LoadMessage(string sceneName)
        {
            Client.Instance.SendMessages("game", "loadScene", sceneName);
        }

        public void UnloadMessage(string sceneName)
        {
            Client.Instance.SendMessages("game", "unLoadScene", sceneName);
        }

        public void LoadScene(string sceneName)
        {
            Debug.Log($"Load Scene : {sceneName}");
            SceneManager.LoadScene(sceneName);
        }

        public void AddScene(string sceneName)
        {
            Debug.Log($"Add Scene : {sceneName}");
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        public void RemoveScene(string sceneName)
        {
            Debug.Log($"Remove Scene : {sceneName}");
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}
