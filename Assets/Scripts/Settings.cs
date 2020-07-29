using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    
    public const int SMALL = 1, MEDIUM = 2, LARGE = 3;
    const int SMALL_GRID_SIZE_X = 8;
    const int SMALL_GRID_SIZE_Y = 5;
    const int MEDIUM_GRID_SIZE_X = 11;
    const int MEDIUM_GRID_SIZE_Y = 7;
    const int LARGE_GRID_SIZE_X = 14;
    const int LARGE_GRID_SIZE_Y = 8;

    //Keys
    const string GAME_SIZE = "gameSize", ELECTRONS_ON = "electronsOn";

    public static void SetSize(int size)
    {
        if (size >= SMALL && size <= LARGE)
        {
            PlayerPrefs.SetInt(GAME_SIZE, size);
        }
        else
        {
            Debug.LogWarning("Size out of range, setting to medium");
            PlayerPrefs.SetInt(GAME_SIZE, MEDIUM);
        }
    }

    public static int GetSize()
    {
        int size = PlayerPrefs.GetInt(GAME_SIZE, MEDIUM);

        if (size >= SMALL && size <= LARGE)
        {
            return size;
        }
        else
        {
            Debug.LogWarning("Size out of range, returning medium");
            return MEDIUM;
        }
    }

    public static void SetElectrons(bool on)
    {
        PlayerPrefs.SetInt(ELECTRONS_ON, on ? 1 : 0);
    }

    public static bool GetElectrons()
    {
        return (PlayerPrefs.GetInt(ELECTRONS_ON, 1) == 1) ? true : false;
    }

    public static Vector2Int GetGridSize()
    {
        int size = GetSize();

        switch(size)
        {
            case SMALL:
                return new Vector2Int(SMALL_GRID_SIZE_X, SMALL_GRID_SIZE_Y);

            case MEDIUM:
                return new Vector2Int(MEDIUM_GRID_SIZE_X, MEDIUM_GRID_SIZE_Y);

            case LARGE:
                return new Vector2Int(LARGE_GRID_SIZE_X, LARGE_GRID_SIZE_Y);

            default:
                Debug.LogWarning("Settings.GetGridSize() was passed an invalid size paramater");
                return new Vector2Int(MEDIUM_GRID_SIZE_X, MEDIUM_GRID_SIZE_Y);
        }
    }
}
