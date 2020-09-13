using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Atom : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1;
    [SerializeField] int rotationOffset = 0;
    [SerializeField] Animator[] electronAnimators;
    [SerializeField] AudioClip clickSound;
    AtomGrid grid;
    Bond[] bonds;
    int gridSize;
    Vector2Int gridPos;


    void Start()
    {        
        InitialiseOffsets();
        grid = FindObjectOfType<AtomGrid>();
        bonds = GetComponentsInChildren<Bond>();
        electronAnimators = GetComponentsInChildren<Animator>();
    }

    void Update()
    {
        RotationAnimator();
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {//When the left mouse button is clicked, the rotation offset is increased (cycles back to 0 from 3)

            rotationOffset = (rotationOffset == 3 ? 0 : rotationOffset + 1);
            //click/tap sound
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);

            UpdateBonds();
            grid.CheckWinCondition();
        }
    }
    private void RotationAnimator()
    {//Rotates the atom using a slerp for smooth animation, target rotation is -90 * offset.

        if (Mathf.Abs(transform.rotation.eulerAngles.z - rotationOffset * -90) <= Mathf.Epsilon)
        {
            return;
        }
        if (Mathf.Abs(transform.rotation.eulerAngles.z - rotationOffset * -90) <= 0.1)
        {
            transform.rotation = Quaternion.Euler(0, 0, rotationOffset * -90);
            return;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, rotationOffset * -90f),
                                                      rotationSpeed * Time.deltaTime);
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

    private void UpdateBonds()
    {
        foreach (Bond bond in bonds)
        {
            bond.UpdateBonds();
        }
    }

    public void SetRotationOffset(int offset)
    {
        if (offset < 0 || offset > 3)
        {
            Debug.LogError("Rotation offset out of range. Corrected, but game may be unwinnable");
        }
        rotationOffset = Mathf.Clamp(offset, 0, 3);
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

    public int GetRotationOffset() { return rotationOffset; }

    public void SetFullyBonded(bool fully)
    {

        //Skip setting electron animations if they are switched off in settings.
        if(!Settings.GetElectrons()) { return; }

        foreach(Animator animator in electronAnimators)
        {
            animator.SetBool("orbitOn", fully);
        }
    }

}
