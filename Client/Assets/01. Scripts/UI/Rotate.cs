using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class Rotate : Buttons
{
    [SerializeField] float duration;
    [SerializeField] RectTransform rt;
    [SerializeField] List<RectTransform> rts = new List<RectTransform>();
    private bool onMoving = false;
    private int currentPanel = 0;

    private void Update()
    {
        if (Input.GetButtonDown("Jump")) DoRotateLeft();
    }

    public void DoRotateLeft()
    {
        if (onMoving) return;
        Sequence seq = DOTween.Sequence();

        if (currentPanel == 4) currentPanel = 0;

        currentPanel++;

        switch (currentPanel)
        {
            case 1:
                DoRotate(rts[0], rts[1]);
                break;
            case 2:
                DoRotate(rts[1], rts[2]);
                break;
            case 3:
                DoRotate(rts[2], rts[3]);
                break;
            case 4:
                DoRotate(rts[3], rts[0]);
                break;
        }
        onMoving = true;

        seq.Append(rt
            .DORotate(transform.eulerAngles + new Vector3(0, 90, 0), duration))
            .SetEase(Ease.Linear)
            .OnComplete(() => onMoving = false);

        Reset();
    }

    private void DoRotate(RectTransform current, RectTransform next)
    {
        foreach (RectTransform rt in rts)
            switch (Mathf.Floor(rt.eulerAngles.y))
            {
                case 0:
                    rt.DOLocalMoveX(960, duration);
                    rt.transform.SetSiblingIndex(0);
                    break;
                case 90:
                    rt.DOLocalMoveX(0, duration);
                    rt.transform.SetSiblingIndex(1);
                    break;
                case 180:
                    rt.DOLocalMoveX(-960, duration);
                    rt.transform.SetSiblingIndex(2);
                    break;
                case 270:
                    rt.DOLocalMoveX(0, duration);
                    rt.transform.SetSiblingIndex(3);
                    break;
            }
        current.DOScale(new Vector3(0.3f, 0.3f), duration / 2);
        next.DOScale(Vector3.one, duration / 2);
    }
}
