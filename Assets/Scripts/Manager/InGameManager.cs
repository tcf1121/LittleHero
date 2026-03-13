using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;
    public int StageMonsterNum;
    public UnityAction DieMonster;
    [SerializeField] private TMP_Text _monterNumUI;
    [SerializeField] private FinUI _finUI;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1f;
        Instance = this;
        DieMonster += CheckMonster;
        StageMonsterNum = 0;
    }

    public void SetMonNum()
    {
        _monterNumUI.text = StageMonsterNum.ToString();
    }

    public void StageClear()
    {
        Time.timeScale = 0f;
        GameManager.Instance.CurrentStage++;
        _finUI.gameObject.SetActive(true);
        _finUI.SetFin(true);
    }

    public void StageFail()
    {
        Time.timeScale = 0f;
        _finUI.gameObject.SetActive(true);
        _finUI.SetFin(false);
    }

    private void CheckMonster()
    {
        StageMonsterNum--;
        _monterNumUI.text = StageMonsterNum.ToString();
        if (StageMonsterNum <= 0)
        {
            StageClear();
        }
    }
}
