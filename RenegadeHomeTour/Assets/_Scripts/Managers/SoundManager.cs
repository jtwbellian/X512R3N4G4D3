using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource music, player, dialogue;

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

    void Init()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
