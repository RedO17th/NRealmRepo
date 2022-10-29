using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType { None = -1, First, Second, Third }

public class FieldManager : MonoBehaviour
{
    [SerializeField] private MovementType _typeOfMovementRule = MovementType.None;

    public FieldBuilder Builder { get; private set; }  = null;

    public BaseCell FirstCell { get; protected set; } = null;
    public BaseCell SecondCell { get; protected set; } = null;

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
        _currentRule.Initialize(this);

        Builder.Initialize(this);
        Builder.Build();
    }

    public void AddCell(BaseCell cell)
    {
        if (cell.State == CellState.Blocked)
        {
            ProcessCancelMovement();

            return;
        }

        if (FirstCell == null)
            FirstCell = cell;
        else if (SecondCell == null)
        {
            SecondCell = cell;
        }
    }

    private void ProcessCancelMovement()
    {
        FirstCell?.ClearState();
        SecondCell?.ClearState();

        FirstCell = null;
        SecondCell = null;
    }

    public void MoveItem(BaseCell cell)
    {
        if (IsTheRulesMet(cell))
            MoveByRules();
    }

    private bool IsTheRulesMet(BaseCell cell)
    {
        bool result = _currentRule.IsPair(cell) && _currentRule.IsPositionCorrect();

        if (AreBothCellsPresentWithNegativeResult(result))
            ProcessCancelMovement();

        return result;
    }

    private bool AreBothCellsPresentWithNegativeResult(bool primaryResult)
    {
        return primaryResult == false && SecondCell != null;
    }

    private void MoveByRules()
    {
        _swapper.SetCells(FirstCell, SecondCell);
        _swapper.Swap();

        FirstCell.ClearState();
        SecondCell.ClearState();

        FirstCell = null;
        SecondCell = null;

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
    private FieldManager _fieldManager = null;

    protected BaseCell _firstCell => _fieldManager.FirstCell;
    protected BaseCell _secondCell => _fieldManager.SecondCell;

    public virtual void Initialize(FieldManager manager)
    {
        _fieldManager = manager;
    }

    public virtual bool IsPair(BaseCell cell)
    {
        bool result = false;

        if (_secondCell == null)
            return false;

        if (_firstCell != _secondCell || BothCellsAreNotFree())
            result = true;


        return result;
    }

    private bool BothCellsAreNotFree()
    {
        return _firstCell.State != CellState.Free && _secondCell.State != CellState.Free;
    }

    public abstract bool IsPositionCorrect();
}

public class CrossRule : BaseMovingRule
{
    private const int OFFSET = 1;

    public override bool IsPositionCorrect()
    {
        bool result = false;

        if (IsHorizontalAxis() && IsCoordsEqualBy(_firstCell.Y, _secondCell.Y))
            result = true;

        if (IsVerticalAxis() && IsCoordsEqualBy(_firstCell.X, _secondCell.X))
            result = true;

        return result;
    }

    private bool IsHorizontalAxis()
    {
        return _firstCell.X + OFFSET == _secondCell.X || _firstCell.X - OFFSET == _secondCell.X;
    }

    private bool IsVerticalAxis()
    {
        return _firstCell.Y + OFFSET == _secondCell.Y || _firstCell.Y - OFFSET == _secondCell.Y;
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
