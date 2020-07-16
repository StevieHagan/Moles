using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom : MonoBehaviour
{
    int gridSize;
    Vector2Int gridPos; 

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Vector2Int GetGridPos()
    {
        if (gridSize == 0)
        {
            gridSize = FindObjectOfType<LevelController>().GetGridSize();
        }

        gridPos.x = Mathf.RoundToInt(transform.position.x / gridSize);
        gridPos.y = Mathf.RoundToInt(transform.position.y / gridSize);

        return gridPos;
    }
}
