using UnityEngine;

[ExecuteInEditMode] //This script is for snapping in Editor mode.
[SelectionBase] //Prevent accidental selection of children.
[RequireComponent(typeof(Atom))]
public class AtomGridSnapper : MonoBehaviour
{
    int gridSize;
    Atom atom;
    int gridSnap;

    void Awake()
    {
        atom = GetComponent<Atom>();

        LevelController level = FindObjectOfType<LevelController>();
        gridSnap = level.GetGridSnap();
        gridSize = level.GetGridSize();

        transform.parent = FindObjectOfType<AtomGrid>().gameObject.transform;
    }

    void Update()
    {
        SnapToGrid();
        RenameAtom();
    }

    private void SnapToGrid()
    {
        int gridPosX = atom.GetGridPos().x;
        int gridPosY = atom.GetGridPos().y;

        gridPosX = Mathf.Clamp(gridPosX, 0, gridSize);
        gridPosY = Mathf.Clamp(gridPosY, 0, gridSize);

        transform.position = (new Vector3(gridPosX * gridSnap,
                                          gridPosY * gridSnap, 0f));
    }

    private void RenameAtom()
    {
        gameObject.name = tag + " " + atom.GetGridPos().ToString();
    }
}
