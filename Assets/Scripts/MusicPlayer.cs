using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] tracks;

    AudioSource audioSource;

    private void Awake()
    {
        //Set this object up as a singleton

        if(FindObjectsOfType<MusicPlayer>().Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateMusicPlayState(Settings.GetMusic());
    }

    private void Update()
    {
        //If no track is currently playing, get a new one going
        if(!audioSource.isPlaying)
        {
            StartNewTrack();
        }
    }

    private void StartNewTrack()
    {
        //set the current clip at random
        int clipNumber = Random.Range(0, tracks.Length);
        audioSource.clip = tracks[clipNumber];
        audioSource.Play();
    }

    public void UpdateMusicPlayState(bool playing)
    {
        audioSource.mute = !playing;
    }
}
