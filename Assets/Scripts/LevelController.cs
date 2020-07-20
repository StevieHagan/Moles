using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    const int GRID_SNAP = 2;
    [SerializeField] int gridSize = 10;
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

    public void DisplayWinCanvas()
    {
        winCanvas.enabled = true;
    }

    public void LoadNewPuzzle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
