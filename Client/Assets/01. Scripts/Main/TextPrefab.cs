using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Core;

namespace Main
{
    public class TextPrefab : PoolableMono
    {
        [SerializeField] float duration, targetVal;
        private TextMeshProUGUI tmp = null;

        public override void Reset()
        {
            tmp = GetComponent<TextMeshProUGUI>();
            transform.SetParent(TextSpawner.Instance.Canvas);
            transform.localScale = Vector3.one;
            transform.localPosition = new Vector3(0, targetVal);
            tmp.alpha = 1;

            StartCoroutine(DoFade(0, duration));
            transform.DOLocalMoveY(transform.localPosition.y + targetVal, duration).OnComplete(() => PoolManager.Instance.Push(this));
        }

        public void Init(string content)
        {
            tmp.SetText(content);
        }

        private IEnumerator DoFade(float endVal, float duration)
        {
            float percent = 1;
            while(percent >= endVal)
            {
                tmp.alpha = Mathf.Lerp(0, 1, percent);
                percent -= Time.deltaTime / duration;
                yield return null;
            }
            yield return null;
        }
    }
}

