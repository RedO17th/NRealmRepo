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
