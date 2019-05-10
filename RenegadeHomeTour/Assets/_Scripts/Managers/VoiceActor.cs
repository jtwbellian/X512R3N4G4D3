using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceActor : EVActor
{
    public AudioSource source;
    public List<AudioClip> lines;
    [SerializeField]
    private int currentLine = 0;

    // Start is called before the first frame update
    void Start()
    {
        subscribesTo = AppliesTo.AUDIO;
    }

    public override void BeginEvent()
    {
        // no more lines, unsubscribe because this actor is done 
        if (currentLine >= lines.Count)
            Unsubscribe();

        if (myEvent.type == EV.audioStart)
        {
            source.PlayOneShot(lines[currentLine]);
            currentLine++;
            CompleteEvent();
        }
        else if (myEvent.type == EV.audioWait)
        {
            source.PlayOneShot(lines[currentLine]);
            Invoke("CompleteEvent", lines[currentLine].length);
            currentLine++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
