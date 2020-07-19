using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bond : MonoBehaviour
{
    [SerializeField] Directions relativeDirection;
    [SerializeField] Vector2Int absoluteDirection;
    [SerializeField] Vector2Int pointsTo;
    [SerializeField] bool isBonded;

    Vector2Int[] absolouteDirections = {Vector2Int.up, Vector2Int.right, Vector2Int.down, 
                    Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down,};

    Atom parent;
    AtomGrid grid;
    Animator animator;

    void Start()
    {
        grid = FindObjectOfType<AtomGrid>();
        grid.AddBond(this);

        parent = GetComponentInParent<Atom>();
        animator = GetComponent<Animator>();

        UpdatePointsTo();
        UpdateIsBonded();
    }

    private void UpdatePointsTo()
    {
        absoluteDirection = absolouteDirections[(int)relativeDirection + parent.GetRotationOffset()];
        pointsTo = parent.GetGridPos() + absoluteDirection;
    }
    
    private void UpdateIsBonded()
    {
        grid.EvaluateBonds();
    }

    public void UpdateBonds() //Called from Broadcast in Atom.cs
    {
        UpdatePointsTo();
        UpdateIsBonded();
    }

    public Vector2Int GetPointsTo() { return pointsTo; }

    public void SetBonded(bool bonded) 
    { 
        isBonded = bonded;
        animator.SetBool("bonded", isBonded);
    }
    public bool GetBonded() { return isBonded; }

}
