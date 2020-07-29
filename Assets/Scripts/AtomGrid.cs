using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AtomGrid : MonoBehaviour
{
    Atom[,] atoms;
    LevelController controller;

    //Set populated by all bonds
    HashSet<Bond> bonds = new HashSet<Bond>(); 

    void Start()
    {
        controller = FindObjectOfType<LevelController>();

        atoms = new Atom[Settings.GetGridSize().x + 2, Settings.GetGridSize().y + 2];

        PopulateAtomsArray();
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

    public bool CheckWinCondition()
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
            controller.DisplayWinCanvas();
            return true;
        }
        else
        {
            return false;
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
        //Iterates through each atom and sets it as fully bonded as appropriate.
        foreach(Atom atom in atoms)
        {
            if(atom == null) { continue; }
            Bond[] childBonds = atom.GetComponentsInChildren<Bond>();
            bool unbondedBondExists = false;
            foreach(Bond childBond in childBonds)
            {
                if(childBond.GetBonded() == false)
                {
                    unbondedBondExists = true;
                    break;
                }
            }
            atom.SetFullyBonded(!unbondedBondExists);
        }
    }
}
