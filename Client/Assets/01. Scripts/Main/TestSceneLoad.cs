using UnityEngine.SceneManagement;
using UnityEngine;

namespace Main
{
    public class TestSceneLoad : MonoBehaviour
    {
        public void SceneLoad(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
