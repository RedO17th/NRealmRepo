using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour, IScalable
{
    [SerializeField] private MeshRenderer _meshRenderer;

    public CellType Type = CellType.None;

    public void PutIn(Transform container)
    {
        transform.parent = container;
    }
    public void SetType(CellType type)
    {
        Type = type;
    }
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void SetMaterial(Material material)
    {
        _meshRenderer.material = material;
    }

    public void Increase()
    {
        if (_scaleRoutine != null)
            StopCoroutine(_scaleRoutine);

        _scaleRoutine = StartCoroutine(ScaleRoutine(_maxScale, _timeToMaxScale));
    }

    public void Decrease()
    {
        if (_scaleRoutine != null)
            StopCoroutine(_scaleRoutine);

        _scaleRoutine = StartCoroutine(ScaleRoutine(_standartScale, _timeToMinScale));
    }

    private Coroutine _scaleRoutine = null;
    private float _maxScale = 1.1f;
    private float _standartScale = 1f;
    private float _timeToMaxScale = 0.2f;
    private float _timeToMinScale = 0.1f;

    private IEnumerator ScaleRoutine(float targetScale, float executionTime)
    {
        float pastTime = 0;

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

    private IEnumerator DecreaseRoutine()
    {
        yield return null;
    }
}

public interface IScalable
{
    void Increase();
    void Decrease();
}
