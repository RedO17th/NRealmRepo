using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayWindow : BaseWindow
{
    public void SendAnExitEvent() => _uiManager.ProcessingExitEvent();
}
