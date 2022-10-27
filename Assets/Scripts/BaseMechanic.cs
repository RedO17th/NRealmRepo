using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMechanic : MonoBehaviour
{
    public abstract void Initialize(Component entity);
}

public class BaseCellMechanic : BaseMechanic
{
    protected BaseCell _cell = null;

    public override void Initialize(Component entity)
    {
        _cell = entity as BaseCell;
    }
}
