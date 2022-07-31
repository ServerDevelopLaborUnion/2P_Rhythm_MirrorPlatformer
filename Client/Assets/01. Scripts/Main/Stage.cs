using System.Collections;
using UnityEngine;
using Core;

namespace Main
{
    public class Stage : MonoBehaviour
    {
        [SerializeField] string musicName = null, nextStage = null;
        [SerializeField] float endPoint = 10f;
        public int Ortho { get; set; } = 7;
        public string NextStage => nextStage;
        public Transform map, P1, P2;

        public void Init()
        {
            //AudioManager.Instance.PlayBGM(musicName);

            map = transform.Find("Map");
            P1 = transform.Find("Player1");
            P2 = transform.Find("Player2");

            StartCoroutine(FinishRace());
        }

        public IEnumerator FinishRace()
        {
            yield return new WaitUntil(() => Mathf.Abs(map.position.x) >= endPoint);
            Client.Instance.SendMessages("game", "clear", "");
            //AudioManager.Instance.PauseBGM();
            map.GetComponent<MoveTo>().enabled = false;
        }

        public void Reset()
        {
            //AudioManager.Instance.PlayBGM(musicName);
            map.localPosition = Vector3.zero;
        }
    }
}