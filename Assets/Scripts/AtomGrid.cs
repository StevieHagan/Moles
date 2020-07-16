using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AtomGrid : MonoBehaviour
{
    int gridSize;
    Atom[,] atoms;

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
        bool unbondedAtomExists = false;

        for(int x = 0; x < gridSize; x++)
        {
            for(int y = 0; y < gridSize; y++)
            {
                if (atoms[x, y] == null) { continue; }
                if (!atoms[x, y].IsFullyBonded())
                {
                    unbondedAtomExists = true;
                }
            }
        }
        if(!unbondedAtomExists)
        {
            print("YOU WIN!!");
        }
    }

    public Atom GetAtomAtGridPos(Vector2Int gridPosition)
    {
        return atoms[gridPosition.x, gridPosition.y];
    }
}
