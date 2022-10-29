using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleMechanic : BaseItemMechanic
{
    [Range(1.1f, 1.25f)]
    [SerializeField] private float _maxScale = 1.1f;

    [Range(0f, 1.5f)]
    [SerializeField] private float _timeToMaxScale = 0.1f;

    [Range(0f, 0.5f)]
    [SerializeField] private float _timeToMinScale = 0.05f;

    private Coroutine _scaleRoutine = null;

    private float _standartScale = 1f;

    public override void Initialize(Component entity)
    {
        base.Initialize(entity);

        _standartScale = _item.CurrentScale;

        _item.OnIncreaseEvent += Increase;
        _item.OnDecreaseEvent += Decrease;
    }

    private void Increase()
    {
        if (_scaleRoutine != null)
            StopCoroutine(_scaleRoutine);

        _scaleRoutine = StartCoroutine(ScaleRoutine(_maxScale, _timeToMaxScale));
    }

    private void Decrease()
    {
        if (_scaleRoutine != null)
            StopCoroutine(_scaleRoutine);

        _scaleRoutine = StartCoroutine(ScaleRoutine(_standartScale, _timeToMinScale));
    }

    private IEnumerator ScaleRoutine(float targetScale, float executionTime)
    {
        var pastTime = 0f;

        var currentScale = transform.localScale.x;

        while (pastTime <= executionTime)
        {
            var scale = Mathf.Lerp(currentScale, targetScale, pastTime / executionTime);

            transform.localScale = new Vector3(scale, 1f, scale);

            pastTime += Time.deltaTime;

            yield return null;
        }

        transform.localScale = new Vector3(targetScale, 1f, targetScale);
        pastTime = 0;
    }

    private void OnDisable()
    {
        if(_scaleRoutine != null)
            StopCoroutine(_scaleRoutine);

        if (_item)
        { 
            _item.OnIncreaseEvent -= Increase;
            _item.OnDecreaseEvent -= Decrease;            
        }
    }
}
