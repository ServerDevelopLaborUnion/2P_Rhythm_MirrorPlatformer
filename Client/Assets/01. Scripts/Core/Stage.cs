using UnityEngine;

namespace Core
{
    public class Stage : MonoBehaviour
    {
        [SerializeField] string musicName = null;

        public void Init()
        {
            //AudioManager.Instance.PlayClip(musicName);
        }
    }
}
