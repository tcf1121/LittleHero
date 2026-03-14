using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;
    public int StageMonsterNum;
    public int ChestNum;
    public UnityAction DieMonster;
    [SerializeField] private TMP_Text _monterNumUI;
    [SerializeField] private TMP_Text _chestNumUI;
    [SerializeField] private FinUI _finUI;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1f;
        Instance = this;
        DieMonster += CheckMonster;
        StageMonsterNum = 0;
        ChestNum = 0;
    }

    public void SetMonNum()
    {
        _monterNumUI.text = StageMonsterNum.ToString();
    }

    public void DropChest(GameObject chest)
    {
        ChestNum++;
    }

    public void ShowFinUI(bool clear)
    {
        if (clear) GameManager.Instance.CurrentStage++;
        Time.timeScale = 0f;
        _finUI.gameObject.SetActive(true);
        _chestNumUI.text = ChestNum.ToString();
        _finUI.SetFin(clear);
        GameManager.Instance.Chest.GetChest(ChestNum);
    }

    private void CheckMonster()
    {
        StageMonsterNum--;
        _monterNumUI.text = StageMonsterNum.ToString();
        if (StageMonsterNum <= 0)
        {
            ShowFinUI(true);
        }
    }
}
