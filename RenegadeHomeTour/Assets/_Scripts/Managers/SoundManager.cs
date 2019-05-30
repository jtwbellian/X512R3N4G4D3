using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : EVActor
{
    public static SoundManager instance;
    public AudioSource music, player, dialogue, environment;
    public AudioClip song_fortuna, song_music, deathClip;

    void Awake()
    {
        if (instance == this)
        {
            Init();
            return;
        }

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
            instance = this;
        }

        Init();
        
    }

    public static SoundManager GetInstance()
    {
        return instance;
    }

    public override void BeginEvent()
    {
        if (myEvent.myName == "fortuna")
        {
            PlayFortune();
            CompleteEvent();
        }
    }

    public void PlayFortune()
    {
        music.Stop();
        music.clip = song_fortuna;
        music.Play();
        Invoke("PlayMusic", 21f);
    }

    public void PlayDeathSnd()
    {
        player.PlayOneShot(deathClip);
    }


    public void PlayMusic()
    {
        music.Stop();
        music.clip = song_music;
        music.Play();
    }
 
    void Init()
    {
        EventManager.GetInstance().AudioEventBegin += OnAudioEvent;
        subscribesTo = AppliesTo.AUDIO;
    }

    // Handle audio events
    public void OnAudioEvent(EventInfo info)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayImpactSFX(AudioClip clip, GameObject obj)
    {
        AudioSource.PlayClipAtPoint(clip, obj.transform.position);
    }
}
