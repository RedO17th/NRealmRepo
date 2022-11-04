using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class HeadSceneManager : MonoBehaviour
{
    [SerializeField] private BaseManager[] _managers;

    private UIManager _uiManager = null;

    private void Start()
    {
        InitializeManagers();
        InitializeGameField();
        SetResultWindowTracking();
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

    private void SetResultWindowTracking()
    {
        _uiManager = GetManagerBy(ManagerType.UI) as UIManager;
        _uiManager.OnResultWindowWasShowed += ProcessLevelCompletion;
    }

    public BaseManager GetManagerBy(ManagerType type)
    {
        return _managers.FirstOrDefault(m => m.Type == type);
    }

    private void ProcessLevelCompletion()
    {
        CompleteExecutionOfManagers();

        UnitySceneManager.LoadScene(UnitySceneManager.GetActiveScene().buildIndex);
    }

    private void CompleteExecutionOfManagers()
    {
        foreach (var m in _managers)
            m.CompleteExecution();

        _uiManager.OnResultWindowWasShowed -= ProcessLevelCompletion;
        _uiManager = null;
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


