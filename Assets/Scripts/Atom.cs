using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Atom : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1;
    [SerializeField] int bondsTouching = 0;

    int rotationOffset = 0;
    int numBonds = 0;
    [SerializeField] bool fullyBonded;
    AtomGrid grid;
    Bond[] bonds;
    int gridSize;
    Vector2Int gridPos;
    List<Atom> pointsTo = new List<Atom>();
    [SerializeField] List<Atom> bondsWith = new List<Atom>();


    void Start()
    {
        grid = FindObjectOfType<AtomGrid>();
        bonds = GetComponentsInChildren<Bond>();
        InitialiseOffsets();
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {//Delayed start to some methods needed to allow all atoms to load
        //into atomgrid before proceeding.
        yield return new WaitForSeconds(0.5f);
        UpdatePointsTo();
        UpdateBondsWith(true);
    }

    // Update is called once per frame
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
            UpdateBondsWith(true);
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


    public void UpdateBondsWith(bool wasMoved)
    {//Pass in true if this atom has just been moved, causes its neighbours to also Update their bonds

        bondsWith.Clear();

        foreach(Atom atom in pointsTo)//iterate over each atom this atom points to
        {
            if(!atom) { continue; }
            if(atom.PointsTo(gridPos)) //Does the atom we're pointing to point back?
            {
                bondsWith.Add(atom);//Add it to the list
            }
        }            
        bondsTouching = bondsWith.Count;

        if (bondsTouching == bonds.Length)
        {
            fullyBonded = true;
        }
        else
        {
            fullyBonded = false;
        }

        //If this atom has just been moved, ask all its neighbours to recheck their bonds.
        if(wasMoved)
        {
            Atom tester;
            tester = grid.GetAtomAtGridPos(gridPos + Vector2Int.up);
            if(tester) { tester.UpdateBondsWith(false); }
            tester = grid.GetAtomAtGridPos(gridPos + Vector2Int.right);
            if (tester) { tester.UpdateBondsWith(false); }
            tester = grid.GetAtomAtGridPos(gridPos + Vector2Int.down);
            if (tester) { tester.UpdateBondsWith(false); }
            tester = grid.GetAtomAtGridPos(gridPos + Vector2Int.left);
            if (tester) { tester.UpdateBondsWith(false); }
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

    public bool IsFullyBonded()
    {
        return fullyBonded;
    }

  
}
