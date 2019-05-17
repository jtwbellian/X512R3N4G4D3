using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_bullet : MonoBehaviour
{

    //public int bounces = 0;
    //private int num_bounces;

    // Start is called before the first frame update
    void OnEnable()
    {
        //num_bounces = 0;
        Invoke("Destroy", 2f);
    }

    void OnCollisionEnter(Collision col)
    {

        /*if (num_bounces < bounces)
        {
            num_bounces++;
            return;
        }*/

        var fxm = FXManager.GetInstance();

        Vector3 pos = transform.position;

        if (col.contactCount > 0)
            pos = col.GetContact(0).point;

        fxm.Burst(FXManager.FX.RadialBurst, pos, transform.rotation.eulerAngles, 4);
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
