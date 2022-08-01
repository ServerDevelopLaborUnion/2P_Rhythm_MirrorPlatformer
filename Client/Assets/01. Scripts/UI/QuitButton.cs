using UnityEngine;
using Core;

public class QuitButton : Buttons
{
    public void DoQuit()
    {
        Reset();
        Application.Quit();
    }
}
