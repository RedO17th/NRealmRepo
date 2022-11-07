using UnityEngine;

public class GamePlayWindow : BaseWindow
{
    [SerializeField] private ExitButton _exitButton;

    protected override void ProcessWindowActivation()
    {
        _exitButton.Initialize(this);
        _exitButton.Activate();
    }

    public void SendAnExitEvent() => _uiManager.ProcessingExitEvent();

    protected override void ProcessWindowDeactivation()
    {
        _exitButton.Deactivate();
    }
}
