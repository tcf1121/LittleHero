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

    public EquipmentData GetItem(EquipmentType equipment, int index)
    {
        switch (equipment)
        {
            case EquipmentType.Head:
                _head[index].Get = true;
                return _head[index];
            case EquipmentType.Armor:
                _armor[index].Get = true;
                return _armor[index];
            case EquipmentType.Shield:
                _shield[index].Get = true;
                return _shield[index];
            case EquipmentType.Weapon:
                _weapon[index].Get = true;
                return _weapon[index];
            default:
                return null;
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
