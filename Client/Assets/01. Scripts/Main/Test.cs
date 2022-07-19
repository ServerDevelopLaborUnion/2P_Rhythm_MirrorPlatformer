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
        }

        public void LoadMessage()
        {
            Client.Instance.SendMessages("loadScene", "loadScene", "loadScene");
        }

        public void UnloadMessage()
        {
            Client.Instance.SendMessages("unLoadScene", "unLoadScene", "loadScene");
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
