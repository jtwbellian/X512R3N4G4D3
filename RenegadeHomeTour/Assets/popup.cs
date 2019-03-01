using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popup : MonoBehaviour
{

    public Text textField;
    public AudioSource audioSource;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Popup Spawned");
        if (textField == null)
        {
            textField = GetComponentInChildren<Text>();
        }

        if (audioSource == null)
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }

        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(new Vector3(0f, 0.001f));
        }

        transform.LookAt(Camera.main.transform);
        audioSource.Play();
        Destroy(gameObject, 1f);
    }

    public void SetText(string text)
    {
        textField.text = text;
    }
}
