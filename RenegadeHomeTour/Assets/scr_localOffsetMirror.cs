using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_localOffsetMirror : MonoBehaviour
{

    public Transform target;
    public bool mirrorX, mirrorY, mirrorZ;

    private Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lastPos != target.localPosition)
        {
            transform.localPosition = new Vector3(target.localPosition.x * ((mirrorX) ? (-1) : (1)),
                                                    target.localPosition.y * ((mirrorY) ? (-1) : (1)),
                                                    target.localPosition.z * ((mirrorZ) ? (-1) : (1)));

            lastPos = target.localPosition;
        }
 
    }
}
