using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameMode { crabs, sandbox};

public delegate void RespawnPlayerHandler();
public delegate void PlayerDieHandler();

public class GameManager : EVActor
{
    public static bool isPaused = false;
    public CrabGame gameMode;
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
    public SkinnedMeshRenderer playerNArmor;

    [Header("Crabtastrophe Game")]
    public int numLocked = 8; 
    public scr_button[] airLockButtons;
    public AirVent [] airVents;
    public Light redLight;
    public GameObject [] fan;
    public GameObject destroyedFan;
    public GameObject mamaCrab;
    public AudioClip bossSpawnSound;

    public event RespawnPlayerHandler OnPlayerRespawn;
    public event PlayerDieHandler OnPlayerDie;

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
        subscribesTo = AppliesTo.PLAYER;
        myName = "GM";
    }

    public void ChangeSkin(Material mat)
    {
        playerHelmet.materials.SetValue(mat, 1);
        playerHelmet.materials[1] = mat;
        playerArmor.material = mat;
        playerNArmor.material = mat;
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

    public void UpdatePlayerCols(Collider [] cols)
    {
        playerCols = cols;
    }

    public void PlayerDie()
    {
        playerIsDead = true;
        FXManager.GetInstance().Burst(FXManager.FX.Shock, Camera.main.transform.position, 10);
        sm.PlayDeathSnd();
        OnPlayerDie.Invoke();
        Invoke("Respawn", 5f);
        Time.timeScale = 0.5f;
        sm.music.pitch = 0.5f;
        sm.music.volume = 0.1f;
    }

    public void Respawn()
    {
        FXManager.GetInstance().Burst(FXManager.FX.Shock, Camera.main.transform.position, 25);
        Time.timeScale = 1f;
        sm.music.pitch = 1f;
        sm.music.volume = 0.5f;
        playerIsDead = false;
        OnPlayerRespawn.Invoke();
    }

    [ContextMenu("Spawn Boss")]
    public void SpawnBoss()
    {
        sm.environment.PlayOneShot(bossSpawnSound);

        foreach (GameObject f in fan)
            f.SetActive(false);

        destroyedFan.SetActive(true);
        mamaCrab.gameObject.SetActive(true);
    }

    public void BossKilled()
    {
        if (myEvent.type == EV.GameEnd)
        {
            sm.music.volume = 0f;
            CompleteEvent();
        }
    }

    [ContextMenu("Spawn Enemies")]
    public void ActivateButtons()
    {
        StartCoroutine(G_ActivateButtons(1));
    }

    public static GameManager GetInstance()
    {
        return instance;
    }

    void Update()
    {
        if (!isPaused && !OVRManager.hasInputFocus)
        {
            Pause();
        }
        else if (isPaused && OVRManager.hasInputFocus)
        {
            UnPause();
        }

        if (myEvent != null)
            if (numLocked >= 8 && myEvent.type == EV.targetHit)
            {
                CompleteEvent();
            }
    }

    public void Pause()
    {
        GameManager.isPaused = true;
        AudioListener.pause = true;
        Time.timeScale = 0.000001f;
    }

    public void UnPause()
    {
        GameManager.isPaused = false;
        AudioListener.pause = false;
        Time.timeScale = 1f;
    }

    public override void BeginEvent()
    {
        switch(myEvent.type)
        {
            // Pregame 
            case EV.audioStart:
                redLight.color = Color.red;
                StartCoroutine(G_ShakeVents(1));
                CompleteEvent();
                break;

            case EV.GameStart:
                sm.PlayFortune();
                StartCoroutine(G_ActivateButtons(1));
                CompleteEvent();
                hud.ShowKills();
                break;

            case EV.EntersTrigger:
                SpawnBoss();
                CompleteEvent();
                break;
        }
    }

    IEnumerator G_ShakeVents(float delay)
    {
        foreach (AirVent a in airVents)
        {
            a.StartShaking();
            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }


    IEnumerator G_ActivateButtons(float delay)
    {
        foreach (scr_button a in airLockButtons)
        {
            a.Turn(true);
            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }
}
