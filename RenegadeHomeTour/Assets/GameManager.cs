using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public GameObject popupPrefab;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init()
    {

    }

    public void CreatePopup(Vector3 pos, string message, float time)
    {
        popup pop = Instantiate(popupPrefab).GetComponent<popup>();
        pop.transform.position = pos;
        pop.SetText(message);
        Destroy(pop, 5f);
    }

    public static GameManager GetInstance()
    {
        return instance;
    }
}
