using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellState { None = -1, Blocked, Free, Busy }

public class BaseCell : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    #region Public properties
    public CellState State { get; private set; } = CellState.None;
    public CellType Type { get; private set; } = CellType.None;
    public CellType ItemType => Item.Type;

    public BaseItem Item { get; private set; } = null;
    public bool MatchesByType { get; private set; } = false;

    public Vector3 Position => transform.position;
    public int X => (int)transform.position.x;
    public int Y => (int)transform.position.z;
    #endregion

    private FieldManager _fieldManager = null;

    public void Initialize(FieldManager manager)
    {
        _fieldManager = manager;
    }

    public void SetState(CellState state) => State = state;

    public void SetItem(BaseItem item)
    {
        Item = item;

        if (Item != null)
        { 
            float yPosition = Position.y + _fieldManager.Builder.YOffsetPosition;
            Item.SetPosition(new Vector3(Position.x, yPosition, Position.z)); 
        }

        CheckMatchByType();
    }

    private void CheckMatchByType()
    {
        MatchesByType = Item != null && Item.Type == Type;
    }

    public void SetType(CellType type) => Type = type;

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetMaterial(Material material)
    {
        _meshRenderer.material = material;
    }

    public Material GetMaterial() 
    {
        return _meshRenderer.material;
    }

    private void OnMouseDown()
    {
        _fieldManager.MoveItem(this);
    }
}
