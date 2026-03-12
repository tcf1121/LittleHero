using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private Image _fillHp;
    [SerializeField] private TMP_Text _maxHpText;
    [SerializeField] private TMP_Text _curHpText;
    [SerializeField] private Image _fillMp;
    [SerializeField] private TMP_Text _maxMpText;
    [SerializeField] private TMP_Text _curMpText;


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
    }
}
