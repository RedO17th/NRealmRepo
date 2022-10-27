using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverMechanic : BaseCellMechanic
{
    private bool IsCover = false;

    private IScalable _item => _cell.Item;

    public override void Initialize(Component cell)
    {
        


    }

    private void OnMouseOver() => ProcessCover();
    private void ProcessCover()
    {
        if (IsCover == false)
        {
            IsCover = true;

            _item.Increase();
        }
    }

    private void OnMouseExit() => ProcessUnCover();
    private void ProcessUnCover()
    {
        if (IsCover)
        {
            IsCover = false;

            _item.Decrease();
        }
    }

}
