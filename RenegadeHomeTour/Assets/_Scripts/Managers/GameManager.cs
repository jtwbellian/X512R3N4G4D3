using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public Transform playerStart;
    public GameObject popupPrefab;
    public Hud hud;
    public SoundManager sm;
    public Director direc;
    private bool popupShown = false;

    public static int crabsKilled = 0;

    // Start is called before the first frame update
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        Init();
    }

    public int GetCrabsKilled()
    {
        return crabsKilled;
    }

    public void IncrementKillCount()
    {
        crabsKilled++;
        hud.Refresh();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void Init()
    {

    }


    public void CreatePopup(Vector3 pos, string message, float time)
    {
        if (popupShown)
            return;

        popup pop = Instantiate(popupPrefab).GetComponent<popup>();
        pop.transform.position = pos;
        pop.SetText(message);

        popupShown = true;
        Invoke("CanShowPopups", 1f);
    }

    public void CanShowPopups()
    {
        popupShown = false;
    }

    public static GameManager GetInstance()
    {
        return instance;
    }
}
