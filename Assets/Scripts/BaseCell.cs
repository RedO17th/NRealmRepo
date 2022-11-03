using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CellState { None = -1, Blocked, Free, Busy }

public class BaseCell : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Collider _collider;
    [SerializeField] private BaseCellMechanic[] _mechanics;

    public event Action OnStateCleared;

    #region Public properties
    public CellState State { get; private set; } = CellState.None;
    public CellType Type { get; private set; } = CellType.None;

    public FieldManager FieldManager { get; private set; } = null;
    public BaseItem Item { get; private set; } = null;
    public bool MatchesByType { get; private set; } = false;

    public Vector3 Position => transform.position;
    public Material Material => _meshRenderer.material;
    public int X => (int)transform.position.x;
    public int Y => (int)transform.position.z;

    public bool Choised { get; private set; } = false;
    #endregion


    public void Initialize(FieldManager manager)
    {
        FieldManager = manager;

        InitializeAndActivateMechanics();
    }

    private void InitializeAndActivateMechanics()
    {
        foreach (var mechanic in _mechanics)
        { 
            mechanic.Initialize(this);
            mechanic.Activate();
        }
    }

    public void PutIn(Transform container)
    {
        transform.parent = container;
    }

    #region Set methods

    public void SetState(CellState state) => State = state;

    public void SetItem(BaseItem item)
    {
        Item = item;

        if (Item != null)
        { 
            float yPosition = Position.y + FieldManager.Builder.YOffsetPosition;
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

    #endregion

    public BaseCellMechanic GetMechanicBy(CellMechanicType type)
    {
        return _mechanics.FirstOrDefault(m => m.Type == type);
    }

    public void ClearState()
    {
        if (Item)
        {
            Item.Decrease();

            OnStateCleared?.Invoke();
        }
    }

    //[TODO] ReWrite functional with ClickMechanic
    public void DisableClickMechanic()
    {
        _collider.enabled = false;
    }
    //..

    public void CompleteExecution()
    {
        Item?.CompleteExecution();
        Item = null;

        CompleteMechanics();

        FieldManager = null;

        Destroy(gameObject);
    }

    private void CompleteMechanics()
    {
        foreach (var mechanic in _mechanics)
            mechanic?.Deactivate();
    }
}
