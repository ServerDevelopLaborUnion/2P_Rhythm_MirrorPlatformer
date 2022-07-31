using DG.Tweening;
using UnityEngine;

public class IntroText : MonoBehaviour
{
    [SerializeField] float duration;
    private RectTransform rt = null;

    private Sequence seq;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        DoIntro();
    }

    private void DoIntro()
    {
        seq = DOTween.Sequence();

        seq.Append(rt.DOScale(new Vector3(1.05f, 1.05f), duration)).SetEase(Ease.OutCubic);
        seq.Append(rt.DOScale(new Vector3(0.95f, 0.95f), duration)).SetEase(Ease.Linear);
        seq.SetLoops(-1);
    }

    private void OnDisable()
    {
        seq.Kill();
    }
}
