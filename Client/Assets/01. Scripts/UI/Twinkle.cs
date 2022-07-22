using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Core;

public class Twinkle : Buttons
{
    [SerializeField] float duration, endVal;

    public void DoTwinkle(Image image)
    {
        image.DOFade(endVal, duration);

        Reset();
    }
}
