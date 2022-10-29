using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellState { None = -1, Blocked, Free, Busy }

public class BaseCell : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private BaseMechanic[] _mechanics;

    public event Action OnStateCleared;

    #region Public properties
    public CellState State { get; private set; } = CellState.None;
    public CellType Type { get; private set; } = CellType.None;

    public BaseItem Item { get; private set; } = null;
    public bool MatchesByType { get; private set; } = false;

    public Vector3 Position => transform.position;
    public Material Material => _meshRenderer.material;
    public int X => (int)transform.position.x;
    public int Y => (int)transform.position.z;

    public bool Choised = false;
    #endregion

    private FieldManager _fieldManager = null;

    public void Initialize(FieldManager manager)
    {
        _fieldManager = manager;

        InitializeMechanics();
    }

    private void InitializeMechanics()
    {
        foreach (var mechanic in _mechanics)
            mechanic.Initialize(this);
    }

    public void PutIn(Transform container)
    {
        transform.parent = container;
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

    private void OnMouseDown()
    {
        if (Item && Choised == false)
        {
            Item.Increase();
            Choised = true;
        }

        _fieldManager.AddCell(this);
        _fieldManager.MoveItem(this);
    }

    public void ClearState()
    {
        Choised = false;

        if (Item)
        {
            Item.Decrease();

            OnStateCleared?.Invoke();
        }
    }
}
