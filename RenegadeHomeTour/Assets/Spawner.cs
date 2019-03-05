using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Spawner : MonoBehaviour
{
    public static int totalSpawns = 0;
    public GameObject obj;
    public float delay = 5f;
    public Transform initialTarget;
    public Vector3 initialForce;
    [SerializeField]
    public UnityEvent onSpawn;
    private float invocationDelay = 0.8f;
    private bool hasBeenInvoked = false;

  // Start is called before the first frame update
  void Start()
    {
        //StartSpawning();
    }


    IEnumerator Spawn()
    {
        while (true)
        {
            // If has NOT been invoked, invoke the delegate and wait to spawn
            if (!hasBeenInvoked)
            {
                hasBeenInvoked = true;
                onSpawn.Invoke();
                yield return new WaitForSeconds(invocationDelay);
            }

            // Spawn the object and reset hasBeenInvoked
            GameObject instance = Instantiate(obj, transform.position, transform.rotation);
            instance.GetComponent<Rigidbody>().AddForce(initialForce);

            CrabController crab;
            crab = instance.GetComponent<CrabController>();

            if (crab != null)
                crab.target = initialTarget;

            hasBeenInvoked = false;
            totalSpawns++;
            yield return new WaitForSeconds(delay);
        }
    }

    public void StartSpawning()
    {
        StartCoroutine("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
