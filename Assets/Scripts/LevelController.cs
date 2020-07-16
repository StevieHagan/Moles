using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    const int GRID_SNAP = 2;
    [SerializeField] int gridSize = 10;

    public int GetGridSnap()
    {
        return GRID_SNAP;
    }

    public int GetGridSize()
    {
        return gridSize;
    }
}
