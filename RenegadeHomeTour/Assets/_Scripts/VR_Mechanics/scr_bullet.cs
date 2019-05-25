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
       // Destroy(gameObject, 2);
        Invoke("Destroy", 2f);
    }

    void OnCollisionEnter(Collision col)
    {

        /*if (num_bounces < bounces)
        {
            num_bounces++;
            return;
        }*/

        Vector3 pos = transform.position;

        //if (col.contactCount > 0)
        //    pos = col.GetContact(0).point;

        FXManager.GetInstance().Burst(FXManager.FX.RadialBurst, pos, transform.rotation.eulerAngles, 1);
        Destroy();//gameObject);
    }
    
    void Destroy()
    {
        gameObject.SetActive(false);
        CancelInvoke();
    }
}
