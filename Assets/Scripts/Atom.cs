using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Atom : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1;
    [SerializeField] int rotationOffset = 0;
    AtomGrid grid;
    Bond[] bonds;
    int gridSize;
    Vector2Int gridPos;
    List<Atom> pointsTo = new List<Atom>();


    void Start()
    {        
        InitialiseOffsets();
        grid = FindObjectOfType<AtomGrid>();
        bonds = GetComponentsInChildren<Bond>();
        UpdatePointsTo();
    }

    void Update()
    {
        RotationAnimator();
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {//When the left mouse button is clicked, the rotation offset is increased (cycles back to 0 from 3)

            rotationOffset = (rotationOffset == 3 ? 0 : rotationOffset + 1);
            BroadcastMessage("UpdateBonds");
            UpdatePointsTo();
            grid.CheckWinCondition();
        }
    }

    private void InitialiseOffsets()
    {//Sets the correct offset index for atoms rotated at design time.
        int zRotation = Mathf.RoundToInt(transform.rotation.eulerAngles.z);

        if(zRotation == 0) { rotationOffset = 0; }
        else if(zRotation == -90 || zRotation == 270) { rotationOffset = 1; }
        else if(zRotation == -180 || zRotation == 180) { rotationOffset = 2; }
        else if(zRotation == -270 || zRotation == 90) { rotationOffset = 3; }
        else { rotationOffset = 0; }
    }

    private void UpdatePointsTo()
    {
        pointsTo.Clear();

        foreach (Bond bond in bonds)
        {
            pointsTo.Add(grid.GetAtomAtGridPos(bond.GetPointsTo()));
        }
    }
    
    private void RotationAnimator()
    {//Rotates the atom using a slerp for smooth animation, target rotation is -90 * offset.

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, rotationOffset * -90f),
                                                      rotationSpeed * Time.deltaTime);
    }


    public Vector2Int GetGridPos()
    {
        if (gridSize == 0)
        {
            gridSize = FindObjectOfType<LevelController>().GetGridSnap();
        }

        gridPos.x = Mathf.RoundToInt(transform.position.x / gridSize);
        gridPos.y = Mathf.RoundToInt(transform.position.y / gridSize);

        return gridPos;
    }

    public int GetRotationOffset() { return rotationOffset; }

    public bool PointsTo(Vector2Int gridPos)
    {
        //returns true if this atom points to a specified grid position
        bool pointsToGridPos = false;

        foreach(Bond bond in bonds)
        {
            if(bond.GetPointsTo() == gridPos) { pointsToGridPos = true; }
        }
        return pointsToGridPos;
    }
  
}
