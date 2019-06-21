using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_bullet : MonoBehaviour
{

    //public int bounces = 0;
    //private int num_bounces;
    public int partType = 1;

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

        switch(partType)
        {
            case 0: break;
            case 1: FXManager.GetInstance().Burst(FXManager.FX.RadialBurst, pos, transform.rotation.eulerAngles, 1); break;
            case 2: FXManager.GetInstance().Burst(FXManager.FX.Shock, pos, transform.rotation.eulerAngles, 5); break;
        }

        Destroy();//gameObject);
    }
    
    void Destroy()
    {
        gameObject.SetActive(false);
        CancelInvoke();
    }
}
