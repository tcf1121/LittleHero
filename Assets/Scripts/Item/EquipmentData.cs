using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipData", menuName = "SO/EquipData")]
public class EquipmentData : ScriptableObject
{
    public EquipmentType equipType;
    public string EquipName;
    public int EquipNum;
    public int Effect;
    public bool Equipped = false;
    public bool Get = false;

    [Header("Visual Settings")]
    public Sprite EquipSprite;
}

public enum EquipmentType
{
    Head,
    Armor,
    Shield,
    Weapon
}
