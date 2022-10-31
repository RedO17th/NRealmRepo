using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverMechanic : BaseCellMechanic
{
    private bool IsCover = false;

    private IScalable _item => _cell.Item;

    public override void Initialize(Component entity)
    {
        base.Initialize(entity);

        _cell.OnStateCleared += ClearMechanicState;
    }

    private void OnMouseOver() => ProcessCover();
    private void ProcessCover()
    {
        if (IsCover == false)
        {
            IsCover = true;

            _item?.Increase();
        }
    }

    private void OnMouseExit() => ProcessUnCover();
    private void ProcessUnCover()
    {
        if (IsCover && _cell.Choised == false)
        {
            IsCover = false;

            _item?.Decrease();
        }
    }

    private void ClearMechanicState() => IsCover = false;

    public override void Complete()
    {
        if (_cell)
            _cell.OnStateCleared -= ClearMechanicState;

        _cell = null;
    }
}
