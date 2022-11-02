using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeadSceneManager : MonoBehaviour
{
    [SerializeField] private BaseManager[] _managers;

    private void Awake()
    {
        //
    }

    private void Start()
    {
        InitializeManagers();
        InitializeGameField();
    }

    private void InitializeManagers()
    {
        foreach (var m in _managers)
            m.Initialize(this);
    }

    private void InitializeGameField()
    {
        var manager = GetManagerBy(ManagerType.Field) as FieldManager;
            manager?.BuildField();
    }

    public BaseManager GetManagerBy(ManagerType type)
    {
        return _managers.FirstOrDefault(m => m.Type == type);
    }

    private void OnDisable()
    {
        foreach (var m in _managers)
            m.CompleteExecution();
    }
}

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



