using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour
{
    private Animator animator;
    private Material [] mats;
    private bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        var renderer = GetComponentInChildren<Renderer>();
        mats = renderer.materials;
        //animator.set
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("laser") && alive)
        {
            StartCoroutine("Dissolve");
            alive = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Dissolve()
    {
        for (float i = 0f; i < 1f; i += 0.01f)
        {
            for(int m = 0; m <= mats.Length - 1; m ++)
            {
                mats[m].SetFloat("Vector1_69FA3116", i);
            }

            if (i > 0.8f)
            {
                Destroy(this.gameObject);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
