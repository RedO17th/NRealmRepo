using System.Collections;
using TMPro;
using UnityEngine;

public class WinWindow : BaseWindow
{
    [SerializeField] private string _headerText;
    [SerializeField] private TextMeshProUGUI _headerTextComponent;

    [Header("Settings of activation mechanic")]
    [Range(0.1f, 0.7f)]
    [SerializeField] private float _minTapTime = 0.2f;

    [Range(0.5f, 1f)]
    [SerializeField] private float _maxTapTime = 1f;

    private Coroutine _activationRoutine = null;

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
        _activationRoutine = StartCoroutine(Activation());
    }

    private IEnumerator Activation()
    {
        foreach (var symbol in _headerText)
        {
            _tapSound.Play();
            _headerTextComponent.text += symbol;

            var time = Random.Range(_minTapTime, _maxTapTime);

            yield return new WaitForSeconds(time);
        }
    }

    protected override void ProcessWindowDeactivation()
    {
        if (_activationRoutine != null)
            StopCoroutine(_activationRoutine);

        _headerTextComponent.text = string.Empty;
    }
}
