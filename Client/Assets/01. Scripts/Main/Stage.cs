using System.Collections;
using UnityEngine;

namespace Main
{
    public class Stage : MonoBehaviour
    {
        [SerializeField] string musicName = null, nextStage = null;
        [SerializeField] float endPoint = 10f;
        public int Ortho { get; set; } = 7;
        public Transform map, P1, P2;

        public void Init()
        {
            //AudioManager.Instance.PlayClip(musicName);

            map = transform.Find("Map");
            P1 = transform.Find("Player1");
            P2 = transform.Find("Player2");

            StartCoroutine(FinishRace());
        }

        public IEnumerator FinishRace()
        {
            yield return new WaitUntil(() => Mathf.Abs(map.position.x) >= endPoint);
            Client.Instance.SendMessages("game", "finish", "");
            InGameManager.Instance.UnlockStage(nextStage);
        }

        public void Reset()
        {
            map.localPosition = Vector3.zero;
        }
    }
}
