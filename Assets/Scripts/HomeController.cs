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

    // Start is called before the first frame update

    private void Start()
    {
        size = Settings.GetSize();
        electronsOn = Settings.GetElectrons();
        UpdateButtons();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1); //start the game
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
                buttons[i].image.color = Color.magenta;
            }
            else
            {
                buttons[i].image.color = Color.white;
            }
        }
    }
}
