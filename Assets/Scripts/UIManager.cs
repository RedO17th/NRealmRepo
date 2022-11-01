using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseManager : MonoBehaviour
{
    public abstract void Initialize();
}

public class UIManager : BaseManager
{
    [SerializeField] private BaseWindow[] _windows;

    public override void Initialize()
    {
        InitializeWindows();
    }

    private void InitializeWindows()
    {
        foreach (var window in _windows)
        {
            window.Initialize(this);
            window.Deactivate();
        }
    }

    [ContextMenu("Enable WinWindow")]
    private void Enable()
    {
        _windows[0].Activate();
    }
}

public class BaseWindow : MonoBehaviour
{
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
