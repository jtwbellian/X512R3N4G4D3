using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    private float invocationDelay = 0.8f;
    private bool hasBeenInvoked = false;
    private GameManager gm;
    private ObjectPooler OP;
    private CrabController crab;

    public static int totalSpawns = 0;
    public GameObject spawnObj;
    public float delay = 5f;
    public Transform initialTarget;
    public Vector3 initialForce;


    [SerializeField]
    public UnityEvent onSpawn;


  // Start is called before the first frame update
  void Start()
    {
        //StartSpawning();
        gm = GameManager.GetInstance();
    }
    
    void OnDrawGizmos()
    {
        var skinnedRenderer = spawnObj.GetComponentInChildren<SkinnedMeshRenderer>();
        var staticRenderer = spawnObj.GetComponentInChildren<MeshFilter>();

        Gizmos.color = Color.red;

        if (skinnedRenderer != null)
        {
            Mesh gizmoMesh = skinnedRenderer.sharedMesh;
            Gizmos.DrawWireMesh(gizmoMesh, 0, transform.position, transform.rotation, skinnedRenderer.transform.localScale);
        }
        else if (staticRenderer != null)
        {
            Mesh gizmoMesh = staticRenderer.sharedMesh;
            Gizmos.DrawWireMesh(gizmoMesh, 0, transform.position, transform.rotation, staticRenderer.transform.localScale);

        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
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

            CrabController crab = null;

            GameObject GO = ObjectPooler.SharedInstance.GetPooledObject((int)BT.crab);
            crab = GO.GetComponent<CrabController>();

            if (!GO)
                yield return new WaitForSeconds(delay);

            GO.SetActive(true);

            crab = GO.GetComponent<CrabController>();

            if (crab != null)
            {
                crab.transform.SetPositionAndRotation(transform.position, transform.rotation);
                crab.target = initialTarget;
            }

            hasBeenInvoked = false;
            totalSpawns++;
            yield return new WaitForSeconds(delay);
        }
    }

    public void StartSpawning()
    {
        StartCoroutine("Spawn");
        gm = GameManager.GetInstance();
        //gm.gameMode.switchesOn++;
    }

    public void StopSpawning()
    {
        StopCoroutine("Spawn");
        gm = GameManager.GetInstance();
        //gm.gameMode.switchesOn--;
    }

}
