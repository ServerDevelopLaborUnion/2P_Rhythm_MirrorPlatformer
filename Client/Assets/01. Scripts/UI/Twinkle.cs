using System.Security.Cryptography.X509Certificates;
using System.Diagnostics.Contracts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Twinkle : MonoBehaviour
{
    [SerializeField] float duration, endVal;

    public void DoTwinkle(Image image)
    {
        image.DOFade(endVal, duration);
    }
}
