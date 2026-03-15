using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 인벤토리 UI
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private List<EquipBtn> _headEquipBtns;
    [SerializeField] private List<EquipBtn> _armorEquipBtns;
    [SerializeField] private List<EquipBtn> _shieldEquipBtns;
    [SerializeField] private List<EquipBtn> _weaponEquipBtns;
    [SerializeField] private TMP_Text _manaText;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private TMP_Text _pushText;
    [SerializeField] private TMP_Text _damageText;

    void Start()
    {
        GameManager.Instance.RefreshEquip += RefreshList;
    }

    void OnEnable()
    {
        GenerateList(_headEquipBtns, GameManager.Instance.Inventory.GetEquipList(0));
        GenerateList(_armorEquipBtns, GameManager.Instance.Inventory.GetEquipList(1));
        GenerateList(_shieldEquipBtns, GameManager.Instance.Inventory.GetEquipList(2));
        GenerateList(_weaponEquipBtns, GameManager.Instance.Inventory.GetEquipList(3));
    }

    // 장비 아이콘 새로고침 (획득 여부, 장착 여부)
    private void GenerateList(List<EquipBtn> equipBtns, List<EquipmentData> equipment)
    {
        for (int i = 0; i < equipBtns.Count; i++)
        {
            equipBtns[i].SetIcon(equipment[i]);
        }
    }

    // 장비 장착 현황 새로고침
    private void RefreshList(EquipmentData equipment)
    {
        switch (equipment.equipType)
        {
            case EquipmentType.Head:
                GenerateList(_headEquipBtns, GameManager.Instance.Inventory.GetEquipList(0));
                _manaText.text = GameManager.Instance.Inventory.GetStat().MpRegen.ToString();
                break;
            case EquipmentType.Armor:
                GenerateList(_armorEquipBtns, GameManager.Instance.Inventory.GetEquipList(1));
                _hpText.text = GameManager.Instance.Inventory.GetStat().Hp.ToString();
                break;
            case EquipmentType.Shield:
                GenerateList(_shieldEquipBtns, GameManager.Instance.Inventory.GetEquipList(2));
                _pushText.text = GameManager.Instance.Inventory.GetStat().Push.ToString();
                break;
            case EquipmentType.Weapon:
                GenerateList(_weaponEquipBtns, GameManager.Instance.Inventory.GetEquipList(3));
                _damageText.text = GameManager.Instance.Inventory.GetStat().Damage.ToString();
                break;
            default:
                break;
        }
    }
}
