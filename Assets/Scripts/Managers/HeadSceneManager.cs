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

        SetNecessaryEventsFromUIManager();
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

    private void SetNecessaryEventsFromUIManager()
    {
        _uiManager = GetManagerBy(ManagerType.UI) as UIManager;
        _uiManager.OnResultWindowWasShowed += ProcessLevelCompletion;
        _uiManager.OnOutputEvent += ProcessingTheExitFromTheApplication;
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
        _uiManager.OnOutputEvent -= ProcessingTheExitFromTheApplication;
        _uiManager = null;
    }

    private void ProcessingTheExitFromTheApplication()
    {
        CompleteExecutionOfManagers();

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif

    }
}



