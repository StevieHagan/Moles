﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    const int GRID_SNAP = 2;
    [SerializeField] int gridSize = 10;
    [SerializeField] float winDelay = 2f;
    [SerializeField] Canvas winCanvas;

    private void Start()
    {
        winCanvas.enabled = false;
    }
    public int GetGridSnap()
    {
        return GRID_SNAP;
    }

    public int GetGridSize()
    {
        return gridSize;
    }

    public void WinThisLevel()
    {
        StartCoroutine(LockAndDisplayWin());
    }

    IEnumerator LockAndDisplayWin()
    {
        //Lock off all the atoms to prevent further turning.
        foreach(Atom atom in FindObjectsOfType<Atom>())
        {
            atom.GetComponent<SphereCollider>().enabled = false;
        }
        //after the delay display the winCanvas
        yield return new WaitForSeconds(winDelay);
        winCanvas.enabled = true;
    }

    public void LoadNewPuzzle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadHome()
    {
        SceneManager.LoadScene(0);
    }
}
