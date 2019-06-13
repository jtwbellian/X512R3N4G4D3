using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_boundary : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnColliderEnter(Collider col)
    {
        if (col.gameObject.tag == "UNAVAILABLE")
        {
            var gm = GameManager.GetInstance();
            gm.CreatePopup(gm.hud.hudAnchor.position, "Return to the center", 2f);
        }
    }

}
