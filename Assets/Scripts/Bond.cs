using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bond : MonoBehaviour
{
    [SerializeField] Directions relativeDirection;
    [SerializeField] Vector2Int absoluteDirection;
    [SerializeField] Vector2Int pointsTo;

    Vector2Int[] absolouteDirections = {Vector2Int.up, Vector2Int.right, Vector2Int.down, 
                    Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down,};

    Atom parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponentInParent<Atom>();
        UpdatePointsTo();
    }

    private void UpdatePointsTo()
    {
        absoluteDirection = absolouteDirections[(int)relativeDirection + parent.GetRotationOffset()];
        pointsTo = parent.GetGridPos() + absoluteDirection;
    }

    public void UpdateBonds() //Called from Broadcast in Atom.cs
    {
        UpdatePointsTo();
    }

    public Vector2Int GetPointsTo() { return pointsTo; }

    // Update is called once per frame
    void Update()
    {
        
    }
}
