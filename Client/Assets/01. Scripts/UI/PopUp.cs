using DG.Tweening;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] float duration;

    public void DoPopUp(RectTransform rt)
    {
        rt.gameObject.SetActive(true);
        rt.localPosition = Vector3.zero;
        rt.DOScale(Vector3.one, duration);
    }

    public void DoPopDown(RectTransform rt)
    {
        rt.DOScale(Vector3.zero, duration).OnComplete(() => {
            rt.gameObject.SetActive(true);
        });
    }
}
