using UnityEngine.UI;
using Core;
using UnityEngine;
using System.Collections.Generic;

public class Skin : Buttons
{
    [SerializeField] List<Sprite> skinList = new List<Sprite>();
    [SerializeField] Image uiImage = null;
    private int index = 0;

    private void Awake()
    {
        index = PlayerPrefs.GetInt("skinIndex", 0);
    }

    private void Start()
    {
        uiImage.sprite = skinList[index];
        DataManager.Instance.ud.skin = skinList[index];
    }

    public void Change(int amount)
    {
        Reset();

        index += amount;
        if(index >= skinList.Count) index = 0;
        else if(index < 0) index = skinList.Count - 1;

        uiImage.sprite = skinList[index];
        DataManager.Instance.ud.skin = skinList[index];

        PlayerPrefs.SetInt("skinIndex", index);
    }
}
