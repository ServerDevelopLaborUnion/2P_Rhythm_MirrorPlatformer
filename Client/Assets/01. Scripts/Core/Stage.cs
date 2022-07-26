using UnityEngine;

namespace Core
{
    public class Stage : MonoBehaviour
    {
        [SerializeField] string musicName = null;
        public int Ortho = 7;
        private GameObject map, P1, P2;

        public void Init()
        {
            map = GameObject.Find("Map");
            P1 = GameObject.Find("Player1");
            P2 = GameObject.Find("Player2");
            //AudioManager.Instance.PlayClip(musicName);
        }

        public void Reset()
        {
            map.transform.localPosition = Vector3.zero;
        }
    }
}
