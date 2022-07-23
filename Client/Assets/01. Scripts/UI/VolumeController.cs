using System;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class VolumeController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI valueText = null;
    [SerializeField] AudioSource source = null;
    private float value = 0;

    private void Start()
    {
        value = source.volume * 10;
        valueText.SetText($"{value}");
    }

    public void SetVolume(int amount)
    {
        value += amount;
        value = Mathf.Clamp(value, 0, 10);

        source.volume = value / 10f;

        valueText.SetText($"{value}");
        valueText.transform.DOShakePosition(0.5f, 3);
    }
}