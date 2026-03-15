using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 플레이어와 관련된 상태 UI 관리
public class PlayerUI : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private Image _fillHp;
    [SerializeField] private TMP_Text _maxHpText;
    [SerializeField] private TMP_Text _curHpText;
    [SerializeField] private Image _fillMp;
    [SerializeField] private TMP_Text _maxMpText;
    [SerializeField] private TMP_Text _curMpText;
    [SerializeField] private GameObject _damageUI;
    [SerializeField] private GameObject[] _skills;


    public void SetHp(int currentHp, int maxHp)
    {
        _curHpText.text = currentHp.ToString();
        _maxHpText.text = maxHp.ToString();
        _fillHp.fillAmount = (float)currentHp / maxHp;
    }

    public void SetMp(int currentMp, int maxMp)
    {
        _curMpText.text = currentMp.ToString();
        _maxMpText.text = maxMp.ToString();
        _fillMp.fillAmount = (float)currentMp / maxMp;
        SetSkill(currentMp);
    }

    private void SetSkill(int currentMp)
    {
        for (int i = 0; i < 4; i++)
        {
            _skills[i].SetActive(currentMp < (i + 1) * 10);
        }
    }

    public void TakeDamage()
    {
        _damageUI.SetActive(true);
    }
}
