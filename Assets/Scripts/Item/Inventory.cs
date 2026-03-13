using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<EquipmentData> _head;
    [SerializeField] private List<EquipmentData> _armor;
    [SerializeField] private List<EquipmentData> _shield;
    [SerializeField] private List<EquipmentData> _weapon;

    private int[] _equippedNum = new int[4];
    private PlayerStat _equippedStat = new();

    public void Equip(EquipmentData equipment)
    {
        switch (equipment.equipType)
        {
            case EquipmentType.Head:
                _head[_equippedNum[0]].Equipped = false;
                _equippedNum[0] = equipment.EquipNum;
                _head[equipment.EquipNum].Equipped = true;
                _equippedStat.MpRegen = equipment.Effect;
                break;
            case EquipmentType.Armor:
                _armor[_equippedNum[1]].Equipped = false;
                _equippedNum[1] = equipment.EquipNum;
                _armor[equipment.EquipNum].Equipped = true;
                _equippedStat.Hp = equipment.Effect;
                break;
            case EquipmentType.Shield:
                _shield[_equippedNum[2]].Equipped = false;
                _equippedNum[2] = equipment.EquipNum;
                _shield[equipment.EquipNum].Equipped = true;
                _equippedStat.Push = equipment.Effect;
                break;
            case EquipmentType.Weapon:
                _weapon[_equippedNum[3]].Equipped = false;
                _equippedNum[3] = equipment.EquipNum;
                _weapon[equipment.EquipNum].Equipped = true;
                _equippedStat.Damage = equipment.Effect;
                break;
            default:
                break;
        }
        GameManager.Instance.RefreshEquip.Invoke(equipment);
    }

    public void GetItem(EquipmentData equipment)
    {
        switch (equipment.equipType)
        {
            case EquipmentType.Head:
                _head[equipment.EquipNum].Get = true;
                break;
            case EquipmentType.Armor:
                _armor[equipment.EquipNum].Get = true;
                break;
            case EquipmentType.Shield:
                _shield[equipment.EquipNum].Get = true;
                break;
            case EquipmentType.Weapon:
                _weapon[equipment.EquipNum].Get = true;
                break;
            default:
                break;
        }
    }

    public PlayerStat GetStat()
    {
        return _equippedStat;
    }

    public List<EquipmentData> GetEquipList(int index)
    {
        if (index == 0) return _head;
        else if (index == 1) return _armor;
        else if (index == 2) return _shield;
        else return _weapon;

    }
}
