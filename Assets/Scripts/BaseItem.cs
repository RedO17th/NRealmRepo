using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour, IScalable
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private BaseMechanic[] _mechanics;

    public CellType Type { get; private set; } = CellType.None;
    public float CurrentScale => transform.localScale.x;

    public event Action OnIncreaseEvent;
    public event Action OnDecreaseEvent;

    public void Initialize(CellType type, Material material, Transform container)
    {
        Type = type;

        SetMaterial(material);
        PutIn(container);

        InitializeAndActivateMechanics();
    }

    private void SetMaterial(Material material)
    {
        _meshRenderer.material = material;
    }
    
    private void PutIn(Transform container)
    {
        transform.parent = container;
    }

    private void InitializeAndActivateMechanics()
    {
        foreach (var mechanic in _mechanics)
        { 
            mechanic?.Initialize(this);
            mechanic?.Activate();
        }
    }
   
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void Increase() => OnIncreaseEvent?.Invoke();
    public void Decrease() => OnDecreaseEvent?.Invoke();

    public void CompleteExecution()
    {
        CompleteMechanics();

        Destroy(gameObject);
    }

    private void CompleteMechanics()
    {
        foreach (var mechanic in _mechanics)
            mechanic?.Deactivate();
    }
}
