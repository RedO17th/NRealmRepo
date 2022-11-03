using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverMechanic : BaseCellMechanic
{
    private bool _isCover = false;

    private ClickMechanic _clickMechanic = null;
    private IScalable _item => _cell.Item;

    public override void Activate()
    {
        _cell.OnStateCleared += ClearMechanicState;

        GetNecessaryComponents();
    }

    private void GetNecessaryComponents()
    {
        _clickMechanic = _cell.GetMechanicBy(CellMechanicType.Click) as ClickMechanic;
    }

    private void OnMouseOver() => ProcessCover();
    private void ProcessCover()
    {
        if (_isCover == false)
        {
            _isCover = true;

            _item?.Increase();
        }
    }

    private void OnMouseExit() => ProcessUnCover();
    private void ProcessUnCover()
    {
        if (_isCover && _clickMechanic.Choised == false)
        {
            _isCover = false;

            _item?.Decrease();
        }
    }

    protected override void ClearMechanicState() => _isCover = false;

    public override void Deactivate()
    {
        if (_cell)
            _cell.OnStateCleared -= ClearMechanicState;

        _clickMechanic = null;
        _cell = null;
    }
}
