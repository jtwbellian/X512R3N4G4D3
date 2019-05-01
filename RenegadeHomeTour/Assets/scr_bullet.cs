using System.Collections;
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
