using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType { None = -1, First, Second, Third }

public class FieldManager : MonoBehaviour
{
    [SerializeField] private MovementType _typeOfMovementRule = MovementType.None;

    public event Action OnFieldWasAssembled;

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
        _swapper.Initialize(this);

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
        _swapper.Swap();

        ProcessCancelMovement();

        CheckGameSequence();
    }

    private void CheckGameSequence()
    {
        if (Builder.IsTheFieldAssembledCorrectly())
            OnFieldWasAssembled?.Invoke();
    }

    private void OnDisable()
    {
        Builder.CompleteExecution();
        Builder = null;

        _currentRule = null;
        _swapper = null;

        _rulesByType.Clear();

        FirstCell = null;
        SecondCell = null;
    }
}
