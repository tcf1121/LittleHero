using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipBtn : MonoBehaviour
{
    [SerializeField] private GameObject _lockedPanel;
    [SerializeField] private GameObject _equippedText;
    [SerializeField] private Button _btn;
    [SerializeField] private Image _equipmentSprite;
    private EquipmentData _equipment;

    public void SetIcon(EquipmentData equipment)
    {
        _equipment = equipment;
        _equipmentSprite.sprite = equipment.EquipSprite;
        _btn.onClick.AddListener(Equipped);
        RefreshIcon(equipment);
    }

    public void RefreshIcon(EquipmentData equipment)
    {
        _lockedPanel.SetActive(!equipment.Get);
        _equippedText.SetActive(equipment.Equipped);
    }

    private void Equipped()
    {
        GameManager.Instance.Installing.Invoke(_equipment);
    }
}
