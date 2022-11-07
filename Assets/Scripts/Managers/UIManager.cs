using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum WindowType { None = -1, Win, Gameplay }

public class UIManager : BaseManager
{
    [SerializeField] private BaseWindow[] _windows;

    public event Action OnResultWindowWasShowed;
    public event Action OnOutputEvent;

    private FieldManager _fieldManager = null;

    public override void Initialize(HeadSceneManager manager)
    {
        base.Initialize(manager);

        InitializeWindows();

        SetTheEventToEndTheGame();

        EnableGamePlayWindow();
    }

    private void InitializeWindows()
    {
        foreach (var window in _windows)
        {
            window.Initialize(this);
            window.Deactivate();
        }
    }

    private void SetTheEventToEndTheGame()
    {
        _fieldManager = _sceneManager.GetManagerBy(ManagerType.Field) as FieldManager;
        _fieldManager.OnFieldWasAssembled += ActivateWindowByEndGame;
    }

    private void EnableGamePlayWindow()
    {
        var window = GetWindowBy(WindowType.Gameplay);
            window?.Activate();
    }

    private void ActivateWindowByEndGame()
    {
        var window = GetWindowBy(WindowType.Win);
            window?.Activate();
    }

    private BaseWindow GetWindowBy(WindowType type)
    {
        return _windows.FirstOrDefault(w => w.Type == type);
    }

    public void ProcessingWinWindowActivation() => OnResultWindowWasShowed?.Invoke();
    public void ProcessingExitEvent() => OnOutputEvent?.Invoke();

    public override void CompleteExecution()
    {
        ProcessWindowsDeactivation();

        if (_fieldManager)
            _fieldManager.OnFieldWasAssembled -= ActivateWindowByEndGame;
        
        _fieldManager = null;
    }

    private void ProcessWindowsDeactivation()
    {
        foreach (var window in _windows)
            window?.Deactivate();
    }
}
