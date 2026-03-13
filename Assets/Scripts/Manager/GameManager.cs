using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null) _instance = new GameManager();
            return _instance;
        }
    }

    [SerializeField] private Inventory _inventory;
    [SerializeField] private List<StageData> _stages;

    public int CurrentStage;


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }


    public StageData GetStage()
    {
        return _stages[CurrentStage];
    }

    public PlayerStat GetStat()
    {
        return _inventory.GetStat();
    }

}
