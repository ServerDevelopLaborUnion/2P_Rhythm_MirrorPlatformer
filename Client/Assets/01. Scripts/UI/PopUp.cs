using DG.Tweening;
using UnityEngine;
using Core;

public class PopUp : Buttons
{
    [SerializeField] float duration;

    public void DoPopUp(RectTransform rt)
    {
        rt.gameObject.SetActive(true);
        rt.localScale = Vector3.zero;
        rt.DOScale(Vector3.one, duration);

        Reset();
    }

    public void DoPopDown(RectTransform rt)
    {
        rt.DOScale(Vector3.zero, duration).OnComplete(() => {
            rt.gameObject.SetActive(false);
            rt.localScale = Vector3.one;
        });

        Reset();
    }
}
