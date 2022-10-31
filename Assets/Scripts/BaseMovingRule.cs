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
