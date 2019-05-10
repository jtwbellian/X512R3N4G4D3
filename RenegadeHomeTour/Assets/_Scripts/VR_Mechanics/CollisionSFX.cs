using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSFX : MonoBehaviour
{
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnCollisionEnter(Collision col)
    {
        var sm = SoundManager.GetInstance();

            Debug.Log("sound is played!");
            sm.PlayImpactSFX(clip, gameObject);
       // }
    }
}
