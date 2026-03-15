using System.Collections;
using UnityEngine;

public abstract class SkillBase : ScriptableObject
{
    public string SkillName;
    public int UseMana;
    public float DamageMultiplier;
    public AudioClip ActivationSound;

    public abstract IEnumerator Execute(PlayerController player);
}