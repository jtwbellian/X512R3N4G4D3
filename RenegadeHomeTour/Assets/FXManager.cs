using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    static FXManager instance = null;
    public ParticleSystem[] part_systems;

    // Start is called before the first frame update
    void Start()
    {
        if (FXManager.GetInstance() == null)
        {
            instance = this;
        }
        else
            Destroy(this);

        if (part_systems == null)
        {
            part_systems = GetComponentsInChildren<ParticleSystem>();
        }
    }

    public static FXManager GetInstance()
    {
        return instance;
    }

    public void Burst(int type, Vector3 pos, Vector3 rot, int amt)
    {
        Debug.Log("Parts emit");
        var emitter = new ParticleSystem.EmitParams(); //part_systems[type];
        emitter.position = pos;
        emitter.rotation3D = rot;
        part_systems[type].Emit(emitter, amt);
        part_systems[type].Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
