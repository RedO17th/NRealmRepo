using UnityEngine;

public enum ManagerType { None = -1, Field, UI }

public class BaseManager : MonoBehaviour
{
    [SerializeField] private ManagerType _managerType = ManagerType.None;

    public ManagerType Type => _managerType;

    protected HeadSceneManager _sceneManager = null;

    public virtual void Initialize(HeadSceneManager manager)
    {
        _sceneManager = manager;
    }

    public virtual void CompleteExecution() { }
}



