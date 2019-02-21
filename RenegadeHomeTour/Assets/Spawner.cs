using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Spawner : MonoBehaviour
{

    public GameObject obj;
    public float delay = 1f;
    public Transform initialTarget;
    public Vector3 initialForce;
    [SerializeField]
    public UnityEvent onSpawn;

  // Start is called before the first frame update
  void Start()
    {
        StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            onSpawn.Invoke();
            GameObject instance = Instantiate(obj, transform.position, transform.rotation);
            instance.GetComponent<Rigidbody>().AddForce(initialForce);

            CrabController crab;
            crab = instance.GetComponent<CrabController>();

            if (crab != null)
                crab.target = initialTarget;

            yield return new WaitForSeconds(delay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
