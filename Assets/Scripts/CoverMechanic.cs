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
            Debug.Log($"CoverMechanic.ProcessCover()");

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

    //[TODO] Rename
    private void ClearMechanicState() => IsCover = false;
    private void OnDisable()
    {
        if(_cell)
            _cell.OnStateCleared -= ClearMechanicState;
    }
}
