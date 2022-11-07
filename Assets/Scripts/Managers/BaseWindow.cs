using UnityEngine;

public class BaseWindow : MonoBehaviour
{
    [SerializeField] protected WindowType _type;

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
