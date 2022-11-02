using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum WindowType { None = -1, Win }

public class UIManager : BaseManager
{
    [SerializeField] private BaseWindow[] _windows;

    private FieldManager _fieldManager = null;

    public override void Initialize(HeadSceneManager manager)
    {
        base.Initialize(manager);

        InitializeWindows();

        SetTheEventToEndTheGame();
    }

    private void SetTheEventToEndTheGame()
    {
        _fieldManager = _sceneManager.GetManagerBy(ManagerType.Field) as FieldManager;
        _fieldManager.OnFieldWasAssembled += ActivateWindowByEndGame;
    }

    private void InitializeWindows()
    {
        foreach (var window in _windows)
        {
            window.Initialize(this);
            window.Deactivate();
        }
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
            window.Deactivate();
    }
}

public class BaseWindow : MonoBehaviour
{
    [SerializeField] private WindowType _type;

    public WindowType Type => _type;

    private UIManager uiManager = null;

    public virtual void Initialize(UIManager manager) 
    {
        uiManager = manager;
    }

    public virtual void Activate() => gameObject.SetActive(true);

    protected virtual void OnEnable() => ProcessWindowActivation();
    protected virtual void ProcessWindowActivation() { }

    public virtual void Deactivate() => gameObject.SetActive(false);

    protected virtual void OnDisable() => ProcessWindowDeactivation();
    protected virtual void ProcessWindowDeactivation() { }

}
