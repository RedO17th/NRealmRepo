using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private GamePlayWindow _gamePlayWindow = null;
    private Button _btn = null;

    private void Awake()
    {
        _btn = GetComponent<Button>();
    }

    public void Initialize(Component manager)
    {
        _gamePlayWindow = manager as GamePlayWindow;
    }

    public void Activate()
    {
        _btn.onClick.AddListener(_gamePlayWindow.SendAnExitEvent);
    }

    public void Deactivate()
    {
        _btn.onClick.RemoveListener(_gamePlayWindow.SendAnExitEvent);
    }
}
