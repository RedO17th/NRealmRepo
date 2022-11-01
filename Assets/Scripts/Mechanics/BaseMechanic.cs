using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMechanic : MonoBehaviour
{
    public abstract void Initialize(Component entity);

    public virtual void Complete() { }
}

public class BaseCellMechanic : BaseMechanic
{
    protected BaseCell _cell = null;

    public override void Initialize(Component entity)
    {
        _cell = entity as BaseCell;
    }
}

public class BaseItemMechanic : BaseMechanic
{
    protected BaseItem _item = null;

    public override void Initialize(Component entity)
    {
        _item = entity as BaseItem;
    }
}
