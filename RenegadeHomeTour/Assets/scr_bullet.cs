﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("Destroy", 2f);
    }

    void OnCollisionEnter(Collision col)
    {
        var fxm = FXManager.GetInstance();
        fxm.Burst(0, transform.position, transform.rotation.eulerAngles, 4);
        fxm.Burst(1, transform.position, transform.rotation.eulerAngles, 4);
        Destroy();
    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        CancelInvoke();
    }
}
