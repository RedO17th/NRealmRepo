using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMechanic : MonoBehaviour
{
    public abstract void Initialize(Component entity);

    public virtual void Activate() { }
    public virtual void Deactivate() { }
}

public enum CellMechanicType { None = -1, Click, Cover }
public class BaseCellMechanic : BaseMechanic
{
    [SerializeField] private CellMechanicType _mechanicType;

    public CellMechanicType Type => _mechanicType;

    protected BaseCell _cell = null;

    public override void Initialize(Component entity)
    {
        _cell = entity as BaseCell;
    }

    protected virtual void ClearMechanicState() { }
}

public class BaseItemMechanic : BaseMechanic
{
    protected BaseItem _item = null;

    public override void Initialize(Component entity)
    {
        _item = entity as BaseItem;
    }
}
