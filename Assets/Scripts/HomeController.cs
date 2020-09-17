using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] Canvas mainCanvas;
    [SerializeField] Canvas infoCanvas;
    [SerializeField] TMP_Text counterText;
    [SerializeField] Toggle electronToggle;
    [SerializeField] Toggle musicToggle;

    int size;
    bool electronsOn;
    bool musicOn;
    Color whiteButtonBackground;
    Color yellowButtonBackground;

    // Start is called before the first frame update

    private void Start()
    {
        size = Settings.GetSize();
        electronsOn = Settings.GetElectrons();
        musicOn = Settings.GetMusic();
        whiteButtonBackground = new Color(1, 1, 1, 0.471f);
        yellowButtonBackground = new Color(1, 0.92f, 0.016f, 0.471f);

        mainCanvas.enabled = true;
        infoCanvas.enabled = false;
        UpdateButtons();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1); //start the game
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ShowInfo(bool show)
    {
        infoCanvas.enabled = show;
        UpdateCompletedCounterText();
    }


    public void SizeButtonClick(int newSize)
    {
        size = newSize;
        Settings.SetSize(newSize);
        UpdateButtons();
    }

    public void ElectronToggle(bool isOn)
    {
        electronsOn = isOn;
        Settings.SetElectrons(isOn);
    }

    public void MusicToggle(bool isOn)
    {
        musicOn = isOn;
        Settings.SetMusic(isOn);
    }

    private void UpdateButtons()
    {
        electronToggle.SetIsOnWithoutNotify(electronsOn);
        musicToggle.SetIsOnWithoutNotify(musicOn);

        for(int i = 0; i < Settings.LARGE; i++)
        {
            if(size == i + 1)
            {
                buttons[i].image.color = yellowButtonBackground;
            }
            else
            {
                buttons[i].image.color = whiteButtonBackground;
            }
        }
    }

    private void UpdateCompletedCounterText()
    {
        Vector3Int counts = Settings.GetCompleted();

        counterText.text = "Puzzles completed:\n" +
                    "Small: " + counts.x.ToString() + "    Med: " + counts.y.ToString() + 
                    "    Large: " + counts.z.ToString();
    }
}
