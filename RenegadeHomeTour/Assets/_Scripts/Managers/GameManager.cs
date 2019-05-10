using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]


    public bool playerIsDead = false;
    public static int MAX_CRABS = 50;
    public static int num_crabs = 0;
    public static GameManager instance;
    public Vector3 playerStart;
    public GameObject popupPrefab;
    public Hud hud;
    public Collider [] playerCols;
    public SoundManager sm;
    public Director direc;
    public Material defaultPlayerMat;
    public MeshRenderer playerHelmet;
    public SkinnedMeshRenderer playerArmor;

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
        num_crabs--;
        crabsKilled++;
        hud.Refresh();
    }

    void Init()
    {
        //Camera.main.transform.root.GetComponentInChildren<Rigidbody>().MovePosition(playerStart);
    }

    public void ChangeSkin(Material mat)
    {
        Debug.Log("Helmet mat was " + playerHelmet.materials[1]);
        playerHelmet.materials.SetValue(mat, 1);
        Debug.Log("Helmet is now " + playerHelmet.materials[1]);
        playerArmor.material = mat;
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

    public void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void PlayerDie()
    {
        var fxMan = FXManager.GetInstance();
        //fxMan.Burst()
    }

    public static GameManager GetInstance()
    {
        return instance;
    }
}
