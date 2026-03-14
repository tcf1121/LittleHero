using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// 인게임(전투) 관리
public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;
    public int StageMonsterNum;
    public int ChestNum;
    public UnityAction DieMonster;
    [SerializeField] private AudioClip _ingameBGM;
    [SerializeField] private TMP_Text _monterNumUI;
    [SerializeField] private TMP_Text _chestNumUI;
    [SerializeField] private FinUI _finUI;

    void Awake()
    {
        Time.timeScale = 1f;
        Instance = this;
        DieMonster += CheckMonster;
        StageMonsterNum = 0;
        ChestNum = 0;
    }


    void Start()
    {
        // 배경음 재생
        SoundManager.Instance.PlayBGM(_ingameBGM);
    }

    // 몬스터 수 UI에 적용
    public void SetMonNum()
    {
        _monterNumUI.text = StageMonsterNum.ToString();
    }

    // 상자 나왔을 때 실행
    public void DropChest()
    {
        ChestNum++;
    }

    // 게임이 끝나고 UI 보여주기
    public void ShowFinUI(bool clear)
    {
        StartCoroutine(WaitCor(clear));
    }

    private IEnumerator WaitCor(bool clear)
    {
        yield return new WaitForSeconds(0.5f);
        if (clear) GameManager.Instance.CurrentStage++;
        Time.timeScale = 0f;
        _finUI.gameObject.SetActive(true);
        _chestNumUI.text = ChestNum.ToString();
        _finUI.SetFin(clear);
        GameManager.Instance.Chest.GetChest(ChestNum);
    }

    // 몬스터가 죽었을 때 실행
    private void CheckMonster()
    {
        StageMonsterNum--;
        _monterNumUI.text = StageMonsterNum.ToString();
        // 몬스터를 다 죽이면 끝내기
        if (StageMonsterNum <= 0)
        {
            ShowFinUI(true);
        }
    }
}
