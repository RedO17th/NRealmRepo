using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType { None = -1, First, Second, Third }

public class FieldManager : MonoBehaviour
{
    [SerializeField] private MovementType _typeOfMovementRule = MovementType.None;

    public FieldBuilder Builder { get; private set; }  = null;

    private BaseMovingRule _currentRule = null;
    private BaseSwapperOfCells _swapper = null;

    private Dictionary<MovementType, BaseMovingRule> _rulesByType;

    private void Awake()
    {
        Builder = GetComponent<FieldBuilder>();

        _rulesByType = new Dictionary<MovementType, BaseMovingRule>()
        {
            { MovementType.None, null },
            { MovementType.Cross, new CrossRule() }
        };
    }

    private void OnEnable()
    {
        _swapper = new BaseSwapperOfCells();

        _currentRule = _rulesByType[_typeOfMovementRule];

        Builder.Initialize(this);
        Builder.Build();
    }

    public void MoveItem(BaseCell cell)
    {
        if (IsTheRulesMet(cell))
            MoveByRules();
    }

    private bool IsTheRulesMet(BaseCell cell)
    {
        return _currentRule.IsPair(cell) && _currentRule.IsPositionCorrect();
    }

    private void MoveByRules()
    {
        var fCell = _currentRule.FirstCell;
        var sCell = _currentRule.SecondCell;

        _currentRule.RemoveData();

        _swapper.SetCells(fCell, sCell);
        _swapper.Swap();

        fCell.SetEmptyState();
        sCell.SetEmptyState();

        CheckGameSequence();
    }

    private void CheckGameSequence()
    {
        if (Builder.IsTheFieldAssembledCorrectly())
        {
            Debug.Log($"FieldManager - Собрано");
        }
    }
}

public enum MovementType { None = -1, Cross }
public abstract class BaseMovingRule
{
    public BaseCell FirstCell { get; protected set; } = null;
    public BaseCell SecondCell { get; protected set; } = null;

    public virtual bool IsPair(BaseCell cell)
    {
        if (cell.State == CellState.Blocked)
        {
            RemoveData();
            return false;
        }

        if (FirstCell == null)
        {
            FirstCell = cell;

            return false;
        }
        else if (SecondCell == null)
        {
            SecondCell = cell;
        }

        if (FirstCell == SecondCell || BothCellsAreFree())
            RemoveData();

        if (FirstCell != null && SecondCell != null)
            return true;

        return false;
    }

    private bool BothCellsAreFree()
    {
        return FirstCell.State == CellState.Free && SecondCell.State == CellState.Free;
    }

    public abstract bool IsPositionCorrect();

    public virtual void RemoveData()
    {
        FirstCell = null;
        SecondCell = null;
    }
}

public class CrossRule : BaseMovingRule
{
    private const int OFFSET = 1;

    public override bool IsPositionCorrect()
    {
        bool result = false;

        if (IsHorizontalAxis() && IsCoordsEqualBy(FirstCell.Y, SecondCell.Y))
            result = true;

        if (IsVerticalAxis() && IsCoordsEqualBy(FirstCell.X, SecondCell.X))
            result = true;

        if(result == false)
            RemoveData();

        return result;
    }

    private bool IsHorizontalAxis()
    {
        return FirstCell.X + OFFSET == SecondCell.X || FirstCell.X - OFFSET == SecondCell.X;
    }

    private bool IsVerticalAxis()
    {
        return FirstCell.Y + OFFSET == SecondCell.Y || FirstCell.Y - OFFSET == SecondCell.Y;
    }

    private bool IsCoordsEqualBy(int fCoord, int sCoord)
    {
        return fCoord == sCoord;
    }
}

public class BaseSwapperOfCells
{
    private BaseCell _firstCell = null;
    private BaseCell _secondCell = null;

    public void SetCells(BaseCell fCell, BaseCell sCell)
    {
        _firstCell = fCell;
        _secondCell = sCell;
    }

    public void Swap()
    {
        SwapColors();
        SwapColorItems();
        SwapState();

        RemoveData();
    }

    private void SwapColors()
    {
        var firstMaterial = _firstCell.Material;
        var secondMaterial = _secondCell.Material;

        _firstCell.SetMaterial(secondMaterial);
        _secondCell.SetMaterial(firstMaterial);
    }
    private void SwapColorItems()
    {
        var firstItem = _firstCell.Item;
        var secondItem = _secondCell.Item;

        _firstCell.SetItem(secondItem);
        _secondCell.SetItem(firstItem);
    }
    private void SwapState()
    {
        var firstState = _firstCell.State;
        var secondState = _secondCell.State;

        _firstCell.SetState(secondState);
        _secondCell.SetState(firstState);
    }

    private void RemoveData()
    {
        _firstCell = null;
        _secondCell = null;
    }

}
