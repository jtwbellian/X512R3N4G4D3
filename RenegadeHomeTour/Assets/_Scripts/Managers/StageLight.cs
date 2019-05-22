using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLight : EVActor
{
    private Light lightSource;

    // Start is called before the first frame update
    void Start()
    {
        lightSource = GetComponent<Light>();
        subscribesTo = AppliesTo.ENV;
    }

    public override void BeginEvent()
    {
        
    }
}
