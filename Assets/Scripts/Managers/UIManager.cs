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
            window.Activate();
    }

    private void ActivateWindowByEndGame()
    {
        var window = GetWindowBy(WindowType.Win);

        if (window)
        {
            window.Activate();
            window.OnActivated += ProcessWindowActivation;
        }
    }

    private BaseWindow GetWindowBy(WindowType type)
    {
        return _windows.FirstOrDefault(w => w.Type == type);
    }

    private void ProcessWindowActivation(WindowType type)
    {
        switch (type)
        {
            case WindowType.Win: { ProcessingWinWindowActivation(); break; }
        }
    }

    private void ProcessingWinWindowActivation() => OnResultWindowWasShowed?.Invoke();

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
        {
            if (window)
            {
                window.OnActivated -= ProcessWindowActivation;
                window.Deactivate();
            }
        }  
    }

    public void ProcessingExitEvent() => OnOutputEvent?.Invoke();
}

public class BaseWindow : MonoBehaviour
{
    [SerializeField] protected WindowType _type;

    public virtual event Action<WindowType> OnActivated;

    public WindowType Type => _type;

    protected UIManager _uiManager = null;

    public virtual void Initialize(UIManager manager) 
    {
        _uiManager = manager;
    }

    public virtual void Activate() => gameObject.SetActive(true);

    protected virtual void OnEnable() => ProcessWindowActivation();
    protected virtual void ProcessWindowActivation() { }

    public virtual void Deactivate() => gameObject.SetActive(false);

    protected virtual void OnDisable() => ProcessWindowDeactivation();
    protected virtual void ProcessWindowDeactivation() { }

}
