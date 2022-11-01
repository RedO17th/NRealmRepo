using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private FieldManager _fieldManager;
    [SerializeField] private UIManager _uiManager;

    private void Awake()
    {
        //
    }

    private void Start()
    {
        _uiManager.Initialize();

        _fieldManager.Initialize();
        _fieldManager.BuildField();
    }
}


