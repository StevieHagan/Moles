using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    int playSize;
    void Start()
    {
        playSize = Settings.GetSize();

        InitialiseCameraPosition();
    }

    void InitialiseCameraPosition()
    {
        switch(playSize)
        {
            case Settings.SMALL:
                gameObject.transform.position = new Vector3(13.37f, 4.05f, -15.95f);
                break;
            case Settings.MEDIUM:
                gameObject.transform.position = new Vector3(17.61f, 5.3f, -23.45f);
                break;
            case Settings.LARGE:
                gameObject.transform.position = new Vector3(20.72f, 5.77f, -27.72f);
                break;
        }
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(-5.218f, -10.454f, -0.224f));
    }

}
