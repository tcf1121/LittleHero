using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] List<EquipBtn> _headEquipBtns;
    [SerializeField] List<EquipBtn> _armorEquipBtns;
    [SerializeField] List<EquipBtn> _shieldEquipBtns;
    [SerializeField] List<EquipBtn> _weaponEquipBtns;

    void Start()
    {
        GameManager.Instance.RefreshEquip += RefreshList;
    }

    void OnEnable()
    {
        GenerateList(_headEquipBtns, GameManager.Instance.GetEquipList(0));
        GenerateList(_armorEquipBtns, GameManager.Instance.GetEquipList(1));
        GenerateList(_shieldEquipBtns, GameManager.Instance.GetEquipList(2));
        GenerateList(_weaponEquipBtns, GameManager.Instance.GetEquipList(3));
    }

    private void GenerateList(List<EquipBtn> equipBtns, List<EquipmentData> equipment)
    {
        for (int i = 0; i < equipBtns.Count; i++)
        {
            equipBtns[i].SetIcon(equipment[i]);
        }
    }

    private void RefreshList(EquipmentData equipment)
    {
        switch (equipment.equipType)
        {
            case EquipmentType.Head:
                GenerateList(_headEquipBtns, GameManager.Instance.GetEquipList(0));
                break;
            case EquipmentType.Armor:
                GenerateList(_armorEquipBtns, GameManager.Instance.GetEquipList(1));
                break;
            case EquipmentType.Shield:
                GenerateList(_shieldEquipBtns, GameManager.Instance.GetEquipList(2));
                break;
            case EquipmentType.Weapon:
                GenerateList(_weaponEquipBtns, GameManager.Instance.GetEquipList(3));
                break;
            default:
                break;
        }
    }
}
