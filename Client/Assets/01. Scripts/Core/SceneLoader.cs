using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance = null;

        private void Awake()
        {
            if(Instance != null) { Debug.Log($"Multiple SceneLoader Instance is Running, Destroy This"); Destroy(gameObject); }
            if(Instance == null) Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
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
