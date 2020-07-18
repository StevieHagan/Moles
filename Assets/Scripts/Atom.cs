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
    int gridSize;
    Vector2Int gridPos;
    Atom[] pointsTo = new Atom[4];
    [SerializeField] List<Atom> bondsWith = new List<Atom>();
    BoxCollider2D collider;


    void Start()
    {
        grid = FindObjectOfType<AtomGrid>();
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
        if (tag == "Single")
        {
            numBonds = 1;
            if (rotationOffset == 0)
            {
                pointsTo[0] = grid.GetAtomAtGridPos(gridPos + Vector2Int.up);
            }
            else if (rotationOffset == 1)
            {
                pointsTo[0] = grid.GetAtomAtGridPos(gridPos + Vector2Int.right);
            }
            else if (rotationOffset == 2)
            {
                pointsTo[0] = grid.GetAtomAtGridPos(gridPos + Vector2Int.down);
            }
            else if (rotationOffset == 3)
            {
                pointsTo[0] = grid.GetAtomAtGridPos(gridPos + Vector2Int.left);
            }
        }
        if (tag == "Double Perp")
        {
            numBonds = 2;
            if (rotationOffset == 0)
            {
                pointsTo[0] = grid.GetAtomAtGridPos(gridPos + Vector2Int.up);
                pointsTo[1] = grid.GetAtomAtGridPos(gridPos + Vector2Int.right);
            }
            else if (rotationOffset == 1)
            {
                pointsTo[0] = grid.GetAtomAtGridPos(gridPos + Vector2Int.right);
                pointsTo[1] = grid.GetAtomAtGridPos(gridPos + Vector2Int.down);
            }
            else if (rotationOffset == 2)
            {
                pointsTo[0] = grid.GetAtomAtGridPos(gridPos + Vector2Int.down);
                pointsTo[1] = grid.GetAtomAtGridPos(gridPos + Vector2Int.left);
            }
            else if (rotationOffset == 3)
            {
                pointsTo[0] = grid.GetAtomAtGridPos(gridPos + Vector2Int.left);
                pointsTo[1] = grid.GetAtomAtGridPos(gridPos + Vector2Int.up);
            }
        }
        if (tag == "Double Straight")
        {
            numBonds = 2;
            if (rotationOffset == 0 || rotationOffset == 2)
            {
                pointsTo[0] = grid.GetAtomAtGridPos(gridPos + Vector2Int.up);
                pointsTo[1] = grid.GetAtomAtGridPos(gridPos + Vector2Int.down);
            }
            else if (rotationOffset == 1 || rotationOffset == 3)
            {
                pointsTo[0] = grid.GetAtomAtGridPos(gridPos + Vector2Int.right);
                pointsTo[1] = grid.GetAtomAtGridPos(gridPos + Vector2Int.left);
            }
        }
        if (tag == "Triple")
        {
            numBonds = 3;
            if (rotationOffset == 0)
            {
                pointsTo[0] = grid.GetAtomAtGridPos(gridPos + Vector2Int.up);
                pointsTo[1] = grid.GetAtomAtGridPos(gridPos + Vector2Int.right);
                pointsTo[2] = grid.GetAtomAtGridPos(gridPos + Vector2Int.down);
            }
            else if (rotationOffset == 1)
            {
                pointsTo[0] = grid.GetAtomAtGridPos(gridPos + Vector2Int.right);
                pointsTo[1] = grid.GetAtomAtGridPos(gridPos + Vector2Int.down);
                pointsTo[2] = grid.GetAtomAtGridPos(gridPos + Vector2Int.left);
            }
            else if (rotationOffset == 2)
            {
                pointsTo[0] = grid.GetAtomAtGridPos(gridPos + Vector2Int.down);
                pointsTo[1] = grid.GetAtomAtGridPos(gridPos + Vector2Int.left);
                pointsTo[2] = grid.GetAtomAtGridPos(gridPos + Vector2Int.up);
            }
            else if (rotationOffset == 3)
            {
                pointsTo[0] = grid.GetAtomAtGridPos(gridPos + Vector2Int.left);
                pointsTo[1] = grid.GetAtomAtGridPos(gridPos + Vector2Int.up);
                pointsTo[2] = grid.GetAtomAtGridPos(gridPos + Vector2Int.right);
            }

        }
        if (tag == "Quad")
        {
            numBonds = 4;
                pointsTo[0] = grid.GetAtomAtGridPos(gridPos + Vector2Int.up);
                pointsTo[1] = grid.GetAtomAtGridPos(gridPos + Vector2Int.right);
                pointsTo[2] = grid.GetAtomAtGridPos(gridPos + Vector2Int.down);
                pointsTo[3] = grid.GetAtomAtGridPos(gridPos + Vector2Int.left);
        }
        //String sentence = "Atom " + name + "points to ";
        //foreach (Atom atom in pointsTo)
        //{
        //    if (atom)
        //    {
        //        sentence = sentence + atom.name + ", ";
        //    }
        //}
        //print(sentence);

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

        if(bondsTouching == numBonds)
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

    public bool PointsTo(Vector2Int gridPos)
    {
        //returns true if this atom points to a specified grid position
        bool pointsToGridPos = false;
        foreach(Atom atom in pointsTo)
        {
            if(!atom) { continue; }
            if(atom.GetGridPos() == gridPos) { pointsToGridPos = true; }
        }
        return pointsToGridPos;
    }

    public bool IsFullyBonded()
    {
        return fullyBonded;
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {//When the left mouse button is clicked, the rotation offset is increased (cycles back to 0 from 3)

            rotationOffset = (rotationOffset == 3 ? 0 : rotationOffset + 1);
            UpdatePointsTo();
            UpdateBondsWith(true);
        }
    }
}
