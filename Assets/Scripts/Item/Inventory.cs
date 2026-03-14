using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<EquipmentData> _head;
    [SerializeField] private List<EquipmentData> _armor;
    [SerializeField] private List<EquipmentData> _shield;
    [SerializeField] private List<EquipmentData> _weapon;
    private List<EquipmentData> _headRuntime;
    private List<EquipmentData> _armorRuntime;
    private List<EquipmentData> _shieldRuntime;
    private List<EquipmentData> _weaponRuntime;

    private int[] _equippedNum = new int[4];
    private PlayerStat _equippedStat = new();

    void Awake()
    {
        CopyData(_head, _headRuntime);
        CopyData(_armor, _armorRuntime);
        CopyData(_shield, _shieldRuntime);
        CopyData(_weapon, _weaponRuntime);
    }

    // so 복제본 생성
    private void CopyData(List<EquipmentData> data, List<EquipmentData> runtime)
    {
        runtime = new List<EquipmentData>();
        foreach (var original in data)
        {
            runtime.Add(Instantiate(original));
        }
    }

    // 장착
    public void Equip(EquipmentData equipment)
    {
        switch (equipment.equipType)
        {
            case EquipmentType.Head:
                _headRuntime[_equippedNum[0]].Equipped = false;
                _equippedNum[0] = equipment.EquipNum;
                _headRuntime[equipment.EquipNum].Equipped = true;
                _equippedStat.MpRegen = equipment.Effect;
                break;
            case EquipmentType.Armor:
                _armorRuntime[_equippedNum[1]].Equipped = false;
                _equippedNum[1] = equipment.EquipNum;
                _armorRuntime[equipment.EquipNum].Equipped = true;
                _equippedStat.Hp = equipment.Effect;
                break;
            case EquipmentType.Shield:
                _shieldRuntime[_equippedNum[2]].Equipped = false;
                _equippedNum[2] = equipment.EquipNum;
                _shieldRuntime[equipment.EquipNum].Equipped = true;
                _equippedStat.Push = equipment.Effect;
                break;
            case EquipmentType.Weapon:
                _weaponRuntime[_equippedNum[3]].Equipped = false;
                _equippedNum[3] = equipment.EquipNum;
                _weaponRuntime[equipment.EquipNum].Equipped = true;
                _equippedStat.Damage = equipment.Effect;
                break;
            default:
                break;
        }
        GameManager.Instance.RefreshEquip.Invoke(equipment);
    }

    // 장비 획득
    public EquipmentData GetItem(EquipmentType equipment, int index)
    {
        switch (equipment)
        {
            case EquipmentType.Head:
                _headRuntime[index].Get = true;
                return _headRuntime[index];
            case EquipmentType.Armor:
                _armorRuntime[index].Get = true;
                return _armorRuntime[index];
            case EquipmentType.Shield:
                _shieldRuntime[index].Get = true;
                return _shieldRuntime[index];
            case EquipmentType.Weapon:
                _weaponRuntime[index].Get = true;
                return _weaponRuntime[index];
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
        if (index == 0) return _headRuntime;
        else if (index == 1) return _armorRuntime;
        else if (index == 2) return _shieldRuntime;
        else return _weaponRuntime;

    }
}
