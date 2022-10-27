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
        Debug.Log($"BaseItem.Increase");
    }

    public void Decrease()
    {
        Debug.Log($"BaseItem.Decrease");
    }
}

public interface IScalable
{
    void Increase();
    void Decrease();
}
