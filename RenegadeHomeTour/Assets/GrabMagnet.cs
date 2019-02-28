using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GrabMagnet : MonoBehaviour
{
    public MeshRenderer orb;
    public bool holdsHat;
    public bool holdsTool;
    private bool empty = true;
    private VRTool lastItem;
    

    public void Free()
    {
        Debug.Log("Holster Free");
        empty = true;

        if (orb != null)
            orb.enabled = true;
    }

    public bool IsFree()
    {
        return empty;
    }

    void OnTriggerStay(Collider col)
    {
        VRTool item;
        item = col.GetComponent<VRTool>();

        if (!empty)
        {
            // To fix the glitch where the holsters bug out
            if (lastItem.home != this )
            {
                // It is a child so fix the home
                if (lastItem.transform.IsChildOf(transform))
                    lastItem.home = this;
                else
                    Free();
            }

            return;
        }



        if (item == null || 
        item.isHeld() || 
        (item.isHat && !holdsHat) ||
        (!item.isHat && !holdsTool))
            return;

        // grab a tool 
        if (col.transform.parent != transform)
        {
            lastItem = item;
            
            if (item.home != null)
                item.home.Free();

            Free();

            item.SetHome(this); 
            
            empty = false;

            if (orb != null)
            {
                orb.enabled = false;
            }

            Debug.Log("Home Set to "  + this.ToString());
        }


    }


}
