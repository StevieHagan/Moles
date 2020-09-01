using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] Toggle electronToggle;

    int size;
    bool electronsOn;
    Color whiteButtonBackground;
    Color yellowButtonBackground;

    // Start is called before the first frame update

    private void Start()
    {
        size = Settings.GetSize();
        electronsOn = Settings.GetElectrons();
        whiteButtonBackground = new Color(1, 1, 1, 0.471f);
        yellowButtonBackground = new Color(1, 0.92f, 0.016f, 0.471f);
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

    private void UpdateButtons()
    {
        electronToggle.SetIsOnWithoutNotify(electronsOn); 

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
}
