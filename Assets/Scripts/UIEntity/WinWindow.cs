using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class WinWindow : BaseWindow
{
    [SerializeField] private string _headerText;
    [SerializeField] private TextMeshProUGUI _headerTextComponent;

    #region Window settings

    [Header("Settings of activation mechanic")]
    [Range(1f, 3f)]
    [SerializeField] private float _delayTime = 1.5f;

    [Range(0.1f, 0.7f)]
    [SerializeField] private float _minTapTime = 0.2f;

    [Range(0.5f, 1f)]
    [SerializeField] private float _maxTapTime = 1f;

    #endregion

    private AudioSource _tapSound = null;

    public override void Initialize(UIManager manager)
    {
        base.Initialize(manager);

        GetNecessaryComponents();
    }

    private void GetNecessaryComponents()
    {
        _tapSound = GetComponent<AudioSource>();
    }

    protected override void ProcessWindowActivation() => ActivationMechanics();
    private void ActivationMechanics()
    {
        StartCoroutine(ActivateWindow());
    }

    private IEnumerator ActivateWindow()
    {
        yield return StartCoroutine(Activation());

        _uiManager.ProcessingWinWindowActivation();
    }

    private IEnumerator Activation()
    {
        foreach (var symbol in _headerText)
        {
            _tapSound.Play();
            _headerTextComponent.text += symbol;

            var time = UnityEngine.Random.Range(_minTapTime, _maxTapTime);

            yield return new WaitForSeconds(time);
        }

        yield return new WaitForSeconds(_delayTime);
    }

    protected override void ProcessWindowDeactivation()
    {
        _tapSound?.Stop();

        _headerTextComponent.text = string.Empty;
    }
}
