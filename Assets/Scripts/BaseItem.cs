using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    public CellType Type = CellType.None;

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
}
