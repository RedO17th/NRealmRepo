using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class FieldBuilder : MonoBehaviour
{
    [SerializeField] private BaseCell _cellPrefab;
    [SerializeField] private Transform _cellContainer;

    [SerializeField] private BaseItem _itemPrefab;
    [SerializeField] private Transform _itemContainer;
    [Range(0.1f, 1f)]
    [SerializeField] private float _yOffsetPosition = 0.5f;

    [Space]
    [SerializeField] private Material _freeMaterial;
    [SerializeField] private Material _emptyMaterial;
    [SerializeField] private Material _blockedMaterial;

    [SerializeField] private List<ColorByType> _colors;

    public float YOffsetPosition => _yOffsetPosition;

    private FieldManager _fieldManager = null;

    private const int FIELDORDER = 5;
    private const int AMOUNTBLOCKED = 6;

    private BaseCell[,] _cells = new BaseCell[FIELDORDER, FIELDORDER];

    private List<ColorByType> _temporaryColorsByType = new List<ColorByType>();
    private List<BaseCell> _unPlayableTemporaryCells = new List<BaseCell>();

    //[TODO] Translate to private
    public List<BaseCell> _gameCellSequence = new List<BaseCell>();

    public void Initialize(FieldManager manager)
    {
        _fieldManager = manager;
    }

    public void Build()
    {
        BuildField();
        DefineTypeOfCommonColumn();

        InitializeBlockedCells();
        InitializeFreeCells();
        InitializeBusyCells();
    }

    private void BuildField()
    {
        for (int y = 2, i = 0; y >= -2; i++, y--)
        {
            for (int x = -2, j = 0; x <= 2; j++, x++)
            {
                var cell = Instantiate(_cellPrefab);
                    cell.Initialize(_fieldManager);
                    cell.PutIn(_cellContainer);
                    cell.SetMaterial(_emptyMaterial);
                    cell.SetPosition(new Vector3(x, 0f, y));

                _cells[i, j] = cell;
            }
        }
    }

    private void DefineTypeOfCommonColumn()
    {
        var colorIndex = 0;
        for (int j = 0; j < FIELDORDER; j += 2, colorIndex++)
        {
            for (int i = 0; i < FIELDORDER; i++)
                _cells[i, j].SetType(_colors[colorIndex].Type);
        }
    }

    #region Initialize Blocked Cells
    private void InitializeBlockedCells()
    {
        _unPlayableTemporaryCells = GetFreeCellsByPattern();

        var firstPart = GetHalfOfTheUnplayableCellsBy(column: 1);
        var secondPart = GetHalfOfTheUnplayableCellsBy(column: 3);

        Shuffle(firstPart);
        Shuffle(secondPart);

        SetBlockedState(firstPart);
        SetBlockedState(secondPart);
    }

    private List<BaseCell> GetFreeCellsByPattern()
    {
        var patternStep = 2;
        var freeCells = new List<BaseCell>();

        for (int i = 0; i < FIELDORDER; i++)
        {
            for (int j = 1; j < FIELDORDER; j += patternStep)
                freeCells.Add(_cells[i, j]);
        }

        return freeCells;
    }

    private List<BaseCell> GetHalfOfTheUnplayableCellsBy(int column)
    {
        var divider = 2;
        var part = new List<BaseCell>();

        for (int i = 0; i < _unPlayableTemporaryCells.Count / divider; i++)
            part.Add(_cells[i, column]);

        return part;
    }

    public void Shuffle<T>(List<T> list)
    {
        Random rand = new();

        for (int i = list.Count - 1; i >= 1; i--)
        {
            int j = rand.Next(i + 1);

            T tmp = list[j];
            list[j] = list[i];
            list[i] = tmp;
        }
    }

    private void SetBlockedState(List<BaseCell> list)
    {
        var divider = 2;

        for (int i = 0; i < AMOUNTBLOCKED / divider; i++)
        {
            list[i].SetState(CellState.Blocked);
            list[i].SetMaterial(_blockedMaterial);
        }
    }
    #endregion

    private void InitializeFreeCells()
    {
        for (int i = 0; i < _unPlayableTemporaryCells.Count; i++)
        {
            var cell = _unPlayableTemporaryCells[i];

            if (cell.State == CellState.Blocked)
                continue;

            cell.SetState(CellState.Free);
            cell.SetMaterial(_freeMaterial);
        }

        _unPlayableTemporaryCells.Clear();
    }

    private void InitializeBusyCells()
    {
        PrepareListOfColors();

        int indexMaterial = 0;
        for (int i = 0; i < FIELDORDER; i++)
        {
            for (int j = 0; j < FIELDORDER; j++)
            {
                if (_cells[i, j].State == CellState.None)
                {
                    var cell = _cells[i, j];
                        cell.SetState(CellState.Busy);

                    var type = _temporaryColorsByType[indexMaterial].Type;
                    var material = _temporaryColorsByType[indexMaterial].Color;

                    var item = Instantiate(_itemPrefab);
                        item.Initialize(type, material, _itemContainer);

                    cell.SetItem(item);
                    _gameCellSequence.Add(cell);

                    indexMaterial++;
                }
            }
        }

        _temporaryColorsByType.Clear();
    }

    private void PrepareListOfColors()
    {
        for (int i = 0; i < _colors.Count; i++)
        {
            for (int j = 0; j < FIELDORDER; j++)
                _temporaryColorsByType.Add(_colors[i]);
        }

        Shuffle(_temporaryColorsByType);
    }

    public bool IsTheFieldAssembledCorrectly()
    {
        return _gameCellSequence.FirstOrDefault(cell => cell.MatchesByType == false) ? false : true;
    }

}

[System.Serializable]
public class ColorByType
{
    [SerializeField] private CellType _cellType;
    [SerializeField] private Material _cellColor;

    public CellType Type => _cellType;
    public Material Color => _cellColor;
}
