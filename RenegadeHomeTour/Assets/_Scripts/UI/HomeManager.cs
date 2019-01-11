using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviour
{
    #region singleton

    private GameObject walls;
    private GameObject flooring;
    private GameObject[] doors;
    private GameObject[] blinds;
    private GameObject[] accentObjects1;
    private GameObject[] accentObjects2;

    public void Init()
    {
        GameObject walls = GameObject.FindWithTag("Floorplan");
        doors = GameObject.FindGameObjectsWithTag("door");
        blinds = GameObject.FindGameObjectsWithTag("blinds");
        accentObjects1 = GameObject.FindGameObjectsWithTag("Accent1");
        accentObjects2 = GameObject.FindGameObjectsWithTag("Accent2");

        Debug.Log("Walls->" + (walls == null ? "NULL" : "INIT") + "\n" +
                  "Doors->" + (doors == null ? "NULL" : "INIT") + "\n" +
                  "Blinds->" + (blinds == null ? "NULL" : "INIT") + "\n" +
                  "Accnt1->" + (accentObjects1 == null ? "NULL" : "INIT") + "\n" +
                  "Accnt2->" + (accentObjects2 == null ? "NULL" : "INIT") + "\n");
    }


    public static HomeManager instance;
    

    public static HomeManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        Init();
    }

    #endregion

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
