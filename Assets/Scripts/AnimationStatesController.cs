using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStatesController : MonoBehaviour
{
    [SerializeField] private string _parametrName = string.Empty;
    [SerializeField] private int _amountAnimation = 0;

    [Range(1f, 10f)]
    [SerializeField] private float _timeInState = 0;

    [Range(1f, 5f)]
    [SerializeField] private float _transitionTime = 0f;

    private Animator _animator = null;

    private int _currentAnimationID = 0;
    private int _newAnimationID = 0;

    private float _timeLeftInState = 0f;
    private bool _canSwitchState = false;

    private void Awake() => GetNecessaryComponents();
    private void GetNecessaryComponents()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start() => StartCoroutine(SwitchStates());

    private void Update()
    {
        if (_canSwitchState && _timeLeftInState >= _timeInState)
        {
            _canSwitchState = false;
            _timeLeftInState = 0f;
            StartCoroutine(SwitchStates());
        }

        if(_canSwitchState) _timeLeftInState += Time.deltaTime;
    }

    private IEnumerator SwitchStates()
    {
        _newAnimationID = GetFreeIndex();

        yield return StartCoroutine(TransitionTo());

        _currentAnimationID = _newAnimationID;
        _canSwitchState = true;
    }

    private int GetFreeIndex()
    {
        var idList = GenerateaListOfFreeIndexes();

        int id = idList[Random.Range(0, idList.Count)];
                 idList.Clear();

        return id;
    }

    private List<int> GenerateaListOfFreeIndexes()
    {
        var idList = new List<int>();

        for (int i = 0; i < _amountAnimation; i++)
        {
            if (i == _currentAnimationID)
                continue;

            idList.Add(i);
        }

        return idList;
    }

    private IEnumerator TransitionTo()
    {
        var timeLeft = 0f;

        while (timeLeft <= _transitionTime)
        {
            var transitionProcent = Mathf.Lerp(_currentAnimationID, _newAnimationID, timeLeft / _transitionTime);

            _animator.SetFloat(_parametrName, transitionProcent);

            timeLeft += Time.deltaTime;

            yield return null;
        }

        _animator.SetFloat(_parametrName, _newAnimationID);
    }

}
