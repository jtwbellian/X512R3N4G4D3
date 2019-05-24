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
            source.clip = lines[currentLine];
            source.Play();
            currentLine++;
            CompleteEvent();
        }
        else if (myEvent.type == EV.audioWait)
        {
            source.clip = lines[currentLine];
            source.Play();
            Invoke("CompleteEvent", lines[currentLine].length);
            currentLine++;
        }
    }

    #region eventDesignerTools

    [ContextMenu("InsertAtCurrentLine")]
    private void InsertEvent()
    {
        lines.Insert(currentLine, lines[currentLine]);
    }

    [ContextMenu("RemoveAtCurrentLine")]
    private void RemoveEvent()
    {
        lines.RemoveAt(currentLine);
    }
#endregion

    // Update is called once per frame
    void Update()
    {
        
    }
}
