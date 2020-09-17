using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Tooltip("The proportion of squares which will remain empty")]
    [Range(0f, 1f)][SerializeField] float blockedDensity = 0.5f;
    [Tooltip("The proportion of squares initially marked as seeds")]
    [Range(0f, 0.2f)] [SerializeField] float seedDensity = 0.1f;
    [Tooltip("Probability a seeded square will bond into an empty square")]
    [Range(0f, 1f)] [SerializeField] float emptyProb = 0.75f;
    [Tooltip("Probability a seeded square will bond into an occupied square")]
    [Range(0f, 1f)] [SerializeField] float occupiedProb = 0.3f;
    [Tooltip("Proportion the probability of bonding decreases each time a bond is formed")]
    [Range(0f, 0.99f)] [SerializeField] float probabilityDropOff;
    [Tooltip("When selected generated atoms are displayed as non-playable schematic")][SerializeField] bool testMode;

    [SerializeField] Atom singleAtom;
    [SerializeField] Atom doubleCornerAtom;
    [SerializeField] Atom doubleStraightAtom;
    [SerializeField] Atom tripleAtom;
    [SerializeField] Atom quadAtom;


    //TODO these are for testing only, remove later
    [SerializeField] GameObject blockedSquare;
    [SerializeField] GameObject seededSquare;
    [SerializeField] GameObject voidBond;
    //END testing block

    LevelController controller;
    AtomGrid atomGrid;
    int gridSizeX;
    int gridSizeY;
    int gridSnap;
    PreAtom[,] atomSquares;

    private class PreAtom
    {
        public bool joinsUp, joinsRight, joinsDown, joinsLeft;
        public AtomType atomType;

        public PreAtom(AtomType aType)
        {
            atomType = aType;
            joinsUp = joinsRight = joinsDown = joinsLeft = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gridSizeX = Settings.GetGridSize().x;
        gridSizeY = Settings.GetGridSize().y;
        controller = FindObjectOfType<LevelController>();
        atomGrid = FindObjectOfType<AtomGrid>();
        gridSnap = controller.GetGridSnap();
        atomSquares = new PreAtom[gridSizeX, gridSizeY];
        probabilityDropOff = 1 - probabilityDropOff;
        PopulateArray();
        InstantiateAtoms();
    }

    private void PopulateArray()
    {
        //Insert blocked squares based on density selection, fill rest of array with empties
        for(int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                if(Random.value <= blockedDensity)
                {
                    atomSquares[x, y] = new PreAtom(AtomType.blocked);
                }
                else
                {
                    atomSquares[x, y] = new PreAtom(AtomType.empty);
                }
            }
        }
        //Insert seed squares based on density setting. There must be at least one.
        int i = 0;
        int numSeedSquares = Mathf.RoundToInt(gridSizeX * gridSizeY * seedDensity);

        do
        {
            int x = Random.Range(0, gridSizeX); int y = Random.Range(0, gridSizeY);
            atomSquares[x, y] = new PreAtom(AtomType.seeded);
            i++;
        } while (i < numSeedSquares);

        //iterate through all the seed squares and place preAtoms in them
        int seededSquares;
        int iteration = 0;
        do
        {
            seededSquares = 0;
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int x = 0; x < gridSizeX; x++)
                {
                    if (atomSquares[x, y].atomType == AtomType.seeded)
                    {
                        seededSquares++;
                        int bondsFormedFactor = 0;
                        //test if bonds already exist in this seeded square
                        if (atomSquares[x, y].joinsUp == true) { bondsFormedFactor++; atomSquares[x, y].atomType = AtomType.occupied; }
                        if (atomSquares[x, y].joinsRight == true) { bondsFormedFactor++; atomSquares[x, y].atomType = AtomType.occupied; }
                        if (atomSquares[x, y].joinsDown == true) { bondsFormedFactor++; atomSquares[x, y].atomType = AtomType.occupied; }
                        if (atomSquares[x, y].joinsLeft == true) { bondsFormedFactor++; atomSquares[x, y].atomType = AtomType.occupied; }
                        //test up for empty
                        if (y < gridSizeY - 1 && atomSquares[x, y + 1].atomType == AtomType.empty)
                        {
                            if (Random.value <= emptyProb * Mathf.Pow(probabilityDropOff, bondsFormedFactor))
                            {
                                atomSquares[x, y].atomType = AtomType.occupied;
                                atomSquares[x, y].joinsUp = true;
                                atomSquares[x, y + 1].atomType = AtomType.seeded;
                                atomSquares[x, y + 1].joinsDown = true;
                                bondsFormedFactor++;
                            }
                        }
                        //test up for occupied
                        if (y < gridSizeY - 1 && atomSquares[x, y + 1].atomType == AtomType.occupied)
                        {
                            if (Random.value <= occupiedProb * Mathf.Pow(probabilityDropOff, bondsFormedFactor))
                            {
                                atomSquares[x, y].atomType = AtomType.occupied;
                                atomSquares[x, y].joinsUp = true;
                                atomSquares[x, y + 1].joinsDown = true;
                                bondsFormedFactor++;
                            }
                        }
                        //test right for empty
                        if (x < gridSizeX - 1 && atomSquares[x + 1, y].atomType == AtomType.empty)
                        {
                            if (Random.value <= emptyProb * Mathf.Pow(probabilityDropOff, bondsFormedFactor))
                            {
                                atomSquares[x, y].atomType = AtomType.occupied;
                                atomSquares[x, y].joinsRight = true;
                                atomSquares[x + 1, y].atomType = AtomType.seeded;
                                atomSquares[x + 1, y].joinsLeft = true;
                                bondsFormedFactor++;
                            }
                        }
                        //test right for occupied
                        if (x < gridSizeX - 1 && atomSquares[x + 1, y].atomType == AtomType.occupied)
                        {
                            if (Random.value <= occupiedProb * Mathf.Pow(probabilityDropOff, bondsFormedFactor))
                            {
                                atomSquares[x, y].atomType = AtomType.occupied;
                                atomSquares[x, y].joinsRight = true;
                                atomSquares[x + 1, y].joinsLeft = true;
                                bondsFormedFactor++;
                            }
                        }
                        //test down for empty
                        if (y > 0 && atomSquares[x, y - 1].atomType == AtomType.empty)
                        {
                            if (Random.value <= emptyProb * Mathf.Pow(probabilityDropOff, bondsFormedFactor))
                            {
                                atomSquares[x, y].atomType = AtomType.occupied;
                                atomSquares[x, y].joinsDown = true;
                                atomSquares[x, y - 1].atomType = AtomType.seeded;
                                atomSquares[x, y - 1].joinsUp = true;
                                bondsFormedFactor++;
                            }
                        }
                        //test down for occupied
                        if (y > 0 && atomSquares[x, y - 1].atomType == AtomType.occupied)
                        {
                            if (Random.value <= occupiedProb * Mathf.Pow(probabilityDropOff, bondsFormedFactor))
                            {
                                atomSquares[x, y].atomType = AtomType.occupied;
                                atomSquares[x, y].joinsDown = true;
                                atomSquares[x, y - 1].joinsUp = true;
                                bondsFormedFactor++;
                            }
                        }
                        //test left for empty
                        if (x > 0 && atomSquares[x - 1, y].atomType == AtomType.empty)
                        {
                            if (Random.value <= emptyProb * Mathf.Pow(probabilityDropOff, bondsFormedFactor))
                            {
                                atomSquares[x, y].atomType = AtomType.occupied;
                                atomSquares[x, y].joinsLeft = true;
                                atomSquares[x - 1, y].atomType = AtomType.seeded;
                                atomSquares[x - 1, y].joinsRight = true;
                                bondsFormedFactor++;
                            }
                        }
                        //test left for occupied
                        if (x > 0 && atomSquares[x - 1, y].atomType == AtomType.occupied)
                        {
                            if (Random.value <= occupiedProb * Mathf.Pow(probabilityDropOff, bondsFormedFactor))
                            {
                                atomSquares[x, y].atomType = AtomType.occupied;
                                atomSquares[x, y].joinsLeft = true;
                                atomSquares[x - 1, y].joinsRight = true;
                                bondsFormedFactor++;
                            }
                        }
                        //if no bonds have been formed in this square, unseed it
                        if(atomSquares[x, y].atomType == AtomType.seeded)
                        {
                            atomSquares[x, y].atomType = AtomType.blocked;
                        }
                    }
                }
            }
            iteration++;
        } while (seededSquares > 0);

    }

    private void InstantiateAtoms()
    {
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 instantiationPosition = new Vector3((x+1) * gridSnap, (y+1) * gridSnap, 0);
                if (testMode)
                {
                    switch (atomSquares[x, y].atomType)
                    {
                        case AtomType.blocked:
                            Instantiate(blockedSquare, instantiationPosition, Quaternion.identity);
                            break;

                        case AtomType.seeded:
                            Instantiate(seededSquare, instantiationPosition, Quaternion.identity);
                            break;

                        case AtomType.occupied:
                            if (atomSquares[x, y].joinsUp == true)
                            {
                                Instantiate(voidBond, instantiationPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
                            }
                            if (atomSquares[x, y].joinsRight == true)
                            {
                                Instantiate(voidBond, instantiationPosition, Quaternion.Euler(new Vector3(0, 0, -90)));
                            }
                            if (atomSquares[x, y].joinsDown == true)
                            {
                                Instantiate(voidBond, instantiationPosition, Quaternion.Euler(new Vector3(0, 0, 180)));
                            }
                            if (atomSquares[x, y].joinsLeft == true)
                            {
                                Instantiate(voidBond, instantiationPosition, Quaternion.Euler(new Vector3(0, 0, 90)));
                            }

                            break;
                    }
                }
                else
                {
                    Atom newAtom;
                    int rotationOffset = Random.Range(0, 4);
                    Quaternion rotation = Quaternion.Euler(0, 0, rotationOffset * -90);
                    int numBonds = 0;
                    //count the bonds for this atom
                    if (atomSquares[x, y].joinsUp == true) { numBonds++; }
                    if (atomSquares[x, y].joinsRight == true) { numBonds++ ; }
                    if (atomSquares[x, y].joinsDown == true) { numBonds++ ; }
                    if (atomSquares[x, y].joinsLeft == true) { numBonds++ ; }

                    switch(numBonds)
                    {
                        case 1:
                            newAtom = Instantiate(singleAtom, instantiationPosition, rotation, atomGrid.transform);
                            newAtom.SetRotationOffset(rotationOffset);
                        break;
                        case 2:
                            if((atomSquares[x, y].joinsUp == true && atomSquares[x, y].joinsDown == true) ||
                                atomSquares[x, y].joinsLeft == true && atomSquares[x, y].joinsRight == true)
                            {
                                newAtom = Instantiate(doubleStraightAtom, instantiationPosition, rotation, atomGrid.transform);
                                newAtom.SetRotationOffset(rotationOffset);
                            }
                            else
                            {
                                newAtom = Instantiate(doubleCornerAtom, instantiationPosition, rotation, atomGrid.transform);
                                newAtom.SetRotationOffset(rotationOffset);
                            }
                        break;
                        case 3:
                            newAtom = Instantiate(tripleAtom, instantiationPosition, rotation, atomGrid.transform);
                            newAtom.SetRotationOffset(rotationOffset);
                        break;
                        case 4:
                            newAtom = Instantiate(quadAtom, instantiationPosition, rotation, atomGrid.transform);
                            newAtom.SetRotationOffset(rotationOffset);
                        break;
                    }
                }
            }
        }
    }
}
