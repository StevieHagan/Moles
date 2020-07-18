using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AtomGrid : MonoBehaviour
{
    int gridSize;
    Atom[,] atoms;

    //Contains all the gridRefs which are pointed at by a bond as KEY
    //and the bond whi
    [SerializeField] HashSet<Bond> bonds = new HashSet<Bond>(); 

    void Start()
    {
        gridSize = FindObjectOfType<LevelController>().GetGridSize();

        atoms = new Atom[gridSize, gridSize];

        PopulateAtomsArray();
    }

    void Update()
    {
        CheckWinCondition();
    }

    private void PopulateAtomsArray()
    {//Put all atoms in a temp linear array then sort them into a 2D array
     //with indexes matching grid position.

        Atom[] tempArray = FindObjectsOfType<Atom>();

        foreach(Atom atom in tempArray)
        {
            int x = atom.GetGridPos().x;
            int y = atom.GetGridPos().y;

            atoms[x, y] = atom;
        }

    }

    private void CheckWinCondition()
    {
        bool unbondedBondExists = false;

        foreach(Bond bond in bonds)
        {
            if(bond.GetBonded() == false)
            {
                unbondedBondExists = true;
                break;
            }
        }
        if(!unbondedBondExists)
        {
            print("YOU WIN!!");
        }
    }

    public Atom GetAtomAtGridPos(Vector2Int gridPosition)
    {
        return atoms[gridPosition.x, gridPosition.y];
    }

    
    public void AddBond(Bond bond)
    {//Adds a bond to the HashSet
        bonds.Add(bond);
        
    }

    public void EvaluateBonds()
    {//Iterates through all bonds and sets them as bonded or unbonded
        print(bonds.Count);

        foreach(Bond bondA in bonds)
        {
            foreach(Bond bondB in bonds)
            {
                if(bondA.GetComponentInParent<Atom>() == bondB.GetComponentInParent<Atom>()) { continue; }
                if( bondB.GetComponentInParent<Atom>().GetGridPos() == bondA.GetPointsTo() &&
                    bondA.GetComponentInParent<Atom>().GetGridPos() == bondB.GetPointsTo())
                {
                    bondA.SetBonded(true);
                    bondB.SetBonded(true);
                    break;
                }
                else
                {
                    bondA.SetBonded(false);
                }
            }
        }
    }
}
