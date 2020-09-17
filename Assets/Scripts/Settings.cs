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
    const string GAME_SIZE = "gameSize", ELECTRONS_ON = "electronsOn", MUSIC_ON = "musicOn",
                 SMALL_COMPLETED = "smlCompleted", MED_COMPLETED = "medCompleted", LARGE_COMPLETED = "lrgCompleted";

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

    public static void SetElectrons(bool isOn)
    {
        PlayerPrefs.SetInt(ELECTRONS_ON, isOn ? 1 : 0);
    }

    public static bool GetElectrons()
    {
        return (PlayerPrefs.GetInt(ELECTRONS_ON, 0) == 1);
    }

    public static void SetMusic(bool isOn)
    {
        PlayerPrefs.SetInt(MUSIC_ON, isOn ? 1 : 0);
        FindObjectOfType<MusicPlayer>().UpdateMusicPlayState(isOn);
    }

    public static bool GetMusic()
    {
        return (PlayerPrefs.GetInt(MUSIC_ON, 1) == 1);
    }

    public static void IncrementCompleted(int size)
    {//Increments the number of levels completed for the puzzle size taken as argument
        switch (size)
        {
            case SMALL:
                PlayerPrefs.SetInt(SMALL_COMPLETED, PlayerPrefs.GetInt(SMALL_COMPLETED, 0) + 1);
                break;
            case MEDIUM:
                PlayerPrefs.SetInt(MED_COMPLETED, PlayerPrefs.GetInt(MED_COMPLETED, 0) + 1);
                break;
            case LARGE:
                PlayerPrefs.SetInt(LARGE_COMPLETED, PlayerPrefs.GetInt(LARGE_COMPLETED, 0) + 1);
                break;
            default:
                Debug.LogError("Size out of range, counter has not been incremented.");
                return;
        }
    }

    public static Vector3Int GetCompleted()
    {//returns the number of puzzles completed by the player as Vector3Int where x=small y=med z=large
        return new Vector3Int(PlayerPrefs.GetInt(SMALL_COMPLETED, 0),
                              PlayerPrefs.GetInt(MED_COMPLETED, 0),
                              PlayerPrefs.GetInt(LARGE_COMPLETED, 0));
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
