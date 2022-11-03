using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMechanic : BaseCellMechanic
{
    public bool Choised { get; private set; } = false;

    private FieldManager _fieldManager = null;
    private BaseItem _item => _cell.Item;

    public override void Initialize(Component entity)
    {
        base.Initialize(entity);

        _fieldManager = _cell.FieldManager;
    }

    public override void Activate()
    {
        _cell.OnStateCleared += ClearMechanicState;
    }

    private void OnMouseDown() => ProcessClick();
    private void ProcessClick()
    {
        if (_item && Choised == false)
        {
            _item.Increase();
            Choised = true;
        }

        _fieldManager.AddCell(_cell);
        _fieldManager.MoveItem(_cell);
    }

    protected override void ClearMechanicState() => Choised = false;

    public override void Deactivate()
    {
        _cell.OnStateCleared -= ClearMechanicState;
    }
}
