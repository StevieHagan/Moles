using UnityEngine;

[ExecuteInEditMode] //This script is for snapping in Editor mode.
[SelectionBase] //Prevent accidental selection of children.
[RequireComponent(typeof(Atom))]
public class AtomGridSnapper : MonoBehaviour
{
    Atom atom;
    int gridSize;

    void Awake()
    {
        atom = GetComponent<Atom>();
        gridSize = FindObjectOfType<LevelController>().GetGridSize();
        transform.parent = FindObjectOfType<Atoms>().gameObject.transform;
    }

    void Update()
    {
        SnapToGrid();
        RenameAtom();
    }

    private void SnapToGrid()
    {
        transform.position = (new Vector3(atom.GetGridPos().x * gridSize,
                                          atom.GetGridPos().y * gridSize, 0f));
    }

    private void RenameAtom()
    {
        gameObject.name = "Atom " + tag + " " + atom.GetGridPos().ToString();
    }
}
