using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour
{
    public Color[] colors;
    public HexGrid hexGrid;
    private Color activeColor;
    int activeElevation;

    private void Awake()
    {
        SelectColor(0);

    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(inputRay, out RaycastHit hit))
        {
            EditCell(hexGrid.GetCell(hit.point));
        }
    }

    private void EditCell(HexCell hexCell)
    {
        hexCell.color = activeColor;
        hexCell.Elevation = activeElevation;
    }

    public void SetElevation(float elevation)
    {
        activeElevation = (int)elevation;
    }

    public void SelectColor(int v)
    {
        activeColor = colors[v];
    }
}
