public class BaseSwapperOfCells
{
    private FieldManager _fieldManager = null;

    private BaseCell _firstCell => _fieldManager.FirstCell;
    private BaseCell _secondCell => _fieldManager.SecondCell;

    public virtual void Initialize(FieldManager manager)
    {
        _fieldManager = manager;
    }

    public void Swap()
    {
        SwapColors();
        SwapColorItems();
        SwapState();
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
}
