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
        if (!col.gameObject.isStatic)
            return;

        var sm = SoundManager.GetInstance();
        sm.PlayImpactSFX(clip, gameObject);
       // }
    }
}
