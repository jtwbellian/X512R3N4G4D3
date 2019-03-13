using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource music, player, dialogue, environment;
    public AudioClip song_fortuna, song_music;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        Init();
    }

    public static SoundManager GetInstance()
    {
        return instance;
    }

    public void PlayFortune()
    {
        music.Stop();
        music.clip = song_fortuna;
        music.Play();
        Invoke("PlayMusic", 21f);
    }

    public void PlayMusic()
    {
        music.Stop();
        music.clip = song_music;
        music.Play();
    }

    void Init()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
