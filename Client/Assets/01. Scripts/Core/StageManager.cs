using UnityEngine;

namespace Core
{
    public class StageManager : MonoBehaviour
    {
        public static StageManager Instance = null;

        private void Awake()
        {
            if(Instance != null) { Debug.Log($"Multiple StageManager Instance is Running, Destroy This"); Destroy(gameObject); }
            if(Instance == null) { Instance = this; DontDestroyOnLoad(transform.root.gameObject); }
        }

        public void UnlockStage(string name)
        {
            DataManager.Instance.sd.unlockedStage.Add(name);
        }

        public void LoadStage(string index)
        {
            Stage stage = Resources.Load<Stage>("Stages/Stage" + index);
            Instantiate(stage, Vector3.zero, Quaternion.identity);
            stage.Init();
        }
    }
}
