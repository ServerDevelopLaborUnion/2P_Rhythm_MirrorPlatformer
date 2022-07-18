using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SEH00N
{
    public class Test : MonoBehaviour
    {
        [SerializeField] string sceneName = null;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
                Load();
            if(Input.GetKeyDown(KeyCode.S))
                UnLoad();
        }

        public void Load()
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        public void UnLoad()
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}
