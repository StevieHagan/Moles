using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1;
    int rotationOffset = 0;
    int gridSize;
    Vector2Int gridPos; 


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotationAnimator();
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
            gridSize = FindObjectOfType<LevelController>().GetGridSize();
        }

        gridPos.x = Mathf.RoundToInt(transform.position.x / gridSize);
        gridPos.y = Mathf.RoundToInt(transform.position.y / gridSize);

        return gridPos;
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {//When the left mouse button is clicked, the rotation offset is increased (cycles back to 0 from 3)

            rotationOffset = (rotationOffset == 3 ? 0 : rotationOffset + 1); 
                
            print("You clicked on " + name + " Which now has a rotation offset of " + rotationOffset);
        }
    }
}
