using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DoText : MonoBehaviour
{
    [SerializeField] float moment = 0f, textDelay = 0.1f;
    [SerializeField] TextMeshProUGUI tmp = null;

    private void OnEnable()
    {
        StartCoroutine(StartText());
    }

    private IEnumerator StartText()
    {
        yield return new WaitForSeconds(moment);
        DOText(tmp, textDelay);
    }

    private void DOText(TextMeshProUGUI TMP, float Delay = 0.1f)
    {
        StartCoroutine(DoTextCoroutine(TMP, Delay));
    }

    private IEnumerator DoTextCoroutine(TextMeshProUGUI TMP, float Delay = 0.1f)
    {
        int i = 0;
        string arr = TMP.text;

        TMP.text = null;

        while(i < arr.Length - 3)
        {
            if(arr[i] == '\\')
            {
                switch(arr[i + 1])
                {
                    case 'n':
                        TMP.text += "\n";
                        break;
                    case '\\':
                        TMP.text += "\\";
                        break;
                }
                i += 2;
                continue;
            }

            TMP.text += arr[i++];
            yield return new WaitForSeconds(Delay);
        }

        yield return null;
    }
}
